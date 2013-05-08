using System;

namespace BpeProducts.Services.Course.Domain.Events
{
    public interface IDomainEvent
    {
        Guid AggregateId { get; set; }
    }
}