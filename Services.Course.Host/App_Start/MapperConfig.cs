using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Host.Controllers;

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
			Mapper.CreateMap<CreateProgramRequest, Program>();
	    }

	    private static void CourseMappings()
        {
            // From Domain entities to DTOs
            Mapper.CreateMap<Domain.Entities.Course, CourseInfoResponse>();

            // From DTOs to Domain Entities
            Mapper.CreateMap<SaveCourseRequest, Domain.Entities.Course>()
                  .ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}