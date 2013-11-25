using BpeProducts.Services.Course.Domain.Courses;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace BpeProducts.Services.Course.Domain.Mappings
{
	public class RubricAssociationMappingOverride : IAutoMappingOverride<RubricAssociation>
	{
		public void Override(AutoMapping<RubricAssociation> mapping)
		{
			mapping.Id(x => x.Id).GeneratedBy.Assigned();
		}
	}
}