using System;
using System.Collections.Generic;

namespace BpeProducts.Services.Course.Host.TempSectionContracts
{
    public class SectionSegmentInfo
    {
        public SectionSegmentInfo()
        {
            ChildSegments = new List<SectionSegmentInfo>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int DisplayOrder { get; set; }
        public Guid? CourseSegmentId { get; set; }
        public Guid ParentSegmentId { get; set; }

        public IList<SectionSegmentInfo> ChildSegments { get; set; }
        public IList<SectionLearningActivityInfo> LearningActivities { get; set; }    
    }
}
