using System;
using BpeProducts.Common.NHibernate.Version;

namespace BpeProducts.Services.Course.Domain
{
    public interface IVersionHandler
    {
        VersionableEntity CreateVersion(string entityType, Guid parentEntityId, string versionNumber);
        void PublishVersion(string entityType, Guid entityId, string publishNote);
    }
}