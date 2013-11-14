using System;
using System.Collections.Generic;
using BpeProducts.Services.Course.Contract;

namespace BpeProducts.Services.Course.Domain.Events
{
    public class CourseInfoUpdated:IDomainEvent
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public ECourseType CourseType { get; set; }
        public bool IsTemplate { get; set; }
        public Guid AggregateId { get; set; }
		public List<Guid> PrerequisiteCourseIds { get; set; }
        public decimal Credit { get; set; }
    }
}
