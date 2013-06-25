using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
    public class PlayCourseVersionCreated : IPlayEvent<CourseVersionCreated, Entities.Course>
    {
        public Entities.Course Apply(CourseVersionCreated msg, Entities.Course course)
        {
            return msg.NewVersion;
        }

        public TE Apply<T, TE>(T msg, TE entity)
            where T : IDomainEvent
            where TE : class
        {
            return Apply(msg as CourseVersionCreated, entity as Entities.Course) as TE;
        }
    }
}