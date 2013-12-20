using System;
using System.Collections.Generic;
using BpeProducts.Common.NHibernate;
using BpeProducts.Common.NHibernate.Version;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using BpeProducts.Services.Course.Domain.Validation;

namespace BpeProducts.Services.Course.Domain.Handlers
{
    public class UpdateModelOnCourseVersionPublish : IHandle<VersionPublished>
    {
        private readonly IRepository _repository;
        private readonly IValidator<Courses.Course> _courseValidator;

        public UpdateModelOnCourseVersionPublish(IRepository repository, Validation.IValidator<Courses.Course> courseValidator)
        {
            _repository = repository;
            _courseValidator = courseValidator;
        }

        public void Handle(IDomainEvent domainEvent)
        {
            var e = domainEvent as VersionPublished;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }

            var entity = _repository.Get(e.EntityType, e.AggregateId) as Course.Domain.Courses.Course;
            
            if (entity == null) return;
            
            IEnumerable<string> brokenRules;

            if (!entity.Validate(_courseValidator, out brokenRules)) return;

            entity.Publish(e.PublishNote);
            _repository.Save(entity);
        }
    }
}