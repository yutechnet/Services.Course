using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
	public class PlayCourseSegmentUpdated:IPlayEvent<CourseSegmentUpdated>
	{
		public Entities.Course Apply(Events.CourseSegmentUpdated msg, Entities.Course course)
		{
			var segment = course.SegmentIndex[msg.SegmentId];
			//segment.ParentSegmentId = msg.ParentSegmentId;//todo:if parent segment changed??
			segment.Name = msg.Name;
			segment.Description = msg.Description;
			segment.Type = msg.Type;
			return course;
		}

		public Entities.Course Apply<T>(T msg, Entities.Course course) where T : IDomainEvent
		{
			return Apply(msg as CourseSegmentUpdated, course);
		}
	}
}