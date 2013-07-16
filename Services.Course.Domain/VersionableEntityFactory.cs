using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate.Version;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain
{
    public class VersionableEntityFactory : IVersionableEntityFactory
    {
        private readonly IRepository _repository;

        public VersionableEntityFactory(IRepository repository)
        {
            _repository = repository;
        }

        public VersionableEntity Get(string entityType, Guid id)
        {
            var entityAssembly = Assembly.Load("BpeProducts.Services.Course.Domain");

            var type = (from t in entityAssembly.GetTypes()
                        where t.Name.ToLower() == entityType
                        select t).FirstOrDefault();

            if (type == null)
            {
                throw new BadRequestException(string.Format("Type {0} is not found.", entityType));
            }

            return Get(type, id);
        }

        public VersionableEntity Get(Type type, Guid id)
        {
            // var type = Type.GetType(entityNamespace + entityType);
            var versionable = _repository.Get(type, id) as VersionableEntity;

            if (versionable == null)
                throw new NotFoundException(string.Format("{0} {1} is not found.", type.Name, id));

            return versionable;
        }

        public VersionableEntity Get(Type type, Guid originalEntityId, string versionNumber)
        {
            return _repository.Query<VersionableEntity>()
                           .FirstOrDefault(r => r.OriginalEntity.Id == originalEntityId && r.VersionNumber == versionNumber);
        }
    }
}