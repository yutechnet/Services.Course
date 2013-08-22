using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain
{
    public class CourseService : ICourseService
    {
        private readonly ICourseFactory _courseFactory;
        private readonly IDomainEvents _domainEvents;
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseFactory courseFactory, IDomainEvents domainEvents, ICourseRepository courseRepository)
        {
            _courseFactory = courseFactory;
            _domainEvents = domainEvents;
            _courseRepository = courseRepository;
        }

        public CourseInfoResponse Create(SaveCourseRequest request)
        {
            var course = _courseFactory.Create(request);
            _domainEvents.Raise<CourseCreated>(new CourseCreated
            {
                AggregateId = course.Id,
                Course = course
            });

            if (request.TemplateCourseId.HasValue)
            {
                // This is a workaround for CourseCreated event, when reconstituted, fails to correctly serialize Program collection
                foreach (var program in course.Programs)
                {
                    _domainEvents.Raise<CourseAssociatedWithProgram>(new CourseAssociatedWithProgram
                    {
                        AggregateId = course.Id,
                        Description = program.Description,
                        Name = program.Name,
                        ProgramId = program.Id,
                        ProgramType = program.ProgramType
                    });
                }

            }

            return Mapper.Map<CourseInfoResponse>(course);
        }

        public void Update(Guid courseId, UpdateCourseRequest request)
        {
            var course = _courseFactory.Reconstitute(courseId);

            if (course == null || !course.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Course {0} not found.", courseId));
            }

            _domainEvents.Raise<CourseUpdated>(new CourseUpdated
            {
                AggregateId = courseId,
                Old = course,
                Request = request
            });

        }

        public CourseInfoResponse Get(Guid courseId)
        {
            var course = _courseRepository.Load(courseId);

            var courseInfo = Mapper.Map<CourseInfoResponse>(course);

            courseInfo.Segments = (from s in courseInfo.Segments 
                                   where s.ParentSegmentId == Guid.Empty 
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
            var course = _courseFactory.Reconstitute(courseId);

            if (course == null || !course.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Course {0} not found.", courseId));
            }

            if (course.IsPublished)
            {
                throw new ForbiddenException(string.Format("Course {0} is published and cannot be deleted.", courseId));
            }

            _domainEvents.Raise<CourseDeleted>(new CourseDeleted
            {
                AggregateId = courseId,
            });
        }

        public void UpdatePrerequisiteList(Guid courseId, List<Guid> prerequisiteIds)
        {
            var course = _courseRepository.Get(courseId);

			// Can't work directly off of course below since NHB will update the prereq list with any additions
	        var beforeStateOfPrerequisites = new List<Guid>();
			course.Prerequisites.Each(p => beforeStateOfPrerequisites.Add(p.Id));

            if (course == null || !course.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Course {0} not found.", courseId));
            }


            // Determine prerequisites added
            foreach (var incomingPrereqId in prerequisiteIds)
            {
                // If all of the current prerequisite courses do not equal this incoming prerequisiteId, it's new
                if (course.Prerequisites.All(existingPrereqs => existingPrereqs.Id != incomingPrereqId))
                {
                    _domainEvents.Raise<CoursePrerequisiteAdded>(new CoursePrerequisiteAdded
                    {
                        AggregateId = courseId,
                        PrerequisiteCourseId = incomingPrereqId
                    });
                }
            }

            // Determine prerequisites removed
			foreach (var currentPrereqCourse in beforeStateOfPrerequisites)
            {
                // If all of the incoming prerequisite Ids do not equal this existing prerequisiteId, it's been removed
                if (prerequisiteIds.All(incomingPrereqIds => incomingPrereqIds != currentPrereqCourse))
                {
                    _domainEvents.Raise<CoursePrerequisiteRemoved>(new CoursePrerequisiteRemoved
                    {
                        AggregateId = courseId,
                        PrerequisiteCourseId = currentPrereqCourse
                    });
                }
            }
        }
    }
}
