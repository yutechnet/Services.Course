using System.ComponentModel.DataAnnotations;

namespace BpeProducts.Services.Course.Contract
{
	public class SaveProgramRequest
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public string Description { get; set; }
		[Required]
		public string TenantId { get; set; }
	}
}