using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using BpeProducts.Common.Contract.Validation;

namespace BpeProducts.Services.Course.Contract
{
    [DataContract]
    public class UpdateProgramRequest
    {
        [Required, DataMember]
        public string Name { get; set; }
        [Required, DataMember]
        public string Description { get; set; }
        [Required, DataMember(IsRequired = true)]
        [DisallowEmptyGuid]
        public Guid OrganizationId { get; set; }
        [Required, DataMember]
        public string ProgramType { get; set; }
        [DataMember]
        public string GraduationRequirements { get; set; }
    }
}