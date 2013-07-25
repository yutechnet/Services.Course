﻿using System;
using System.Linq;
using AutoMapper;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Host.App_Start
{
	public static class MapperConfig
	{
		public static void ConfigureMappers()
		{
			CourseMappings();
			ProgramMappings();
            OutcomeMappings();
		}

		private static void ProgramMappings()
		{
			Mapper.CreateMap<Program, ProgramResponse>();
			Mapper.CreateMap<SaveProgramRequest, Program>();
		}

		private static void CourseMappings()
		{
			// From Domain entities to DTOs
		    Mapper.CreateMap<Domain.Courses.Course, CourseInfoResponse>()
		          .ForMember(dest => dest.ProgramIds, opt => opt.MapFrom(course => course.Programs.Select(p => p.Id).ToList()))
                  .ForMember(dest => dest.TemplateCourseId, opt=> opt.MapFrom(course => course.Template == null ? Guid.Empty : course.Template.Id));
		    Mapper.CreateMap<Domain.Courses.CourseSegment, CourseSegmentInfo>();

			// From DTOs to Domain Entities
			Mapper.CreateMap<SaveCourseRequest, Domain.Courses.Course>()
			      .ForMember(x => x.Id, opt => opt.Ignore());
			Mapper.CreateMap<Contract.SaveCourseSegmentRequest, CourseSegmentInfo>();
		    Mapper.CreateMap<CourseSegmentInfo, Domain.Courses.CourseSegment>();
            Mapper.CreateMap<Domain.Courses.CourseSegment, Domain.Courses.CourseSegment>();

		    Mapper.CreateMap<CourseSegmentAdded, CourseSegmentInfo>();
		}

        private static void OutcomeMappings()
        {
            Mapper.CreateMap<LearningOutcome, OutcomeInfo>();
            Mapper.CreateMap<OutcomeRequest, LearningOutcome>();
        }
	}
}