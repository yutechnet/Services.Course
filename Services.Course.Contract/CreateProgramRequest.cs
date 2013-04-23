namespace BpeProducts.Services.Course.Host.Controllers
{
	public class CreateProgramRequest
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string TenantId { get; set; }
	}
}