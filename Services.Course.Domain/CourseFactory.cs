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
		    var courseId = Guid.NewGuid();

	        Entities.Course template = null;
	        if (request.TemplateCourseId.HasValue)
	        {
	            template = new Entities.Course {Id = request.TemplateCourseId.Value};
	        }

	        var course = new Entities.Course
			    {
                    Id = courseId,
                    Template = template,
                    ActiveFlag = true,
                    OrganizationId = request.OrganizationId,
                    VersionNumber = new Version(1, 0, 0, 0).ToString(),
                    CourseType = request.CourseType,
                    IsTemplate = false ,
                    TenantId = request.TenantId
                };
            course.SetOriginalEntity(course);

			Mapper.Map(request, course);
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

                    Template = course.Template
	            };

	        return newVersion;
	    }

        public Entities.Course BuildFromTemplate(Entities.Course template)
        {
            var courseId = Guid.NewGuid();
            var course = new Entities.Course
            {
                Id = courseId,
                Name = template.Name,
                Code = template.Code,
                Description = template.Description,

                Programs = new List<Program>(template.Programs),
                Segments = new List<CourseSegment>(template.Segments),
                Outcomes = new List<LearningOutcome>(template.Outcomes),

                CourseSegmentJson = template.CourseSegmentJson,
                TenantId = template.TenantId,
                VersionNumber = new Version(1, 0, 0, 0).ToString(),
                OrganizationId = template.OrganizationId,

                Template = template,
            };
            course.SetOriginalEntity(course);

            return course;
        }
	}
}