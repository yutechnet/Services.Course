using BpeProducts.Services.Course.Domain.Courses;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace BpeProducts.Services.Course.Domain.Mappings
{
	public class CourseRubricMappingOverride : IAutoMappingOverride<CourseRubric>
	{
		public void Override(AutoMapping<CourseRubric> mapping)
		{
			mapping.Id(x => x.Id).GeneratedBy.Assigned();
		}
	}
}