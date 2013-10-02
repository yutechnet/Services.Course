using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
    public class PlayCourseSegmentReordered : IPlayEvent<CourseSegmentReordered, Courses.Course>
    {
        public Courses.Course Apply(CourseSegmentReordered msg, Courses.Course course)
        {
            course.ReorderSegment(msg.SegmentId, msg.Request, msg.DisplayOrder);
            return course;
        }

        public TE Apply<T, TE>(T msg, TE entity) where T : IDomainEvent where TE : class
        {
            return Apply(msg as CourseSegmentReordered, entity as Courses.Course) as TE;
        }
    }
}