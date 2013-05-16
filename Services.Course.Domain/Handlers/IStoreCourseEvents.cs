using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain.Events;
using EventStore;

namespace BpeProducts.Services.Course.Domain.Handlers
{
    public interface IStoreCourseEvents : IStoreEvents
    {
        void Store<T>(T domainEvent) where T : IDomainEvent;
        void Store<T>(Guid aggregateId, List<T> domainEvents) where T : IDomainEvent;
    }
}
