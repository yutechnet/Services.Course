using System;
using System.Collections.Generic;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain.Courses.Events
{
	public class PlayCourseLearningActivityAdded:IPlayEvent<CourseLearningActivityAdded, Course>
	{
		public Course Apply(Domain.Events.CourseLearningActivityAdded msg, Course course)
		{
		    course.AddLearningActivity(msg.SegmentId, msg.Request, msg.LearningActivityId);
			return course;
		}

        public TE Apply<T, TE>(T msg, TE entity) where T : IDomainEvent where TE : class
	    {
            return Apply(msg as CourseLearningActivityAdded, entity as Course) as TE;
	    }
	}
}