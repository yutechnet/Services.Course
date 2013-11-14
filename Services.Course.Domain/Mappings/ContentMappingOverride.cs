using BpeProducts.Services.Course.Domain.Courses;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace BpeProducts.Services.Course.Domain.Mappings
{
    public class ContentMappingOverride : IAutoMappingOverride<LearningMaterial>
    {
        public void Override(AutoMapping<LearningMaterial> mapping)
        {
            mapping.Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}