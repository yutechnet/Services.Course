using System.ComponentModel.DataAnnotations;

namespace BpeProducts.Services.Course.Host.Controllers
{
	public class CreateProgramRequest
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public string Description { get; set; }
		[Required]
		public string TenantId { get; set; }
	}
}