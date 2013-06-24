using System;

namespace BpeProducts.Services.Course.Domain.Events
{
    public class CourseSegmentAdded:IDomainEvent
    {
        public Guid ParentSegmentId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public Guid AggregateId { get; set; }
        public long DiscussionId { get; set; }
    }
}
