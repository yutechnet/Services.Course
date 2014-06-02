using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.Contract.Events;

namespace BpeProducts.Services.Course.Contract.Events
{
	public class CourseCreated:ICreatedOrganizationalEntity
	{
		public Guid Id { get; set; }
		public Guid OrganizationId { get; set; }
		public string Type { get; set; }
	}
}
