﻿using System;
using System.Collections.Generic;

namespace BpeProducts.Services.Course.Host.TempSectionContracts
{
    public class CourseSegmentInfo
    {
        public CourseSegmentInfo()
        {
            ChildSegments = new List<CourseSegmentInfo>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int DisplayOrder { get; set; }
        public Guid ParentSegmentId { get; set; }

        public int TenantId { get; set; }

        public IList<CourseSegmentInfo> ChildSegments { get; set; }
        public IList<CourseLearningActivityResponse> CourseLearningActivities { get; set; }
    }
}