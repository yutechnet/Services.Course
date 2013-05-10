using System;

namespace BpeProducts.Services.Course.Domain.Events
{
    public class CourseDisassociatedWithProgram : IDomainEvent
    {
        public Guid ProgramId { get; set; }
        public Guid AggregateId { get; set; }
    }
}