using System;
using BpeProducts.Common.Contract.Events;

namespace BpeProducts.Services.Course.Contract.Events
{
	public class CourseUpdated : ICreatedOrganizationalEntity
	{
		public Guid Id { get; set; }
		public Guid OrganizationId { get; set; }
		public string Type { get; set; }
	}
}