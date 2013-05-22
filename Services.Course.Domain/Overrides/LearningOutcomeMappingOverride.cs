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
			//mapping.HasMany<IHaveOutcomes>(x => x.AssociatedEntities)
			//	.KeyColumn("EntityId")
			//	.ForeignKeyConstraintName("none");

			//mapping.ReferencesAny(x => x.AssociatedEntities)
			//	.EntityIdentifierColumn("EntityId")
			//	.EntityTypeColumn("EntityType")
			//	.AddMetaValue<Program>("program")
			//	.AddMetaValue<Domain.Entities.Course>("course")
			//	.IdentityType<Guid>();

			//mapping
			//	.HasManyToMany<Ih>(x => x.AssociatedEntities).
			//	.Table("EntityOutcome");
        }
    }
}
