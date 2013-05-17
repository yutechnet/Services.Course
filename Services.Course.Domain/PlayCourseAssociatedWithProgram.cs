using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
	public class PlayCourseAssociatedWithProgram : IPlayEvent<CourseAssociatedWithProgram>
	{
		public Entities.Course Apply(Events.CourseAssociatedWithProgram msg, Entities.Course course)
		{
			course.Programs.Add(new Program
				{
					Id = msg.ProgramId
				});
			return course;
		}

		public Entities.Course Apply<T>(T msg, Entities.Course course) where T : IDomainEvent
		{
			return Apply(msg as CourseAssociatedWithProgram, course);
		}
	}
}