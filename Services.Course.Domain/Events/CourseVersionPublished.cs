using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpeProducts.Services.Course.Domain.Events
{
    public class CourseVersionPublished : IDomainEvent
    {
        public Guid AggregateId { get; set; }
        public string PublishNote { get; set; }
    }
}
