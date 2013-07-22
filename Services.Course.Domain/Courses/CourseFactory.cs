using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using Autofac.Features.Indexed;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Repositories;
using EventStore;

namespace BpeProducts.Services.Course.Domain.Courses
{
	public class CourseFactory : VersionFactory<Course>, ICourseFactory
	{
	    private readonly ICourseRepository _courseRepository;

	    public CourseFactory(IStoreEvents store, IIndex<string, IPlayEvent> index, ICourseRepository courseRepository) : base(store, index)
	    {
	        _courseRepository = courseRepository;
	    }

	    public Course Create(SaveCourseRequest request)
	    {
	        Course course = null;
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

	        course.IsBuilt = true;

	        return course;
	    }

        protected Course BuildFromTemplate(Course template, SaveCourseRequest request)
        {
            //var course = Mapper.Map<Entities.Course>(request);

            var course = new Course
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Code = request.Code,
                    Description = request.Description,
                    OrganizationId = request.OrganizationId,
                };

            var newSegments = Mapper.Map<List<CourseSegment>>(template.Segments);
            foreach (var courseSegment in newSegments)
            {
                courseSegment.Id = Guid.NewGuid();
                courseSegment.Course = course;
            }

            course.Programs = new List<Program>(template.Programs);
            course.Outcomes = new List<LearningOutcome>(template.Outcomes);

            course.CourseType = template.CourseType;
            course.Segments = newSegments;
            course.TenantId = template.TenantId;
            course.VersionNumber = new Version(1, 0, 0, 0).ToString();
            course.Template = template;
            course.ActiveFlag = true;
            course.SetOriginalEntity(course);
            course.IsTemplate = request.IsTemplate;
            course.IsBuilt = true;

            return course;
        }

	    protected Course BuildFromScratch(SaveCourseRequest request)
        {
            var course = new Course
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Code = request.Code,
                Description = request.Description,
                OrganizationId = request.OrganizationId,
                TenantId = request.TenantId,
                ActiveFlag = true,
                VersionNumber = new Version(1, 0, 0, 0).ToString(),
                CourseType = request.CourseType,
                IsBuilt = true
            };
            course.SetOriginalEntity(course);

            return course;
        }
	}
}