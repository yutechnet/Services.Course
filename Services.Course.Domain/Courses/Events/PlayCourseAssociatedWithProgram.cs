using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
	public class PlayCourseAssociatedWithProgram : IPlayEvent<CourseAssociatedWithProgram, Entities.Course>
	{
		public Entities.Course Apply(Events.CourseAssociatedWithProgram msg, Entities.Course course)
		{
			course.Programs.Add(new Program
				{
					Id = msg.ProgramId,
                    Name = msg.Name,
                    Description = msg.Description,
                    ProgramType = msg.ProgramType,
                    TenantId = course.TenantId,
                    OrganizationId = course.OrganizationId
				});
			return course;
		}

	    public TE Apply<T, TE>(T msg, TE entity) where T : IDomainEvent where TE : class
	    {
            return Apply(msg as CourseAssociatedWithProgram, entity as Entities.Course) as TE;
	    }
	}
}