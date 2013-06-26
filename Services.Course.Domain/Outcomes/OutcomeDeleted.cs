using System;
using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain.Outcomes
{
    public class OutcomeDeleted:IDomainEvent
    {
        public Guid AggregateId { get; set; }
	}
}
