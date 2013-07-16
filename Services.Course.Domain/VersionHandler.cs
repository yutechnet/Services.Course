using System;
using System.Linq;
using System.Reflection;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate.Version;
using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
    public class VersionHandler : IVersionHandler
    {
        private readonly IVersionableEntityFactory _versionableEntityFactory;
        private readonly IDomainEvents _domainEvents;

        public VersionHandler(IVersionableEntityFactory versionableEntityFactory, IDomainEvents domainEvents)
        {
            _versionableEntityFactory = versionableEntityFactory;
            _domainEvents = domainEvents;
        }

        public VersionableEntity CreateVersion(string entityType, Guid parentEntityId, string versionNumber)
        {
            var type = GetEntityType(entityType);
            if (type == null)
            {
                throw new BadRequestException(string.Format("Type {0} is not found.", entityType));
            }

            VersionableEntity parentEntity = _versionableEntityFactory.Get(type, parentEntityId);
            if (parentEntity == null)
            {
                throw new NotFoundException(string.Format("{0} {1} not found.", entityType, parentEntityId));
            }

            var existingVersion = _versionableEntityFactory.Get(type, parentEntity.OriginalEntity.Id, versionNumber);
            if (existingVersion != null)
            {
                throw new BadRequestException(string.Format("Version {0} for {1} {2} already exists.", versionNumber, entityType, parentEntity.OriginalEntity.Id));
            }

            var newVersion = parentEntity.CreateVersion(versionNumber);

            _domainEvents.Raise<VersionCreated>(new VersionCreated
            {
                AggregateId = newVersion.Id,
                NewVersion = newVersion
            });

            return newVersion;

        }

        public void PublishVersion(string entityType, Guid entityId, string publishNote)
        {
            var type = GetEntityType(entityType);
            if (type == null)
            {
                throw new BadRequestException(string.Format("Type {0} is not found.", entityType));
            }

            VersionableEntity entity = _versionableEntityFactory.Get(type, entityId);
            if (entity == null)
            {
                throw new NotFoundException(string.Format("{0} {1} is not found.", entityType, entityId));
            }

            if (entity.IsPublished)
            {
                throw new BadRequestException(string.Format("{0} {1} is already published and cannot be published again."));
            }

            _domainEvents.Raise<VersionPublished>(new VersionPublished
            {
                AggregateId = entity.Id,
                PublishNote = publishNote,
                EntityType = type
            });

        }

        private static Type GetEntityType(string entityTypeName)
        {
            var entityAssembly = Assembly.Load("BpeProducts.Services.Course.Domain");

            var type = (from t in entityAssembly.GetTypes()
                        where t.Name.ToLower() == entityTypeName
                        select t).FirstOrDefault();
            return type;
        }
    }
}