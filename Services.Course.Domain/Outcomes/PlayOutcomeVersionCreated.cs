﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain.Outcomes
{
    public class PlayOutcomeVersionCreated : IPlayEvent<OutcomeVersionCreated, Entities.LearningOutcome>
    {
        public Entities.LearningOutcome Apply(OutcomeVersionCreated msg, LearningOutcome entity)
        {
            return msg.NewVersion;
        }

        public TE Apply<T, TE>(T msg, TE entity) where T : IDomainEvent where TE : class
        {
            return Apply(msg as OutcomeVersionCreated, entity as Entities.LearningOutcome) as TE;
        }
    }
}
