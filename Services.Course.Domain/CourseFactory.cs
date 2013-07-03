using System;
using System.Collections.Generic;
using AutoMapper;
using Autofac.Features.Indexed;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate.Version;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Repositories;
using EventStore;
using NHibernate;

namespace BpeProducts.Services.Course.Domain
{
	public class CourseFactory : VersionFactory<Entities.Course>, ICourseFactory
	{
	    private readonly ICourseRepository _courseRepository;

	    public CourseFactory(IStoreEvents store, IIndex<string, IPlayEvent> index, ICourseRepository courseRepository) : base(store, index)
	    {
	        _courseRepository = courseRepository;
	    }

	    public Entities.Course Create(SaveCourseRequest request)
	    {
	        Entities.Course course = null;
            if (request.TemplateCourseId.HasValue)
            {
                var template = Reconstitute(request.TemplateCourseId.Value);
                if (template == null)
                {
                    throw new NotFoundException(string.Format("Course template {0} not found.", request.TemplateCourseId.Value));
                }

                course = BuildFromTemplate(template, request);
            }
            else
            {
                course = BuildFromScratch(request);
            }

	        return course;
	    }

	    public Entities.Course BuildNewVersion(Entities.Course course, string version)
	    {
	        var existing = _courseRepository.GetVersion(course.OriginalEntity.Id, version);

            if(existing != null)
                throw new BadRequestException(string.Format("Course version {0} already exists for CourseId {1}", version, course.OriginalEntity.Id));

	        var newVersion = new Entities.Course
	            {
                    Id = Guid.NewGuid(),
                    OriginalEntity = course.OriginalEntity,
                    ParentEntity = course,
	                Name = course.Name,
	                Code = course.Code,
	                Description = course.Description,

	                Programs = new List<Program>(course.Programs),
	                Segments = new List<CourseSegment>(course.Segments),
	                Outcomes = new List<LearningOutcome>(course.Outcomes),

	                CourseSegmentJson = course.CourseSegmentJson,
	                TenantId = course.TenantId,
                    VersionNumber = version,
                    OrganizationId = course.OrganizationId,

                    Template = course.Template,
                    ActiveFlag = true
	            };

	        return newVersion;
	    }

        private Entities.Course BuildFromTemplate(Entities.Course template, SaveCourseRequest request)
        {
            var course = Mapper.Map<Entities.Course>(request);

            course.Id = Guid.NewGuid();
            course.CourseType = template.CourseType;
            course.Programs = new List<Program>(template.Programs);
            course.Segments = new List<CourseSegment>(template.Segments);
            course.Outcomes = new List<LearningOutcome>(template.Outcomes);
            course.CourseSegmentJson = template.CourseSegmentJson;
            course.TenantId = request.TenantId;
            course.VersionNumber = new Version(1, 0, 0, 0).ToString();
            course.OrganizationId = request.OrganizationId;
            course.Template = template;
            course.ActiveFlag = true;
            course.SetOriginalEntity(course);

            return course;
        }

        private Entities.Course BuildFromScratch(SaveCourseRequest request)
        {
            var course = Mapper.Map<Entities.Course>(request);

            course.Id = Guid.NewGuid();
            course.ActiveFlag = true;
            course.OrganizationId = request.OrganizationId;
            course.VersionNumber = new Version(1, 0, 0, 0).ToString();
            course.CourseType = request.CourseType;
            course.SetOriginalEntity(course);

            return course;
        }
	}
}