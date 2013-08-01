//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using BpeProducts.Services.Course.Domain.Entities;
//using FluentNHibernate.Automapping;
//using FluentNHibernate.Automapping.Alterations;

//namespace BpeProducts.Services.Course.Domain.Overrides
//{
//    public class LearningOutcomeMappingOverride : IAutoMappingOverride<LearningOutcome>
//    {
//        public void Override(AutoMapping<LearningOutcome> mapping)
//        {
//            const string uniqueVersion = "UniqueVersion";

//            mapping.Id(x => x.Id).GeneratedBy.Assigned();
//            mapping.Map(outcome => outcome.Description).CustomSqlType("nvarchar(max)");

//            mapping.References<LearningOutcome>(l => l.OriginalEntity);
//            mapping.References<LearningOutcome>(l => l.ParentEntity);

//            mapping
//                .HasManyToMany(x => x.SupportedOutcomes)
//                .ParentKeyColumn("EntityId")
//                .ChildKeyColumn("LearningOutcomeId")
//                .ForeignKeyConstraintNames("none", "none")
//                .Table("EntityLearningOutcome");

//            //mapping.HasManyToMany(x => x.SupportingEntities)
//            //    .ParentKeyColumn("EntityId")
//            //    .ChildKeyColumn("LearningOutcomeId")
//            //    .ForeignKeyConstraintNames("none", "none")
//            //    .Table("EntityLearningOutcome");
//        }
//    }
//}
