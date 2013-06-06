using System;

namespace BpeProducts.Services.Course.Domain.Events
{
    public class CourseVersionCreated : IDomainEvent
    {
        public Guid AggregateId { get; set; }

        public Guid OriginalCourseId { get; set; }
        public Guid ParentCourseId { get; set; }
        public string VersionNumber { get; set; }
        public bool IsPublished { get; set; }
    }
}