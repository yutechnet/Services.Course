using System;

namespace BpeProducts.Services.Course.Domain.Events
{
    public class CourseSegmentUpdated : IDomainEvent
    {
        public Guid ParentSegmentId { get; set; }
        public Guid SegmentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public Guid AggregateId { get; set; }
    }
}