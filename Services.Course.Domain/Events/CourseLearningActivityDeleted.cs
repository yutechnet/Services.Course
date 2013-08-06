using System;
using System.Collections.Generic;
using BpeProducts.Services.Course.Contract;

namespace BpeProducts.Services.Course.Domain.Events
{
    public class CourseLearningActivityDeleted : IDomainEvent
    {
        public Guid LearningActivityId { get; set; }
        public Guid SegmentId { get; set; }
        public Guid AggregateId { get; set; }
    }
}