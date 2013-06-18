using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
    public class PlayCourseVersionPublished : IPlayEvent<CourseVersionPublished>
    {
        public Entities.Course Apply(CourseVersionPublished msg, Entities.Course course)
        {
            course.IsPublished = true;
            course.PublishNote = msg.PublishNote;

            return course;
        }

        public Entities.Course Apply<T>(T msg, Entities.Course course) where T : IDomainEvent
        {
            return Apply(msg as CourseVersionPublished, course);
        }
    }
}