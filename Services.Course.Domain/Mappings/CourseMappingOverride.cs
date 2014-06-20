using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Mapping;

namespace BpeProducts.Services.Course.Domain.Mappings
{
    public class CourseMappingOverride : IAutoMappingOverride<CourseAggregates.Course>
    {
        public void Override(AutoMapping<CourseAggregates.Course> mapping)
        {
            mapping.Map(x => x.Name).Access.CamelCaseField(Prefix.Underscore);
            mapping.Map(x => x.Code).Access.CamelCaseField(Prefix.Underscore);
            mapping.Map(x => x.Description).Access.CamelCaseField(Prefix.Underscore);
            mapping.Map(x => x.CourseType).Access.CamelCaseField(Prefix.Underscore);
            mapping.Map(x => x.Credit).Access.CamelCaseField(Prefix.Underscore);
            mapping.Map(x => x.MetaData).Access.CamelCaseField(Prefix.Underscore);
            
            mapping.References<CourseAggregates.Course>(c => c.OriginalEntity);
            mapping.References<CourseAggregates.Course>(c => c.ParentEntity);

            mapping.Id(x => x.Id).GeneratedBy.Assigned();

            mapping
                .HasMany(x => x.Segments)
                .Access.CamelCaseField(Prefix.Underscore)
                .OrderBy("DisplayOrder")
                .ForeignKeyConstraintName("FK_Course_CourseSegment");

			mapping
				.HasManyToMany(x => x.Prerequisites)
				.Access.CamelCaseField(Prefix.Underscore)
				.ParentKeyColumn("CourseId")
				.ChildKeyColumn("PrerequisiteCourseId")
				.ForeignKeyConstraintNames("FK_CoursePrerequisite_Course", "FK_CoursePrerequisite_Course2")
				.Table("CoursePrerequisite")
				.Cascade.None();

            mapping
                .HasManyToMany(x => x.Programs)
                .Access.CamelCaseField(Prefix.Underscore)
                .ParentKeyColumn("CourseId")
                .ChildKeyColumn("ProgramId")
                .ForeignKeyConstraintNames("FK_Course", "FK_Program")
                .Table("CourseProgram");

            mapping
                .References(c => c.Template)
                .Column("TemplateCourseId")
                .ForeignKey("FK_Course_Course3");
        }
    }
}
