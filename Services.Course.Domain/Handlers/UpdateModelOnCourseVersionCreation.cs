using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Handlers
{
    // TODO: Remove this and other IHandle<> classes after LearningOutcome management has been completely migrated over to Assessment Service
    public class UpdateModelOnCourseVersionCreation : IHandle<VersionCreated>
    {
        private readonly IRepository _repository;

        public UpdateModelOnCourseVersionCreation(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(IDomainEvent domainEvent)
        {
            var e = domainEvent as VersionCreated;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }

            // create a new version based on the parent version
            _repository.Save(e.NewVersion);
        }
    }
}
