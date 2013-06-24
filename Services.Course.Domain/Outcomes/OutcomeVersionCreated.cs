using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain.Outcomes
{
    public class OutcomeVersionCreated : IDomainEvent
    {
        public Guid AggregateId { get; set; }
        public Guid ParentEntityId { get; set; }
        public Guid OriginalEntityId { get; set; }
        public string VersionNumber { get; set; }
        public bool IsPublished { get; set; }
    }
}
