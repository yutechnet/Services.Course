using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain.Handlers
{
    public interface IHandle<T> where T:IDomainEvent
    {
        void Handle(IDomainEvent domainEvent);
    }
}