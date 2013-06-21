using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
	public class PlayCourseCreated:IPlayEvent<CourseCreated>
	{
		public Entities.Course Apply(CourseCreated msg, Entities.Course course)
		{
			course.Id = msg.AggregateId;
		    course.TemplateCourseId = msg.TemplateCourseId;
			course.Name = msg.Name;
			course.Code = msg.Code;
			course.Description = msg.Description;
			course.ActiveFlag = msg.ActiveFlag;
		    course.OrganizationId = msg.OrganizationId;
			return course;
		}

		public Entities.Course Apply<T>(T msg, Entities.Course course) where T : IDomainEvent
		{
			return Apply(msg as CourseCreated, course);
		}
	}
}