using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace BpeProducts.Services.Course.Domain.Overrides
{
    public class CourseMappingOverride : IAutoMappingOverride<Entities.Course>
    {
        public void Override(AutoMapping<Entities.Course> mapping)
        {
            mapping.IgnoreProperty(course => course.SegmentIndex);
        }
    }
}
