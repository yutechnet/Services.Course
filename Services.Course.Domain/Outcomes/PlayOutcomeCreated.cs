using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;

namespace BpeProducts.Services.Course.Domain.Outcomes
{
	public class PlayOutcomeCreated:IPlayEvent<OutcomeCreated, Entities.LearningOutcome>
	{
		public Entities.LearningOutcome Apply(OutcomeCreated msg, Entities.LearningOutcome learningOutcome)
		{
            learningOutcome.Id = msg.AggregateId;
            learningOutcome.Description = msg.Description;
            learningOutcome.ActiveFlag = msg.ActiveFlag;
            return learningOutcome;
		}

        public TE Apply<T, TE>(T msg, TE entity)
            where T : IDomainEvent
            where TE : class
	    {
            return Apply(msg as OutcomeCreated, entity as Entities.LearningOutcome) as TE;
	    }
	}
}