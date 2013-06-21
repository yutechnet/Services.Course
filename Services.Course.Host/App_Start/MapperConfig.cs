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
		    Mapper.CreateMap<Domain.Entities.Course, CourseInfoResponse>()
		          .ForMember(dest => dest.ProgramIds, opt => opt.MapFrom(course => course.Programs.Select(p => p.Id).ToList()));

			// From DTOs to Domain Entities
			Mapper.CreateMap<SaveCourseRequest, Domain.Entities.Course>()
			      .ForMember(x => x.Id, opt => opt.Ignore());
			Mapper.CreateMap<Contract.SaveCourseSegmentRequest, CourseSegment>();

		    Mapper.CreateMap<CourseSegmentAdded, CourseSegment>();

		    Mapper.CreateMap<ContentRequest, Content>();
		}

        private static void OutcomeMappings()
        {
            Mapper.CreateMap<LearningOutcome, OutcomeResponse>();
            Mapper.CreateMap<OutcomeRequest, LearningOutcome>();
        }
	}
}