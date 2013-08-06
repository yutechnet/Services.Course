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

        [DataMember]   
        public Boolean IsGradeable { get; set; }
        [DataMember]   
        public Boolean IsExtraCredit { get; set; }
        [DataMember]   
        public int MaxPoint { get; set; }
        [DataMember]   
        public int Weight { get; set; }
        [DataMember]   
        public Guid ObjectId { get; set; }

    }
}
