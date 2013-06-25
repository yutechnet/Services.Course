using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain.Handlers
{
    public class OutcomeCreated : IDomainEvent
    {
        public Guid AggregateId { get; set; }
        public string Description { get; set; }
        public Entities.LearningOutcome Outcome { get; set; }
        public bool ActiveFlag { get; set; }
    }
}
