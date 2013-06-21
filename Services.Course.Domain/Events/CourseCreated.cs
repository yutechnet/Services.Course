using System;

namespace BpeProducts.Services.Course.Domain.Events
{
	public class CourseCreated : IDomainEvent
	{
        public Guid OrganizationId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Code { get; set; }
	    public Entities.Course Course { get; set; }
		public bool ActiveFlag { get; set; }
		public Guid AggregateId { get; set; }
        public Guid TemplateCourseId { get; set; }
	}
}