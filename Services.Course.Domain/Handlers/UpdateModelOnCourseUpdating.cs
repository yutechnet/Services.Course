using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using NHibernate;
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

		    Mapper.Map(e.Request, course); // dangerous, should use immutable collections in entity
   
			course.Programs.Clear();
		    var programs = _programRepository.Get(e.Request.ProgramIds);
	        course.SetPrograms(programs);

            _courseRepository.Save(course);
		}
	}

    public interface IProgramRepository
    {
        Program Get(Guid id);
        IList<Program> Get(List<Guid> ids);
    }

    class ProgramRepository : IProgramRepository
    {
        private readonly ISession _session;

        public ProgramRepository(ISession session)
        {
            _session = session;
        }

        public Program Get(Guid id)
        {
            return _session.Get<Program>(id);
        }

        public IList<Program> Get(List<Guid> ids)
        {
            var programs = new List<Program>();
            foreach (var id in ids)
            {
                var program = _session.Get<Program>(id);

                if(program == null)
                    throw new BadRequestException(string.Format("Program {0} does not exist", id));

                programs.Add(program);
            }

            return programs;
        }
    }
}