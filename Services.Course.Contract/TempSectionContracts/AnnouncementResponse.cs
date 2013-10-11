using System;

namespace BpeProducts.Services.Course.Host.TempSectionContracts
{
    public class AnnouncementResponse:TenantResponseBase
    {
        public Guid TemplateId { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public DateTime PostDateTime { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SectionResponse Section { get; set; }
        public bool IsTemplate { get; set; }
    }
}