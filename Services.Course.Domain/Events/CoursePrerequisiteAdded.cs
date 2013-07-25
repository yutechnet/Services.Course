using System;

namespace BpeProducts.Services.Course.Domain.Events
{
	public class CoursePrerequisiteAdded : IDomainEvent
	{
		public Guid AggregateId { get; set; }
		public Guid PrerequisiteCourseId { get; set; }
	}
}