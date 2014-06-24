using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using BpeProducts.Common.Contract.Validation;

namespace BpeProducts.Services.Course.Contract
{
    [DataContract]
    public class VersionRequest
    {
        [Required, DataMember(IsRequired = true)]
        [DisallowEmptyGuid]
        public Guid ParentVersionId { get; set; }
        [Required, DataMember(IsRequired = true)]
        public string VersionNumber { get; set; }
    }
}
