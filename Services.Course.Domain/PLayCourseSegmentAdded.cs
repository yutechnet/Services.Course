using System;
using System.Collections.Generic;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
	public class PLayCourseSegmentAdded:IPlayEvent<CourseSegmentAdded>
	{
		public Entities.Course Apply(Events.CourseSegmentAdded msg, Entities.Course course)
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
					Name = msg.Name
				};
			parentSegmentCollection.Add(segment);
			course.SegmentIndex[msg.Id] = segment;

			return course;
		}

		public Entities.Course Apply<T>(T msg, Entities.Course course) where T : IDomainEvent
		{
			return Apply(msg as CourseSegmentAdded, course);
		}
	}
}