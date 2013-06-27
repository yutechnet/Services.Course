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
            const string uniqueVersion = "UniqueVersion";

            mapping.IgnoreProperty(course => course.SegmentIndex);
            mapping.IgnoreProperty(course => course.Segments);
            
            mapping.Map(c => c.OriginalEntityId).UniqueKey(uniqueVersion);
            mapping.Map(c => c.VersionNumber).UniqueKey(uniqueVersion);

            mapping.Map(course => course.CourseSegmentJson).CustomSqlType("nvarchar(max)");
            mapping.Id(x => x.Id).GeneratedBy.Assigned();
            mapping
                .HasManyToMany<Program>(x => x.Programs)
                .ParentKeyColumn("CourseId")
                .ChildKeyColumn("ProgramId")
                .ForeignKeyConstraintNames("FK_Course", "FK_Program")
                .Table("CourseProgram");
            mapping
				.HasManyToMany<LearningOutcome>(x => x.Outcomes)
				.ParentKeyColumn("EntityId")
				.ChildKeyColumn("LearningOutcomeId")
				.ForeignKeyConstraintNames("none","none")
				.Table("EntityLearningOutcome");

            mapping
                .References(c => c.Template)
                .Column("TemplateCourseId")
                .ForeignKey("FK_Course_Course3");
        }
    }
}
