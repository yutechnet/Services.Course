using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace BpeProducts.Services.Course.Domain.Overrides
{
    public class CourseMappingOverride : IAutoMappingOverride<Entities.Course>
    {
        public void Override(AutoMapping<Entities.Course> mapping)
        {
            mapping.IgnoreProperty(course => course.SegmentIndex);
            mapping.IgnoreProperty(course => course.Segments);
            mapping.Map(course => course.CourseSegmentJson).CustomSqlType("nvarchar(max)");
            //mapping.Map(course => course.Id).Generated.Never();
            mapping.Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
