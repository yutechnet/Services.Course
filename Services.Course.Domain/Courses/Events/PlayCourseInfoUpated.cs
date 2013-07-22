using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
	public class PlayCourseInfoUpated:IPlayEvent<CourseInfoUpdated, Courses.Course>
	{
		public Courses.Course Apply(Events.CourseInfoUpdated msg, Courses.Course course)
		{
			course.Name = msg.Name;
			course.Code = msg.Code;
			course.Description = msg.Description;
		    course.CourseType = msg.CourseType;
		    course.IsTemplate = msg.IsTemplate;

			return course;
		}

        public TE Apply<T, TE>(T msg, TE entity) where T : IDomainEvent where TE : class
	    {
            return Apply(msg as CourseInfoUpdated, entity as Courses.Course) as TE;
	    }
	}
}