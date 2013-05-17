using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
	public  class PlayCourseDeleted:IPlayEvent<CourseDeleted>
	{
		public Entities.Course Apply(CourseDeleted msg, Entities.Course course)
		{
			course.ActiveFlag = false;
			return course;
		}

		public Entities.Course Apply<T>(T msg, Entities.Course course) where T : IDomainEvent
		{
			return Apply(msg as CourseDeleted, course);
		}
	}
}