using System;

namespace BpeProducts.Services.Course.Domain.Events
{
    public class CourseAssociatedWithProgram:IDomainEvent
    {
        public Guid AggregateId { get; set; }
        public Guid ProgramId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProgramType { get; set; }
    }

	public class CoursePrerequisiteAdded : IDomainEvent
	{
		public Guid AggregateId { get; set; }
		public Guid PrerequisiteCourseId { get; set; }
	}

	public class CoursePrerequisiteRemoved : IDomainEvent
	{
		public Guid AggregateId { get; set; }
		public Guid PrerequisiteCourseId { get; set; }
	}
}
