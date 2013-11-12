using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain.Entities;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using BpeProducts.Services.Course.Domain.Courses;
using FluentNHibernate.Mapping;

namespace BpeProducts.Services.Course.Domain.Overrides
{
    public class CourseSegmentMappingOverride : IAutoMappingOverride<CourseSegment>
    {
        public void Override(AutoMapping<CourseSegment> mapping)
        {
            mapping.Id(x => x.Id).GeneratedBy.Assigned();

            mapping.References(x => x.ParentSegment)
                .Access.CamelCaseField(Prefix.Underscore)
                .Column("ParentSegmentId");

            mapping.HasMany(x => x.ChildSegments)
                .Access.CamelCaseField(Prefix.Underscore)
                .OrderBy("DisplayOrder")
                .KeyColumn("ParentSegmentId");
           
            mapping.Map(segment => segment.SerializedData).CustomSqlType("nvarchar(max)");

            mapping
                .HasManyToMany(x => x.SupportedOutcomes)
                .Access.CamelCaseField(Prefix.Underscore)
                .ParentKeyColumn("EntityId")
                .ChildKeyColumn("LearningOutcomeId")
                .ForeignKeyConstraintNames("none", "none")
                .Table("EntityLearningOutcome")
                .Inverse();
        }
    }
}
