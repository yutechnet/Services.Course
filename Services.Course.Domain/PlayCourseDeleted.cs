using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
	public  class PlayCourseDeleted:IPlayEvent<CourseDeleted, Entities.Course>
	{
		public Entities.Course Apply(CourseDeleted msg, Entities.Course course)
		{
			course.ActiveFlag = false;
			return course;
		}

	    public TE Apply<T, TE>(T msg, TE entity) where T : IDomainEvent where TE : class
	    {
            return Apply(msg as CourseDeleted, entity as Entities.Course) as TE;
	    }
	}
}