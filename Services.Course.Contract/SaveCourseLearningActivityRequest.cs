using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BpeProducts.Services.Course.Contract
{
    [DataContract]
    public class SaveCourseLearningActivityRequest
    {
        [Required]
        [DataMember(IsRequired = true)]
        public int TenantId { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Type { get; set; }

        public Boolean? IsGradeable { get; set; }

        public Boolean? IsExtraCredit { get; set; }

        public int? MaxPoint { get; set; }

        public int? Weight { get; set; }

        public Guid? ObjectId { get; set; }

    }
}
