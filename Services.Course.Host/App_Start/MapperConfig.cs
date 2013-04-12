using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using BpeProducts.Services.Course.Contract;

namespace BpeProducts.Services.Course.Host.App_Start
{
    public static class MapperConfig
    {
        public static void ConfigureMappers()
        {
            CourseMappings();
        }

        private static void CourseMappings()
        {
            // From Domain entities to DTOs
            Mapper.CreateMap<Domain.Entities.Course, CourseInfoResponse>();

            // From DTOs to Domain Entities
            Mapper.CreateMap<SaveCourseRequest, Domain.Entities.Course>()
                  .ForMember(x => x.Id,opt=>opt.Ignore())
                  .ForMember(x=> x.DateAdded,opt => opt.Ignore())
                  .ForMember(x => x.DateUpdated, opt => opt.Ignore());
        }
    }
}