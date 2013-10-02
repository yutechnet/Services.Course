using System.Linq;
using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
	public class PlayCourseDisassociatedWithProgram : IPlayEvent<CourseDisassociatedWithProgram, Courses.Course>
	{
		public Courses.Course Apply(Events.CourseDisassociatedWithProgram msg, Courses.Course course)
		{
            course.RemoveProgram(msg.ProgramId);
			return course;
		}

	    public TE Apply<T, TE>(T msg, TE entity) where T : IDomainEvent where TE : class
	    {
	        return Apply(msg as CourseDisassociatedWithProgram, entity as Courses.Course) as TE;
	    }
	}
}