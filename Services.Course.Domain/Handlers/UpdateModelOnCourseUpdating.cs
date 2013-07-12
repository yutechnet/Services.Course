using System;
using System.Linq;
using AutoMapper;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using NHibernate.Linq;

namespace BpeProducts.Services.Course.Domain.Handlers
{
	public class UpdateModelOnCourseUpdating : IHandle<CourseUpdated>
	{
	    private readonly IProgramRepository _programRepository;
	    private readonly IRepository _repository;

	    public UpdateModelOnCourseUpdating(ICourseRepository courseRepository, IProgramRepository programRepository, IRepository repository)
        {
            _programRepository = programRepository;
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
		    var programs = _programRepository.Get(e.Request.ProgramIds);
	        course.SetPrograms(programs);

	        _repository.Save(course);
		}
	}
}