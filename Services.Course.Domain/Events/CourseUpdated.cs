using System;
using BpeProducts.Services.Course.Contract;

namespace BpeProducts.Services.Course.Domain.Events
{
    public class CourseUpdated : IDomainEvent
    {
        public Guid AggregateId { get; set; }
        public Courses.Course Old { get; set; }
		public SaveCourseRequest Request{ get; set; }
    }
}
