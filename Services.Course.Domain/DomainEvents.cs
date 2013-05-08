using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;

namespace BpeProducts.Services.Course.Domain
{
    public interface IDomainEvents
    {
        void Raise<T>(IDomainEvent domainEvent) where T : IDomainEvent;
    }

    public class DomainEvents : IDomainEvents
    {
        private readonly ILifetimeScope _scope;

        public DomainEvents(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public void Raise<T>(IDomainEvent domainEvent) where T:IDomainEvent
        {
            var handlers = _scope.Resolve<IEnumerable<IHandle<T>>>();
            foreach (var handler in handlers)
            {
                handler.Handle(domainEvent);
            }

        }
    }
}
