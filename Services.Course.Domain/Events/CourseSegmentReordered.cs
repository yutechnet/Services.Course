using System;
using BpeProducts.Services.Course.Contract;

namespace BpeProducts.Services.Course.Domain.Events
{
    public class CourseSegmentReordered : IDomainEvent
    {
        public Guid SegmentId { get; set; }
        public Guid AggregateId { get; set; }
        public int DisplayOrder { get; set; }
        public UpdateCourseSegmentRequest Request { get; set; }
    }
}