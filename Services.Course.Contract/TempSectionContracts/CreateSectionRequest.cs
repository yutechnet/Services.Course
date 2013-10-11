using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BpeProducts.Services.Course.Host.TempSectionContracts
{
    [DataContract]
    public class CreateSectionRequest
    {
        public CreateSectionRequest()
        {
            Segments = new List<SectionSegmentRequest>();            
        }

        [Required]
        [DataMember(IsRequired = true)]
        public int TenantId { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Code { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public DateTime StartDate { get; set; }

        [DataMember]
        public DateTime? EndDate { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public Guid CourseId { get; set; }

        [DataMember]
        public List<SectionSegmentRequest> Segments { get; set; }
    }
}
