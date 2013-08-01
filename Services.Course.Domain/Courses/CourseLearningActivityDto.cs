using System;

namespace BpeProducts.Services.Course.Domain.Courses
{
    public class CourseLearningActivityDto
    {
        public int TenantId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Boolean IsGradeable { get; set; }
        public Boolean IsExtraCredit { get; set; }
        public int MaxPoint { get; set; }
        public int Weight { get; set; }
        public Guid ObjectId { get; set; }
    }
}
