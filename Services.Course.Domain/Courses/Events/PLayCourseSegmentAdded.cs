using System;
using System.Collections.Generic;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain.Courses.Events
{
	public class PlayCourseSegmentAdded:IPlayEvent<CourseSegmentAdded, Course>
	{
		public Course Apply(Domain.Events.CourseSegmentAdded msg, Course course)
		{
		    course.AddSegment(msg.SegmentId, msg.ParentSegmentId, msg.Request);
			return course;
		}

        public TE Apply<T, TE>(T msg, TE entity) where T : IDomainEvent where TE : class
	    {
            return Apply(msg as CourseSegmentAdded, entity as Course) as TE;
	    }
	}
}