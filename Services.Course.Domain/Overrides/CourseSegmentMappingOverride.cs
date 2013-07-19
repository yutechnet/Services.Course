using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using BpeProducts.Services.Course.Domain.Courses;

namespace BpeProducts.Services.Course.Domain.Overrides
{
    public class CourseSegmentMappingOverride : IAutoMappingOverride<CourseSegment>
    {
        public void Override(AutoMapping<CourseSegment> mapping)
        {
            mapping.Id(x => x.Id).GeneratedBy.Assigned();

            mapping.IgnoreProperty(segment => segment.Name);
            mapping.IgnoreProperty(segment => segment.Description);
            mapping.IgnoreProperty(segment => segment.Type);
            mapping.IgnoreProperty(segment => segment.DisplayOrder);
            mapping.IgnoreProperty(segment => segment.Content);

            mapping.Map(segment => segment.SerializedData).CustomSqlType("nvarchar(max)");
        }
    }
}
