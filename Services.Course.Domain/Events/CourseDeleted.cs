using System;

namespace BpeProducts.Services.Course.Domain.Events
{
    public class CourseDeleted:IDomainEvent
    {
        public Guid AggregateId { get; set; }
	}
}
