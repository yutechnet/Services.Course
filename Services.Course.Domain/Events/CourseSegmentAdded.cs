using System;
using System.Collections.Generic;
using BpeProducts.Services.Course.Contract;

namespace BpeProducts.Services.Course.Domain.Events
{
    public class CourseSegmentAdded : IDomainEvent
    {
        public Guid AggregateId { get; set; }
        public Guid SegmentId { get; set; }
        public Guid ParentSegmentId { get; set; }
        public SaveCourseSegmentRequest Request { get; set; }
    }
}
