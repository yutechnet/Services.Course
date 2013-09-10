//using System.Web.Http.Odata.Query;

using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BpeProducts.Services.Course.Domain.Handlers
{
	public class UpdateModelOnCourseCreation : IHandle<CourseCreated>
	{
	    private readonly IRepository _repository;

	    public UpdateModelOnCourseCreation(IRepository repository)
		{
	        _repository = repository;
		}

	    public void Handle(IDomainEvent domainEvent)
		{
			var e = domainEvent as CourseCreated;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }

	        _repository.Save(e.Course);
        }
	}
}
