﻿using System;
using System.Linq;
using AutoMapper;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.CourseAggregates;
using BpeProducts.Services.Course.Domain.ProgramAggregates;

namespace BpeProducts.Services.Course.Domain
{
    public static class MapperConfiguration
    {
        public static void Configure()
        {
            CourseMappings();
            ProgramMappings();
            OutcomeMappings();
        }

        private static void ProgramMappings()
        {
            Mapper.CreateMap<Program, ProgramResponse>();
            //.ForMember(dest => dest.Courses, opt => opt.MapFrom(program => program.Courses.Select(c => new CourseInfoResponse
            //    {
            //        Id = c.Id,
            //        Name = c.Name,
            //        Code = c.Code
            //    }).ToList()));
            // this option is not needed
            Mapper.CreateMap<SaveProgramRequest, Program>();
            Mapper.CreateMap<UpdateProgramRequest, Program>();
        }

        private static void CourseMappings()
        {
            // From Domain entities to DTOs
            Mapper.CreateMap<Domain.CourseAggregates.Course, CourseInfoResponse>()
                  .ForMember(dest => dest.ProgramIds, opt => opt.MapFrom(course => course.Programs.Select(p => p.Id).ToList()))
                  .ForMember(dest => dest.TemplateCourseId, opt => opt.MapFrom(course => course.Template == null ? Guid.Empty : course.Template.Id))
                  .ForMember(dest => dest.PrerequisiteCourseIds, opt => opt.MapFrom(course => course.Prerequisites.Select(p => p.Id).ToList()));
            Mapper.CreateMap<CourseSegment, CourseSegmentInfo>();
            Mapper.CreateMap<CourseLearningActivity, CourseLearningActivityResponse>();

            // From DTOs to Domain Entities
            Mapper.CreateMap<SaveCourseRequest, Domain.CourseAggregates.Course>()
                  .ForMember(x => x.Id, opt => opt.Ignore());
            Mapper.CreateMap<SaveCourseSegmentRequest, CourseSegmentInfo>();
            Mapper.CreateMap<CourseSegmentInfo, CourseSegment>();

            Mapper.CreateMap<SaveCourseLearningActivityRequest, CourseLearningActivity>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            Mapper.CreateMap<LearningMaterialRequest, LearningMaterial>();

            Mapper.CreateMap<UpdateLearningMaterialRequest, LearningMaterial>();

            //for course deep copy purpose
            Mapper.CreateMap<Domain.CourseAggregates.Course, Domain.CourseAggregates.Course>()
                  .ForMember(dest => dest.IsPublished, opt => opt.UseValue(false));

            Mapper.CreateMap<CourseSegment, CourseSegment>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(source => Guid.NewGuid()))
                  .ForMember(dest => dest.SourceCourseSegmentId, opt => opt.MapFrom(source => source.Id));

            Mapper.CreateMap<CourseLearningActivity, CourseLearningActivity>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(source => Guid.NewGuid()))
                  .ForMember(dest => dest.SourceCourseLearningActivityId, opt => opt.MapFrom(source => source.Id));

            Mapper.CreateMap<LearningMaterial, LearningMaterial>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(source => Guid.NewGuid()))
                  .ForMember(dest => dest.SourceLearningMaterialId, opt => opt.MapFrom(source => source.Id));

            //for simple course info
            Mapper.CreateMap<Domain.CourseAggregates.Course, SimpleCourseInfoResponse>();
        }

        private static void OutcomeMappings()
        {
        }
    }
}