using System;
using System.Collections.Generic;
using AutoMapper;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.ProgramAggregates;

namespace BpeProducts.Services.Course.Domain.CourseAggregates
{
    public class CourseFactory : ICourseFactory
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IAssessmentClient _assessmentClient;

        public CourseFactory(ICourseRepository courseRepository, IAssessmentClient assessmentClient)
        {
            _courseRepository = courseRepository;
            _assessmentClient = assessmentClient;
        }
        
        public Course Build(SaveCourseRequest request)
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
                        TenantId = request.TenantId,
                    };
                return Build(createCourseFromTemplateRequest);
            }

            return BuildFromScratch(request);
        }


        public Course Build(CreateCourseFromTemplateRequest request)
        {
            var template = _courseRepository.GetOrThrow(request.TemplateCourseId);
            
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
                    TenantId = request.TenantId,
                    MetaData = template.MetaData,
                    ExtensionAssets = template.ExtensionAssets,
				};

            var newLearningMaterials = Mapper.Map<List<LearningMaterial>>(template.LearningMaterials);
            foreach (LearningMaterial learningMaterial in newLearningMaterials)
            {
                learningMaterial.Id = Guid.NewGuid();
                learningMaterial.Course = course;
            }

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
                    learningMaterial.Course = course;
                    learningMaterial.CloneOutcomes(_assessmentClient);
                }
            }

            course.Programs = new List<Program>(template.Programs);
            course.CourseType = template.CourseType;
            course.Segments = newSegments;
            course.TenantId = template.TenantId;
            course.Template = template;
            course.Credit = template.Credit;
            course.IsDeleted = false;
            course.IsTemplate = request.IsTemplate;
            course.LearningMaterials = newLearningMaterials.FindAll(x => x.CourseSegment == null);
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
                    IsDeleted = false,
                    CourseType = request.CourseType,
                    Credit = request.Credit,
                    MetaData = request.MetaData,
                    ExtensionAssets = request.ExtensionAssets
                };

            return course;
        }
    }
}