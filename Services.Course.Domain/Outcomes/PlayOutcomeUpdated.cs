using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain.Outcomes
{
	public class PlayOutcomeUpdated:IPlayEvent<OutcomeUpdated, Entities.LearningOutcome>
	{
        public Entities.LearningOutcome Apply(OutcomeUpdated msg, Entities.LearningOutcome course)
        {
            course.Description = msg.Description;
			return course;
		}

        public TE Apply<T, TE>(T msg, TE entity)
            where T : IDomainEvent
            where TE : class
	    {
            return Apply(msg as OutcomeUpdated, entity as Entities.LearningOutcome) as TE;
	    }
	}
}