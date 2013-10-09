using System;
using System.Collections.Generic;
using BpeProducts.Services.Course.Host.TempSectionContracts;

namespace Services.Section.Contracts
{
    public class CourseInfoResponse
    {
        public Guid Id { get; set; }
        public Guid TemplateCourseId { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public List<CourseSegmentInfo> Segments { get; set; }

        public string VersionNumber { get; set; }
        public bool IsPublished { get; set; }
        public string PublishNote { get; set; }
        public Guid OrganizationId { get; set; }
    }
}