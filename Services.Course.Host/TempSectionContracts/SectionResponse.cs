using System;
using System.Collections.Generic;

namespace BpeProducts.Services.Course.Host.TempSectionContracts
{
    public class SectionResponse : TenantResponseBase
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Guid CourseId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public IList<SectionSegmentInfo> Segments { get; set; }
    }
}
