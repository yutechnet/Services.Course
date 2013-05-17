using System.Linq;
using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
	public class PLayCourseDisassociatedWithProgram : IPlayEvent<CourseDisassociatedWithProgram>
	{
		public Entities.Course Apply(Events.CourseDisassociatedWithProgram msg, Entities.Course course)
		{
			var program = course.Programs.FirstOrDefault(p => p.Id.Equals(msg.ProgramId));
			if (program != null)
			{
				course.Programs.RemoveAt(course.Programs.IndexOf(program));
			}

			return course;
		}

		public Entities.Course Apply<T>(T msg, Entities.Course course) where T : IDomainEvent
		{
			return Apply(msg as CourseDisassociatedWithProgram, course);
		}
	}
}