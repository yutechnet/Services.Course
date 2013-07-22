using System.Linq;
using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
	public class PlayCourseDisassociatedWithProgram : IPlayEvent<CourseDisassociatedWithProgram, Courses.Course>
	{
		public Courses.Course Apply(Events.CourseDisassociatedWithProgram msg, Courses.Course course)
		{
			var program = course.Programs.FirstOrDefault(p => p.Id.Equals(msg.ProgramId));
			if (program != null)
			{
				course.Programs.RemoveAt(course.Programs.IndexOf(program));
			}

			return course;
		}

	    public TE Apply<T, TE>(T msg, TE entity) where T : IDomainEvent where TE : class
	    {
	        return Apply(msg as CourseDisassociatedWithProgram, entity as Courses.Course) as TE;
	    }
	}
}