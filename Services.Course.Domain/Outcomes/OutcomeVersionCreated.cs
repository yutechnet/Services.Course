using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain.Outcomes
{
    public class OutcomeVersionCreated : IDomainEvent
    {
        public Guid AggregateId { get; set; }
        public LearningOutcome NewVersion { get; set; }
    }
}
