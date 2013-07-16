using System;
using BpeProducts.Common.NHibernate.Version;

namespace BpeProducts.Services.Course.Domain.Events
{
    public class VersionCreated : IDomainEvent
    {
        public Guid AggregateId { get; set; }
        public VersionableEntity NewVersion { get; set; }
    }
}