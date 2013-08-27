using System;

namespace BpeProducts.Services.Course.Domain.Events
{
	public class CoursePrerequisiteRemoved : IDomainEvent
	{
		public Guid AggregateId { get; set; }
		public Guid PrerequisiteCourseId { get; set; }
        //public Courses.Course PrerequisiteCourse { get; set; }
	}
}