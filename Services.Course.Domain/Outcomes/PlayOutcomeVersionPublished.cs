using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain.Outcomes
{
    public class PlayOutcomeVersionPublished : IPlayEvent<OutcomeVersionPublished, Entities.LearningOutcome>
    {
        public Entities.LearningOutcome Apply(OutcomeVersionPublished msg, LearningOutcome entity)
        {
            entity.IsPublished = true;
            entity.PublishNote = msg.PublishNote;

            return entity;
        }

        public TE Apply<T, TE>(T msg, TE entity)
            where T : IDomainEvent
            where TE : class
        {
            return Apply(msg as OutcomeVersionPublished, entity as Entities.LearningOutcome) as TE;
        }
    }
}
