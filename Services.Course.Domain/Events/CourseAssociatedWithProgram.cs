using System;

namespace BpeProducts.Services.Course.Domain.Events
{
    public class CourseAssociatedWithProgram:IDomainEvent
    {
        public Guid ProgramId { get; set; }
        public Guid AggregateId { get; set; }
    }
}
