using System;
using System.Collections.Generic;
using AutoMapper;
using BpeProducts.Common.Authorization;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
using Services.Assessment.Contract;
using Services.Authorization.Contract;

namespace BpeProducts.Services.Course.Domain.Courses
{
    public class CourseFactory : VersionFactory<Course>, ICourseFactory
    {
        private readonly IRepository _courseRepository;
        private readonly IAssessmentClient _assessmentClient;
        public CourseFactory(IRepository courseRepository, IAssessmentClient assessmentClient)
            : base(courseRepository)
        {
            _courseRepository = courseRepository;
            _assessmentClient = assessmentClient;
        }


        public Course Create(SaveCourseRequest request)
        {
            if (request.TemplateCourseId.HasValue)
            {
                // TODO: This block should be removed once Jenga migrates to using the separate endpoint for creating course from Template
                var createCourseFromTemplateRequest = new CreateCourseFromTemplateRequest
                    {
                        Code = request.Code,
                        Description = request.Description,
                        IsTemplate = request.IsTemplate,
                        Name = request.Name,
                        OrganizationId = request.OrganizationId,
                        TemplateCourseId = request.TemplateCourseId.Value,
                        TenantId = request.TenantId
                    };
                return Create(createCourseFromTemplateRequest);
            }
            else
            {
                return BuildFromScratch(request);                
            }
        }


        public Course Create(CreateCourseFromTemplateRequest request)
        {
            // var template = Reconstitute(request.TemplateCourseId.Value);
            var template = _courseRepository.Get<Course>(request.TemplateCourseId);
            if (template == null)
            {
                throw new BadRequestException(string.Format("Course template {0} not found.", request.TemplateCourseId));
            }

            return BuildFromTemplate(template, request);
        }

        protected Course BuildFromTemplate(Course template, CreateCourseFromTemplateRequest request)
        {
            var course = new Course
                {
                    Id = Guid.NewGuid(),
                    Name = string.IsNullOrEmpty(request.Name) ? template.Name : request.Name,
                    Code = string.IsNullOrEmpty(request.Code) ? template.Code : request.Code,
                    Description = string.IsNullOrEmpty(request.Description) ? template.Description : request.Description,
                    OrganizationId = request.OrganizationId,
                    TenantId = request.TenantId
				};

			var newSegments = Mapper.Map<List<CourseSegment>>(template.Segments);
			foreach (CourseSegment courseSegment in newSegments)
			{
				courseSegment.Id = Guid.NewGuid();
				courseSegment.Course = course;
			}

            foreach (CourseSegment courseSegment in newSegments)
            {
                courseSegment.Id = Guid.NewGuid();
                courseSegment.Course = course;
                foreach (LearningMaterial learningMaterial in courseSegment.LearningMaterials)
                {
                    learningMaterial.CourseSegment = courseSegment;
                    learningMaterial.CloneOutcomes(_assessmentClient);
                }
            }

            course.Programs = new List<Program>(template.Programs);
            course.CourseType = template.CourseType;
            course.Segments = newSegments;
            course.TenantId = template.TenantId;
            course.Template = template;
            course.Credit = template.Credit;
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
                    Credit = request.Credit
                };

            return course;
        }
    }
}