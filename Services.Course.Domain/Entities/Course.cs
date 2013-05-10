using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.NHibernate;

namespace BpeProducts.Services.Course.Domain.Entities
{
    public class Course : TenantEntity
    {
        [Required]
        public virtual string Name { get; set; }
        [Required]
        public virtual string Code { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<Program> Programs { get; set; }
		public virtual IList<CourseSegment> Segments { get; set; }

        public Course()
        {
            Programs = new List<Program>();
			Segments = new List<CourseSegment>();
        }
       
    }

	public class CourseSegment : TenantEntity
	{
		public CourseSegment()
		{
			Segments = new List<CourseSegment>();
		}

		[Required]
		public virtual string Name { get; set; }
		public virtual string Description { get; set; }
		public virtual IList<CourseSegment> Segments { get; set; }
		public virtual CourseSegment ParentSegment { get; set; } 
	}
}
