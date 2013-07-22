using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
	public class PlayCourseSegmentUpdated:IPlayEvent<CourseSegmentUpdated, Courses.Course>
	{
		public Courses.Course Apply(Events.CourseSegmentUpdated msg, Courses.Course course)
		{
		    course.UpdateSegment(msg.SegmentId, msg.Request);
			return course;
		}

        public TE Apply<T, TE>(T msg, TE entity)
            where T : IDomainEvent
            where TE : class
	    {
            return Apply(msg as CourseSegmentUpdated, entity as Courses.Course) as TE;
	    }
	}
}