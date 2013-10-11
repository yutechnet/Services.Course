using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BpeProducts.Services.Course.Host.TempSectionContracts
{
    public class CreateAnnouncementRequest:TenantRequestBase
    {
        [DataMember]
        public Guid TemplateId { get; set; }
        [DataMember]
        public Guid SectionId { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        [StringLength(500)]
        public string Subject { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public DateTime PostDateTime { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public DateTime EndDate { get; set; }

        [DataMember]
        public bool IsTemplate { get; set; }
    }
}