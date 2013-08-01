using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpeProducts.Services.Course.Contract
{
    public class CourseLearningActivityResponse
    {

        public Guid Id { get; set; }
        public int TenantId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Boolean? IsGradeable { get; set; }
        public Boolean? IsExtraCredit { get; set; }
        public int? MaxPoint { get; set; }
        public int? Weight { get; set; }
        public Guid? ObjectId { get; set; }
        public Guid CourseSegmentId { get; set; }
    }
}
