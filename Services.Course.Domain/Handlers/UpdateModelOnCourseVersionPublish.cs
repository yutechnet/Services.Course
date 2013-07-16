using System;
using BpeProducts.Common.NHibernate.Version;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Handlers
{
    public class UpdateModelOnCourseVersionPublish : IHandle<VersionPublished>
    {
        private readonly IRepository _repository;

        public UpdateModelOnCourseVersionPublish(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(IDomainEvent domainEvent)
        {
            var e = domainEvent as VersionPublished;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }

            var entity = _repository.Get(e.EntityType, e.AggregateId) as VersionableEntity;
            if (entity != null)
            {
                entity.Publish(e.PublishNote);

                _repository.Save(entity);
            }
        }
    }
}