using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
	public  class PlayCourseDeleted:IPlayEvent<CourseDeleted, Courses.Course>
	{
		public Courses.Course Apply(CourseDeleted msg, Courses.Course course)
		{
			course.ActiveFlag = false;
			return course;
		}

	    public TE Apply<T, TE>(T msg, TE entity) where T : IDomainEvent where TE : class
	    {
            return Apply(msg as CourseDeleted, entity as Courses.Course) as TE;
	    }
	}
}