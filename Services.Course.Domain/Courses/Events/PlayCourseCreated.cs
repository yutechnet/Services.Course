using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
	public class PlayCourseCreated:IPlayEvent<CourseCreated, Courses.Course>
	{
		public Courses.Course Apply(CourseCreated msg, Courses.Course course)
		{
		    return msg.Course;
		}

        public TE Apply<T, TE>(T msg, TE entity)
            where T : IDomainEvent
            where TE : class
	    {
            return Apply(msg as CourseCreated, entity as Courses.Course) as TE;
	    }
	}
}