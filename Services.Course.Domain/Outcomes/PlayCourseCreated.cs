using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;

namespace BpeProducts.Services.Course.Domain.Outcomes
{
	public class PlayOutcomeCreated:IPlayEvent<OutcomeCreated, Entities.LearningOutcome>
	{
		public Entities.LearningOutcome Apply(OutcomeCreated msg, Entities.LearningOutcome course)
		{
			course.Id = msg.AggregateId;
			course.Description = msg.Description;
			course.ActiveFlag = msg.ActiveFlag;
			return course;
		}

        public TE Apply<T, TE>(T msg, TE entity)
            where T : IDomainEvent
            where TE : class
	    {
            return Apply(msg as OutcomeCreated, entity as Entities.LearningOutcome) as TE;
	    }
	}
}