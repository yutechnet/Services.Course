using System;

namespace BpeProducts.Services.Course.Domain.Events
{
    public class CourseSegmentDeleted : IDomainEvent
    {
        public Guid SegmentId { get; set; }
        public Guid AggregateId { get; set; }
    }
}