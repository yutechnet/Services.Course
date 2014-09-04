using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.ProgramAggregates;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.CourseAggregates
{
    public class CourseFactory : ICourseFactory
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IAssessmentClient _assessmentClient;
        private readonly IProgramRepository _programRepository;

        public CourseFactory(ICourseRepository courseRepository, IAssessmentClient assessmentClient, IProgramRepository programRepository)
        {
            _courseRepository = courseRepository;
            _assessmentClient = assessmentClient;
            _programRepository = programRepository;
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
                        CorrelationId = request.CorrelationId  
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
            var course = template.CloneSelf();

            if (!string.IsNullOrEmpty(request.Name))
                course.Name = request.Name;
            if (!string.IsNullOrEmpty(request.Code))
                course.Code = request.Code;
            if (!string.IsNullOrEmpty(request.Description))
                course.Description = request.Description;
            course.CorrelationId = !string.IsNullOrEmpty(request.CorrelationId) ? request.CorrelationId : null;
            course.OrganizationId = request.OrganizationId;
            course.TenantId = request.TenantId;
            course.Template = template;
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
                    IsDeleted = false,
                    CourseType = request.CourseType,
                    Credit = request.Credit,
                    MetaData = request.MetaData,
                    ExtensionAssets = request.ExtensionAssets,
                    Prerequisites = request.PrerequisiteCourseIds.Select(x=>_courseRepository.GetOrThrow(x)).ToList(),
                    CorrelationId = request.CorrelationId  
                };

            var programs = _programRepository.Get(request.ProgramIds);
            if (programs!=null)
                course.SetPrograms(programs.ToList());

            return course;
        }
    }
}