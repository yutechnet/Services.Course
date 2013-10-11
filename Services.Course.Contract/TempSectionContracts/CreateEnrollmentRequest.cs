using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BpeProducts.Services.Course.Host.TempSectionContracts
{
    [DataContract]
    public class CreateEnrollmentRequest : TenantRequestBase
    {
        [Required]
        [DataMember(IsRequired = true)]
        public Guid UserId { get; set; }
        [Required]
        [DataMember(IsRequired = true)]
        public string Role { get; set; }
        [Required]
        [DataMember(IsRequired = true)]
        public DateTime StartDate { get; set; }
        [DataMember]
        public DateTime? EndDate { get; set; }
    }
}