using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain.CourseAggregates;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Mapping;

namespace BpeProducts.Services.Course.Domain.Mappings
{
    public class CourseLearningActivityMappingOverride: IAutoMappingOverride<CourseLearningActivity>
    {
        public void Override(AutoMapping<CourseLearningActivity> mapping)
        {
            mapping.Id(x => x.Id).GeneratedBy.Assigned();
            mapping.IgnoreProperty(x => x.SourceCourseLearningActivityId);
        }
    }
}