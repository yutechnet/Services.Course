using System;
using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain.Outcomes
{
    public class OutcomeCreated : IDomainEvent
    {
        public Guid AggregateId { get; set; }
        public string Description { get; set; }
        public Entities.LearningOutcome Outcome { get; set; }
        public bool ActiveFlag { get; set; }
    }
}
