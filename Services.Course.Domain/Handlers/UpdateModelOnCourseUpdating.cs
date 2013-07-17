using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using NHibernate.Linq;

namespace BpeProducts.Services.Course.Domain.Handlers
{
	public class UpdateModelOnCourseUpdating : IHandle<CourseUpdated>
	{
        private readonly ICourseRepository _courseRepository;
	    private readonly IProgramRepository _programRepository;

	    public UpdateModelOnCourseUpdating(ICourseRepository courseRepository, IProgramRepository programRepository)
	    {
	        _courseRepository = courseRepository;
	        _programRepository = programRepository;
	    }

	    public void Handle(IDomainEvent domainEvent)
		{
            var e = domainEvent as CourseUpdated;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }

	        var course = _courseRepository.Get(e.AggregateId);

	        course.Name = e.Request.Name;
	        course.Description = e.Request.Description;
	        course.Code = e.Request.Code;
	        course.IsTemplate = e.Request.IsTemplate;
	        course.CourseType = e.Request.CourseType;

		    //Mapper.Map(e.Request, course); // dangerous, should use immutable collections in entity

            course.Programs.Clear();
            var programs = _programRepository.Get(e.Request.ProgramIds);
            course.SetPrograms(programs.ToList());

	        _courseRepository.Save(course);
		}
	}
}