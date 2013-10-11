using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BpeProducts.Services.Course.Host.TempSectionContracts
{
    [DataContract]
    public class UpdateEnrollmentRequest
    {
        [Required]
        [DataMember(IsRequired = true)]
        public DateTime StartDate { get; set; }
        [DataMember]
        public DateTime? EndDate { get; set; }

        // enrollment status
        [Required]
        [DataMember(IsRequired = true)]
        public string Status { get; set; }
    }
}