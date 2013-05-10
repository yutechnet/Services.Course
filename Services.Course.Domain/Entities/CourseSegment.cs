using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BpeProducts.Common.NHibernate;

namespace BpeProducts.Services.Course.Domain.Entities
{
    public class CourseSegment : TenantEntity
    {
        public CourseSegment()
        {
            ChildrenSegments = new List<CourseSegment>();
        }

        [Required]
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string Type { get; set; }
        public virtual int DisplayOrder { get; set; }
        public virtual IList<CourseSegment> ChildrenSegments { get; set; }
        public virtual CourseSegment ParentSegment { get; set; } 
    }
}