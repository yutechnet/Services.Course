using System;

namespace BpeProducts.Services.Course.Contract
{
	public class ProgramResponse
	{
        public Guid OrganizationId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public Guid Id { get; set; }
        public string ProgramType { get; set; }
	}
}