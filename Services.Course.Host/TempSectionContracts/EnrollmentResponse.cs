using System;

namespace BpeProducts.Services.Course.Host.TempSectionContracts
{
    public class EnrollmentResponse : TenantResponseBase
    {
        public Guid UserId { get; set; }
        public SectionResponse Section { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}