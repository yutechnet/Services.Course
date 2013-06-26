using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain.Outcomes
{
	public  class PlayOutcomeDeleted:IPlayEvent<OutcomeDeleted, Entities.LearningOutcome>
	{
        public Entities.LearningOutcome Apply(OutcomeDeleted msg, Entities.LearningOutcome course)
		{
			course.ActiveFlag = false;
			return course;
		}

	    public TE Apply<T, TE>(T msg, TE entity) where T : IDomainEvent where TE : class
	    {
            return Apply(msg as OutcomeDeleted, entity as Entities.LearningOutcome) as TE;
	    }
	}
}