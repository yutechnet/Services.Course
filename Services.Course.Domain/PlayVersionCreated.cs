using BpeProducts.Common.NHibernate.Version;
using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
    public class PlayVersionCreated : IPlayEvent<VersionCreated, VersionableEntity>
    {
        public VersionableEntity Apply(VersionCreated msg, VersionableEntity entity)
        {
            return msg.NewVersion;
        }

        public TE Apply<T, TE>(T msg, TE entity)
            where T : IDomainEvent
            where TE : class
        {
            return Apply(msg as VersionCreated, entity as VersionableEntity) as TE;
        }
    }
}