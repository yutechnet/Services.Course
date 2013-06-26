using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain.Outcomes
{
    public class OutcomeVersionPublished : IDomainEvent
    {
        public Guid AggregateId { get; set; }
        public string PublishNote { get; set; }
    }
}
