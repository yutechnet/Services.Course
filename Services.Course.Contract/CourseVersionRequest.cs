using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BpeProducts.Services.Course.Contract
{
    [DataContract]
    public class CourseVersionRequest
    {
        [Required, DataMember(IsRequired = true)]
        public Guid ParentVersionId { get; set; }
        [Required, DataMember(IsRequired = true)]
        public string VersionNumber { get; set; }
    }
}
