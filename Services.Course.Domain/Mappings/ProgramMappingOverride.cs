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
    public class ProgramMappingOverride : IAutoMappingOverride<Entities.Program>
    {
        public void Override(AutoMapping<Entities.Program> mapping)
        {
            mapping
                .HasManyToMany(x => x.Courses)
                .ParentKeyColumn("ProgramId")
                .ChildKeyColumn("CourseId")
                .Table("CourseProgram")
                .Inverse();
			
			mapping
				.HasManyToMany(x => x.SupportedOutcomes)
				.ParentKeyColumn("EntityId")
				.ChildKeyColumn("LearningOutcomeId")
				.ForeignKeyConstraintNames("none", "none")
                .Table("EntityLearningOutcome")
                .Inverse();
        }
    }
}

