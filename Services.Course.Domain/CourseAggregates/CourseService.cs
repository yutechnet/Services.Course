using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BpeProducts.Common.Authorization;
using BpeProducts.Common.Contract;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Authorization.Contract;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Contract.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using NServiceBus;


namespace BpeProducts.Services.Course.Domain.CourseAggregates
{
    public class CourseService : ICourseService
    {
        private readonly ICourseFactory _courseFactory;
        private readonly ICourseRepository _courseRepository;
	    private readonly ICoursePublisher _coursePublisher;
        private readonly IAssessmentClient _assessmentClient;
        private readonly IProgramRepository _programRepository;
	    private readonly IBus _bus;

	    public CourseService(ICourseFactory courseFactory, 
			ICourseRepository courseRepository, 
            ICoursePublisher coursePublisher, 
			IAssessmentClient assessmentClient, 
			IProgramRepository programRepository,IBus bus)
        {
            _courseFactory = courseFactory;
            _courseRepository = courseRepository;
			_coursePublisher = coursePublisher;
	        _assessmentClient = assessmentClient;
            _programRepository = programRepository;
	        _bus = bus;
        }

		[AuthByAcl(Capability = Capability.CourseCreate, OrganizationObject = "request")]
        public CourseInfoResponse Create(SaveCourseRequest request)
        {
            var course = _courseFactory.Build(request);
            _courseRepository.Save(course);
			_bus.Publish(
				new CourseCreated
					{
						Id=course.Id,
						OrganizationId = course.OrganizationId,
						Type="course"
					});
			return Mapper.Map<CourseInfoResponse>(course);
        }

        [AuthByAcl(Capability = Capability.CourseCreate, OrganizationObject = "request")]
        public CourseInfoResponse Create(CreateCourseFromTemplateRequest request)
        {
            var course = _courseFactory.Build(request);
            _courseRepository.Save(course);
			_bus.Publish(
				new CourseCreated
				{
					Id = course.Id,
					OrganizationId = course.OrganizationId,
					Type = "course"
				});
            return Mapper.Map<CourseInfoResponse>(course);
        }

        public void UpdateActiviationStatus(Guid courseId, ActivationRequest request)
        {
            var course = _courseRepository.GetOrThrow(courseId);
            course.UpdateActivationStatus(request);
            _courseRepository.Save(course);
        }

        public void Update(Guid courseId, UpdateCourseRequest request)
        {
            var course = _courseRepository.GetOrThrow(courseId);

            course.Name = request.Name;
            course.Description = request.Description;
            course.Code = request.Code;
            course.IsTemplate = request.IsTemplate;
            course.CourseType = request.CourseType;
            course.Credit = request.Credit;
            course.MetaData = request.MetaData;
            course.ExtensionAssets = request.ExtensionAssets;

            var programs = _programRepository.Get(request.ProgramIds);
            course.SetPrograms(programs.ToList());

            _courseRepository.Save(course);

			_bus.Publish(
				new CourseUpdated
				{
					Id = course.Id,
					OrganizationId = course.OrganizationId,
					Type = "course"
				});
        }

        [AuthByAcl(Capability = Capability.CourseView, ObjectId = "courseId", ObjectType = typeof(Course))]
        public CourseInfoResponse Get(Guid courseId)
        {
            var course = _courseRepository.GetOrThrow(courseId);

            var courseInfo = Mapper.Map<CourseInfoResponse>(course);

            courseInfo.Segments = (from s in courseInfo.Segments 
                                   where !s.ParentSegmentId.HasValue
                                   select s).ToList();
            return courseInfo;
        }

        public IEnumerable<CourseInfoResponse> Search(string queryString)
        {
            var queryArray = queryString.Split('?');
            var courses = _courseRepository.ODataQuery(queryArray.Length > 1 ? queryArray[1] : "");

            var courseResponses = new List<CourseInfoResponse>();
            Mapper.Map(courses, courseResponses);
            return courseResponses;
        }

        public void Delete(Guid courseId)
        {
            var course = _courseRepository.GetOrThrow(courseId);

            course.Delete();
            _courseRepository.Save(course);
        }

        public void UpdatePrerequisiteList(Guid courseId, List<Guid> newPrerequisiteIds)
        {
            var course = _courseRepository.GetOrThrow(courseId);

            var existing = (from prereq in course.Prerequisites
                            select prereq.Id).ToList();

            var removed = (from e in existing
                           where !newPrerequisiteIds.Contains(e)
                           select e).ToList();

            var added = (from n in newPrerequisiteIds
                         where !existing.Contains(n)
                         select n).ToList();

            foreach (var toAdd in added)
            {
                var prerequisiteCourse = _courseRepository.GetOrThrow(toAdd);

                course.AddPrerequisite(prerequisiteCourse);
            }

            foreach (var toRemove in removed)
            {
                course.RemovePrerequisite(toRemove);
            }

            _courseRepository.Save(course);
        }

        public IEnumerable<CourseInfoResponse> GetPublishedCourses(Guid organizationId)
        {
            var courses = _courseRepository.GetPublishedCourses(organizationId);
            return Mapper.Map<IEnumerable<CourseInfoResponse>>(courses);
        }

	    public CourseInfoResponse CreateVersion(Guid parentVersionId, string versionNumber)
	    {
		    var parentVersion = _courseRepository.Get(parentVersionId);

			if (parentVersion == null)
			{
				throw new BadRequestException(string.Format("Parent course {0} is not found.", parentVersionId));
			}

			if (_courseRepository.GetVersion(parentVersion.OriginalEntity.Id, versionNumber) != null)
			{
				throw new BadRequestException(string.Format("Version {0} for Course {1} already exists.", versionNumber, parentVersion.OriginalEntity.Id));
			}

			var newVersion = parentVersion.CreateVersion(versionNumber) as Course;
            if (newVersion == null)
            {
                throw new Exception(string.Format("Failed to create a new version {0} from the parent version {1}", versionNumber, parentVersion.Id));
            }

			newVersion.CloneOutcomes(_assessmentClient);
            _courseRepository.Save(newVersion);
	        return Mapper.Map<CourseInfoResponse>(newVersion);
	    }

	    public void PublishVersion(Guid courseId, string publishNote)
	    {
			var course = _courseRepository.GetOrThrow(courseId);
			course.Publish(publishNote, _coursePublisher);
            _courseRepository.Save(course);
		}
    }
}
