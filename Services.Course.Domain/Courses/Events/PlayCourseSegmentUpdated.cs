using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
    public class PlayCourseSegmentUpdated : IPlayEvent<CourseSegmentUpdated, Courses.Course>
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