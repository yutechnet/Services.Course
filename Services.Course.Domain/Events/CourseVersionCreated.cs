using System;

namespace BpeProducts.Services.Course.Domain.Events
{
    public class CourseVersionCreated : IDomainEvent
    {
        public Guid AggregateId { get; set; }
        public Entities.Course NewVersion { get; set; }
    }
}