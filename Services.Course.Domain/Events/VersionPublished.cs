using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.NHibernate.Version;

namespace BpeProducts.Services.Course.Domain.Events
{
    public class VersionPublished : IDomainEvent
    {
        public Guid AggregateId { get; set; }
        public string PublishNote { get; set; }
        public Type EntityType { get; set; }
    }
}
