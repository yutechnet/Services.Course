using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
    public class PlayCourseVersionCreated : IPlayEvent<CourseVersionCreated>
    {
        public Entities.Course Apply(CourseVersionCreated msg, Entities.Course course)
        {
            course.Id = msg.AggregateId;
            course.ParentCourseId = msg.ParentCourseId;
            course.OriginalCourseId = msg.OriginalCourseId;
            course.VersionNumber = msg.VersionNumber;
            course.IsPublished = msg.IsPublished;

            return course;

        }

        public Entities.Course Apply<T>(T msg, Entities.Course course) where T : IDomainEvent
        {
            return Apply(msg as CourseVersionCreated, course);
        }
    }
}