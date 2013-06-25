using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
	public class PlayCourseInfoUpated:IPlayEvent<CourseInfoUpdated>
	{
		public Entities.Course Apply(Events.CourseInfoUpdated msg, Entities.Course course)
		{
			course.Name = msg.Name;
			course.Code = msg.Code;
			course.Description = msg.Description;
		    course.CourseType = msg.CourseType;
		    course.IsTemplate = msg.IsTemplate;

			return course;
		}

		public Entities.Course Apply<T>(T msg, Entities.Course course) where T : IDomainEvent
		{
			return Apply(msg as CourseInfoUpdated, course);
		}
	}
}