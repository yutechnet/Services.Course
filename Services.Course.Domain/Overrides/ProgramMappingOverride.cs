﻿using System;
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
            mapping.HasMany(p => p.Courses).Inverse();
			
			mapping
				.HasManyToMany<LearningOutcome>(x => x.Outcomes)
				.ParentKeyColumn("EntityId")
				.ChildKeyColumn("LearningOutcomeId")
				.ForeignKeyConstraintNames("none", "none")
				.Table("EntityLearningOutcome");
        }
    }
}

