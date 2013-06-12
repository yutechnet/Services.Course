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
    public class LearningOutcomeMappingOverride : IAutoMappingOverride<Entities.LearningOutcome>
    {
        public void Override(AutoMapping<LearningOutcome> mapping)
        {
			mapping.Map(outcome => outcome.Description).CustomSqlType("nvarchar(max)");
            mapping
                .HasManyToMany<LearningOutcome>(x => x.Outcomes)
                .ParentKeyColumn("EntityId")
                .ChildKeyColumn("LearningOutcomeId")
                .ForeignKeyConstraintNames("none", "none")
                .Table("EntityLearningOutcome");
        }
    }
}
