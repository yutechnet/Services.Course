using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain.Courses.Events
{
    public class PlayCourseLearningActivityUpdated:IPlayEvent<CourseLearningActivityUpdated, Courses.Course>
    {

        public Course Apply(Domain.Events.CourseLearningActivityUpdated msg, Courses.Course course)
		{
            course.UpdateLearningActivity(msg.SegmentId, msg.LearningActivityId, msg.Request);
			return course;
		}

        public TE Apply<T, TE>(T msg, TE entity)
            where T : IDomainEvent
            where TE : class
	    {
            return Apply(msg as CourseLearningActivityUpdated, entity as Courses.Course) as TE;
	    }

    }
}
