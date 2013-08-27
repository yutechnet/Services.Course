using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain.Courses.Events
{
	public class PlayCoursePrerequisiteRemoved : IPlayEvent<CoursePrerequisiteRemoved, Course>
	{

		public Course Apply(Domain.Events.CoursePrerequisiteRemoved msg, Course course)
		{

			course.RemovePrerequisite(msg.PrerequisiteCourseId);
			return course;
		}

		public TE Apply<T, TE>(T msg, TE entity)
			where T : IDomainEvent
			where TE : class
		{
			return Apply(msg as CoursePrerequisiteRemoved, entity as Course) as TE;
		}
	}
}