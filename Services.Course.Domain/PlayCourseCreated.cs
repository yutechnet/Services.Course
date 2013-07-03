using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
	public class PlayCourseCreated:IPlayEvent<CourseCreated, Entities.Course>
	{
		public Entities.Course Apply(CourseCreated msg, Entities.Course course)
		{
		    return msg.Course;
		}

        public TE Apply<T, TE>(T msg, TE entity)
            where T : IDomainEvent
            where TE : class
	    {
            return Apply(msg as CourseCreated, entity as Entities.Course) as TE;
	    }
	}
}