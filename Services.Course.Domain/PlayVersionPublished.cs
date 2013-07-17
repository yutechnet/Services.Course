using BpeProducts.Common.NHibernate.Version;
using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
    public class PlayVersionPublished : IPlayEvent<VersionPublished, VersionableEntity>
    {
        public VersionableEntity Apply(VersionPublished msg, VersionableEntity entity)
        {
            entity.Publish(msg.PublishNote);
            return entity;
        }

        public TE Apply<T, TE>(T msg, TE entity)
            where T : IDomainEvent
            where TE : class
        {
            return Apply(msg as VersionPublished, entity as VersionableEntity) as TE;
        }
    }
}