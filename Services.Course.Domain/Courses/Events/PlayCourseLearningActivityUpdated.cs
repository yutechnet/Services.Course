using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
    public class PlayCourseLearningActivityUpdated:IPlayEvent<CourseLearningActivityUpdated, Courses.Course>
    {

        public Courses.Course Apply(Events.CourseLearningActivityUpdated msg, Courses.Course course)
		{
            course.UpdateLearningActivity(msg.SegmentId, msg.LearningActivityId, msg.Request);
			return course;
		}

        public TE Apply<T, TE>(T msg, TE entity)
            where T : IDomainEvent
            where TE : class
	    {
            return Apply(msg as CourseLearningActivityUpdated, entity as Courses.Course) as TE;
	    }

    }
}
