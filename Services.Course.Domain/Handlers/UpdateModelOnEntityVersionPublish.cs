using System;
using System.Collections.Generic;
using BpeProducts.Common.NHibernate;
using BpeProducts.Common.NHibernate.Version;
using BpeProducts.Services.Course.Domain.Courses;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using BpeProducts.Services.Course.Domain.Validation;

namespace BpeProducts.Services.Course.Domain.Handlers
{
    public class UpdateModelOnEntityVersionPublish : IHandle<VersionPublished>
    {
        private readonly IRepository _repository;
	    private readonly ICoursePublisher _coursePublisher;

	    public UpdateModelOnEntityVersionPublish(IRepository repository, ICoursePublisher coursePublisher)
		{
			_repository = repository;
			_coursePublisher = coursePublisher;
		}

	    public void Handle(IDomainEvent domainEvent)
        {
            var e = domainEvent as VersionPublished;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }

            var entity = _repository.Get(e.EntityType, e.AggregateId) as VersionableEntity;
            
            if (entity == null) return;

			if (entity is Courses.Course)
			{
				var c = (Courses.Course) entity;
				c.Publish(e.PublishNote, _coursePublisher);

			}
			else
			{
				entity.Publish(e.PublishNote);	
			}
            
            _repository.Save(entity);
        }
    }
}