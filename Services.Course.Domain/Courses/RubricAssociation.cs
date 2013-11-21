using System;
using BpeProducts.Common.NHibernate;

namespace BpeProducts.Services.Course.Domain.Courses
{
	public class RubricAssociation : TenantEntity
	{
		public virtual Guid RubricId { get; set; }
	}
}
