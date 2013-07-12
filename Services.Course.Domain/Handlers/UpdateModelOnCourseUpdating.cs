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
	    private readonly IRepository _repository;

	    public UpdateModelOnCourseUpdating(IRepository repository)
        {
	        _repository = repository;
        }

	    public void Handle(IDomainEvent domainEvent)
		{
			var e = domainEvent as CourseUpdated;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }

	        var course = _repository.Get<Entities.Course>(e.AggregateId);

		    Mapper.Map(e.Request, course); // dangerous, should use immutable collections in entity
   
			course.Programs.Clear();
	        var programs = _repository.Query<Program>().Where(p => e.Request.ProgramIds.Contains(p.Id));
	        course.SetPrograms(programs.ToList());

	        _repository.Save(course);
		}
	}
}