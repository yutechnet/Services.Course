using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BpeProducts.Services.Course.Contract
{
    public class CourseSegmentInfo 
    {
        public CourseSegmentInfo()
        {
            ChildSegments = new List<CourseSegmentInfo>();
        }
        public List<LearningMaterialInfo> LearningMaterials { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int DisplayOrder { get; set; }
        public Guid ParentSegmentId { get; set; }

        public IList<CourseSegmentInfo> ChildSegments { get; set; }
        public IList<CourseLearningActivityResponse> CourseLearningActivities { get; set; }
        public int? ActiveDate { get; set; }
        public int? InactiveDate { get; set; }
    }
}