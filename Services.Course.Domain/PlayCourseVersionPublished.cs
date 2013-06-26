using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
    public class PlayCourseVersionPublished : IPlayEvent<CourseVersionPublished, Entities.Course>
    {
        public Entities.Course Apply(CourseVersionPublished msg, Entities.Course course)
        {
            course.IsPublished = true;
            course.PublishNote = msg.PublishNote;

            return course;
        }

        public TE Apply<T, TE>(T msg, TE entity)
            where T : IDomainEvent
            where TE : class
        {
            return Apply(msg as CourseVersionPublished, entity as Entities.Course) as TE;
        }
    }
}