using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain.Courses.Events
{
    public class PlayCourseLearningActivityDeleted:IPlayEvent<CourseLearningActivityDeleted, Courses.Course>
    {

        public Courses.Course Apply(CourseLearningActivityDeleted msg, Courses.Course course)
		{
            course.DeleteLearningActivity(msg.SegmentId, msg.LearningActivityId );
			return course;
		}

        public TE Apply<T, TE>(T msg, TE entity) where T : IDomainEvent where TE : class
	    {
            return Apply(msg as CourseLearningActivityDeleted, entity as Courses.Course) as TE;
	    }

    }
}