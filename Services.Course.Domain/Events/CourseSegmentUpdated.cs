using System;
using System.Collections.Generic;
using BpeProducts.Services.Course.Contract;

namespace BpeProducts.Services.Course.Domain.Events
{
    public class CourseSegmentUpdated : IDomainEvent
    {
        public Guid SegmentId { get; set; }
        public Guid AggregateId { get; set; }
        public SaveCourseSegmentRequest Request { get; set; }
    }
}