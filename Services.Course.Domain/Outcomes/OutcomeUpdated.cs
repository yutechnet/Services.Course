using System;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain.Outcomes
{
    public class OutcomeUpdated : IDomainEvent
    {
        public Guid AggregateId { get; set; }
        public string Description { get; set; }
    }
}
