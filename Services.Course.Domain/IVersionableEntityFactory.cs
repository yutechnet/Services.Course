using System;
using BpeProducts.Common.NHibernate.Version;

namespace BpeProducts.Services.Course.Domain
{
    public interface IVersionableEntityFactory
    {
        VersionableEntity Get(string entityType, Guid id);
        VersionableEntity Get(Type type, Guid id);
        VersionableEntity Get(Type type, Guid originalEntityId, string versionNumber);
    }
}