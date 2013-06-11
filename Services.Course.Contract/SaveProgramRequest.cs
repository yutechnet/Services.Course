using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BpeProducts.Services.Course.Contract
{
    [DataContract]
	public class SaveProgramRequest
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public string Description { get; set; }
        [Required, DataMember(IsRequired= true)]
        public Guid OrganizationId { get; set; }
		[Required]
		public string TenantId { get; set; }
	}
}