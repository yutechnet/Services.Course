using System;
using System.Collections.Generic;

namespace BpeProducts.Services.Course.Host.TempSectionContracts
{
    public class SectionSegmentRequest
    {
        public SectionSegmentRequest()
        {
            ChildSegments = new List<SectionSegmentRequest>();
            LearningActivities = new List<SectionLearningActivityRequest>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int DisplayOrder { get; set; }
        public Guid? CourseSegmentId { get; set; }

        public IList<SectionSegmentRequest> ChildSegments { get; set; }
        public IList<SectionLearningActivityRequest> LearningActivities { get; set; }
    }
}