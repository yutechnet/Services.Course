using System.Linq;
using AutoMapper;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
using CourseSegment = BpeProducts.Services.Course.Domain.Entities.CourseSegment;

namespace BpeProducts.Services.Course.Host.App_Start
{
	public static class MapperConfig
	{
		public static void ConfigureMappers()
		{
			CourseMappings();
			ProgramMappings();
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
			Mapper.CreateMap<CourseSegment, Contract.CourseSegment>();

			// From DTOs to Domain Entities
			Mapper.CreateMap<SaveCourseRequest, Domain.Entities.Course>()
			      .ForMember(x => x.Id, opt => opt.Ignore());
			Mapper.CreateMap<Contract.CourseSegment, CourseSegment>();
		}
	}
}