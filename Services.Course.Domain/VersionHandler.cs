using System;
using System.Linq;
using System.Reflection;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate.Version;
using BpeProducts.Services.Asset.Contracts;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Courses;
using NHibernate.Type;
using ServiceStack.Common.Extensions;

namespace BpeProducts.Services.Course.Domain
{
    public class VersionHandler : IVersionHandler
    {
        private readonly IVersionableEntityFactory _versionableEntityFactory;
        private readonly IDomainEvents _domainEvents;
        private readonly IAssetServiceClient _assetService;
        public VersionHandler(IVersionableEntityFactory versionableEntityFactory, IDomainEvents domainEvents, IAssetServiceClient assetService)
        {
            _versionableEntityFactory = versionableEntityFactory;
            _domainEvents = domainEvents;
            _assetService = assetService;
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
                throw new BadRequestException(string.Format("{0} {1} is already published and cannot be published again.", entityType, entityId));
            }

            if (type == typeof(Domain.Courses.Course))
            {
                PublishCourseAssets(entity);
            }

            _domainEvents.Raise<VersionPublished>(new VersionPublished
            {
                AggregateId = entity.Id,
                PublishNote = publishNote,
                EntityType = type
            });

        }

        private void PublishCourseAssets(VersionableEntity entity)
        {
            var course = (Domain.Courses.Course)entity;
            course.Segments.ForEach(cs => cs.LearningMaterials.ForEach(l => PublishLearningMaterialAsset(l.AssetId)));
        }

        private void PublishLearningMaterialAsset(Guid assetId)
        {
            if (!CheckAssetIsPublished(assetId))
                _assetService.PublishAsset(assetId, string.Empty);
        }
        private bool CheckAssetIsPublished(Guid assetId)
        {
            var asset = _assetService.GetAsset(assetId);
            return asset.IsPublished;
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