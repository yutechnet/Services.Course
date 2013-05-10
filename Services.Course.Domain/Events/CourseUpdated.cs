using System;

namespace BpeProducts.Services.Course.Domain.Events
{
    public class CourseUpdated : IDomainEvent
    {
        public Guid AggregateId { get; set; }
        public Entities.Course New { get; set; }
        public Entities.Course Old { get; set; }
    }
}
