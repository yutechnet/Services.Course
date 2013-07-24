using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain.Courses;
using BpeProducts.Services.Course.Domain.Entities;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Mapping;

namespace BpeProducts.Services.Course.Domain.Overrides
{
    public class CourseMappingOverride : IAutoMappingOverride<Courses.Course>
    {
        public void Override(AutoMapping<Courses.Course> mapping)
        {
            mapping.Map(x => x.Name).Access.CamelCaseField(Prefix.Underscore);
            mapping.Map(x => x.Code).Access.CamelCaseField(Prefix.Underscore);
            mapping.Map(x => x.Description).Access.CamelCaseField(Prefix.Underscore);
            mapping.Map(x => x.CourseType).Access.CamelCaseField(Prefix.Underscore);
            
            mapping.References<Courses.Course>(c => c.OriginalEntity);
            mapping.References<Courses.Course>(c => c.ParentEntity);

            mapping.Id(x => x.Id).GeneratedBy.Assigned();

            mapping
                .HasMany(x => x.Segments)
                .Access.CamelCaseField(Prefix.Underscore)
                .ForeignKeyConstraintName("FK_Course_CourseSegment");

            mapping.IgnoreProperty(x => x.Prerequisites);
                //.HasMany(x => x.Prerequisites)
                //.Access.CamelCaseField(Prefix.Underscore)
                //.ForeignKeyConstraintName("FK_Course_Prerequisite");

            mapping
                .HasManyToMany(x => x.Programs)
                .Access.CamelCaseField(Prefix.Underscore)
                .ParentKeyColumn("CourseId")
                .ChildKeyColumn("ProgramId")
                .ForeignKeyConstraintNames("FK_Course", "FK_Program")
                .Table("CourseProgram");

            mapping
                .HasManyToMany(x => x.Outcomes)
                .Access.CamelCaseField(Prefix.Underscore)
                .ParentKeyColumn("EntityId")
                .ChildKeyColumn("LearningOutcomeId")
                .ForeignKeyConstraintNames("none", "none")
                .Table("EntityLearningOutcome");

            mapping
                .References(c => c.Template)
                .Column("TemplateCourseId")
                .ForeignKey("FK_Course_Course3");
        }
    }
}
