﻿using BpeProducts.Services.Course.Domain.CourseAggregates;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace BpeProducts.Services.Course.Domain.Mappings
{
    public class LearningMaterialMappingOverride : IAutoMappingOverride<LearningMaterial>
    {
        public void Override(AutoMapping<LearningMaterial> mapping)
        {
            mapping.Id(x => x.Id).GeneratedBy.Assigned();
            mapping.IgnoreProperty(x => x.SourceLearningMaterialId);
        }
    }
}