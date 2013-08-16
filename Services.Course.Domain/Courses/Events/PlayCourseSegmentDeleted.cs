using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
    public class PlayCourseSegmentDeleted : IPlayEvent<CourseSegmentDeleted, Courses.Course>
    {
        public Courses.Course Apply(CourseSegmentDeleted msg, Courses.Course course)
        {
            course.DeleteSegment(msg.SegmentId);
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