using System;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Handlers
{
	public class UpdateModelOnCourseDeletion : IHandle<CourseDeleted>
	{
	    private readonly IRepository _repository;

	    public UpdateModelOnCourseDeletion(IRepository repository)
		{
		    _repository = repository;
		}

	    public void Handle(IDomainEvent domainEvent)
		{
			var e = domainEvent as CourseDeleted;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }

	        var course = _repository.Get<Courses.Course>(e.AggregateId);
	        course.ActiveFlag = false;
	        _repository.Save(course);
		}
	}
}