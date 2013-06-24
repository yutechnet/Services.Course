using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BpeProducts.Services.Course.Contract
{
    public class CourseSegment
    {
        public CourseSegment()
        {
            ChildrenSegments = new List<CourseSegment>();
        }

        public virtual Guid Id { get; set; }
        [Required]
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        [Required]
        public virtual string Type { get; set; }
        public virtual int DisplayOrder { get; set; }

        public virtual long DiscussionId { get; set; }

        public virtual List<CourseSegment> ChildrenSegments { get; set; }
        public virtual Guid ParentSegmentId { get; set; } 

        public virtual void AddSubSegment(CourseSegment segment)
        {
            segment.ParentSegmentId = Id;
            ChildrenSegments.Add(segment);
        }
    }
}