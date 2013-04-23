using System;
using BpeProducts.Common.NHibernate;

namespace BpeProducts.Services.Course.Domain.Entities
{
	public class Program:TenantEntity
	{
		public virtual String Name
		{
			get ; 
			set ; 
		}

		public virtual string Description
		{
			get;
			set; 
		}
	}
}