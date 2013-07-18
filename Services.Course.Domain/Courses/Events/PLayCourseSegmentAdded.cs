using System;
using System.Collections.Generic;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain.Courses.Events
{
	public class PlayCourseSegmentAdded:IPlayEvent<CourseSegmentAdded, Entities.Course>
	{
		public Entities.Course Apply(Domain.Events.CourseSegmentAdded msg, Entities.Course course)
		{
			// Look for the parent Segment Guid in the Segment Index and add the child.
			IList<CourseSegment> parentSegmentCollection;
			if (msg.ParentSegmentId == Guid.Empty)
			{
				parentSegmentCollection = course.Segments;
			}
			else
			{
				var parentSegment = course.SegmentIndex[msg.ParentSegmentId];
				parentSegmentCollection = parentSegment.ChildrenSegments;
			}

			var segment = new CourseSegment
				{
					ParentSegmentId = msg.ParentSegmentId,
					Id = msg.Id,
					Description = msg.Description,
					Name = msg.Name,
                    Content = new List<Content>(msg.Content)
				};
			parentSegmentCollection.Add(segment);
			course.SegmentIndex[msg.Id] = segment;

			return course;
		}

        public TE Apply<T, TE>(T msg, TE entity) where T : IDomainEvent where TE : class
	    {
            return Apply(msg as CourseSegmentAdded, entity as Entities.Course) as TE;
	    }
	}
}