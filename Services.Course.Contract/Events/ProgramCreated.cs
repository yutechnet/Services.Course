using System;
using BpeProducts.Common.Contract.Events;

namespace BpeProducts.Services.Course.Contract.Events
{
	public class ProgramCreated:CreatedOrganizationalEntity
	{
		public Guid Id { get; set; }
		public Guid OrganizationId { get; set; }
		public string Type { get; set; }
	}

	public class ProgramUpdated : CreatedOrganizationalEntity
	{
		public Guid Id { get; set; }
		public Guid OrganizationId { get; set; }
		public string Type { get; set; }
	}
}