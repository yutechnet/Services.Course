using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpeProducts.Services.Course.Contract
{
    public class CourseInfoResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public List<Guid> ProgramIds { get; set; } 
        public DateTime DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        public Guid AddedBy { get; set; }
        public Guid UpdatedBy { get; set; }
		public List<CourseSegment> Segments { get; set; }
    }
}
