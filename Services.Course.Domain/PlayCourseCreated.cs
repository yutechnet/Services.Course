using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
	public class PlayCourseCreated:IPlayEvent<CourseCreated, Entities.Course>
	{
		public Entities.Course Apply(CourseCreated msg, Entities.Course course)
		{
			course.Id = msg.AggregateId;
			course.Name = msg.Name;
			course.Code = msg.Code;
			course.Description = msg.Description;
			course.ActiveFlag = msg.ActiveFlag;
			return course;
		}

        public TE Apply<T, TE>(T msg, TE entity)
            where T : IDomainEvent
            where TE : class
	    {
            return Apply(msg as CourseCreated, entity as Entities.Course) as TE;
	    }
	}
}