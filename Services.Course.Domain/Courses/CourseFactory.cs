using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using AutoMapper;
using Autofac.Features.Indexed;
using BpeProducts.Common.Capabilities;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate;
using BpeProducts.Common.WebApi.Authorization;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Repositories;
using EventStore;

namespace BpeProducts.Services.Course.Domain.Courses
{
	public class CourseFactory : VersionFactory<Course>, ICourseFactory
	{
        private readonly IRepository _courseRepository;

        public CourseFactory(IStoreEvents store, IIndex<string, IPlayEvent> index, IRepository courseRepository)
            : base(store, index, courseRepository)
	    {
	        _courseRepository = courseRepository;
	    }

		[ClaimsAuthorize(Capability.CourseCreate)]
		public Course Create(SaveCourseRequest request)
	    {
	        Course course = null;
            if (request.TemplateCourseId.HasValue)
            {
               // var template = Reconstitute(request.TemplateCourseId.Value);
                var template = _courseRepository.Get<Domain.Courses.Course>(request.TemplateCourseId.Value);
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

        protected Course BuildFromTemplate(Course template, SaveCourseRequest request)
        {
            
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

            //var pgms = new List<Program>();
            //foreach (var program in template.Programs)
            //{
            //    pgms.Add(program.DeepClone());
            //}
            //course.Programs = pgms;

            course.Programs = new List<Program>(template.Programs);


            course.SupportedOutcomes = new List<LearningOutcome>(template.SupportedOutcomes);

            course.CourseType = template.CourseType;
            course.Segments = newSegments;
            course.TenantId = template.TenantId;
            course.Template = template;
            course.ActiveFlag = true;
            course.IsTemplate = request.IsTemplate;

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
                CourseType = request.CourseType,
            };

            return course;
        }
	}
}