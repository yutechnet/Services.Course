using System;

namespace BpeProducts.Services.Course.Domain.Events
{
    public class CourseInfoUpdated:IDomainEvent
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public Guid AggregateId { get; set; }
    }
}
