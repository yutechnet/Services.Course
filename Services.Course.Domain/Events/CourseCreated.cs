using System;
using BpeProducts.Services.Course.Contract;

namespace BpeProducts.Services.Course.Domain.Events
{
	public class CourseCreated : IDomainEvent
	{
		public Guid AggregateId { get; set; }
        public Entities.Course Course { get; set; }
	}
}
