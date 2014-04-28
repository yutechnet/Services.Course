using BpeProducts.Services.Course.Domain.CourseAggregates;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Mapping;

namespace BpeProducts.Services.Course.Domain.Mappings
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
                   .HasMany(x => x.LearningMaterials)
                   .Access.CamelCaseField(Prefix.Underscore)
                   .ForeignKeyConstraintName("FK_CourseSegment_LearningMaterial");

            mapping.IgnoreProperty(x => x.SourceCourseSegmentId);
        }
    }
}
