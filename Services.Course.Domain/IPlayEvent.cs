using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
    public interface IPlayEvent<in T, TE> : IPlayEvent where T : IDomainEvent where TE : class
    {
        TE Apply(T msg, TE entity);
    }

    public interface IPlayEvent
    {
        TE Apply<T, TE>(T msg, TE entity)
            where T : IDomainEvent
            where TE : class;
    }
}