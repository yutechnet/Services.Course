using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using EventStore;

namespace BpeProducts.Services.Course.Domain
{
    public class CourseFactory : ICourseFactory
    {
	    public CourseFactory(IStoreEvents store)
	    {
		    _store = store;
	    }

		private IStoreEvents _store;

	    

	    public Entities.Course Create(SaveCourseRequest request)
        {
            //TODO: get tenant id
            var course = new Entities.Course {Id = Guid.NewGuid(), ActiveFlag = true};
            Mapper.Map(request, course);
            return course;
        }

        public Entities.Course Reconstitute(Guid aggregateId)
        {
           
            Entities.Course course;

            // Get the latest snapshot
            Snapshot latestSnapshot = _store.Advanced.GetSnapshot(aggregateId, int.MaxValue);
            if (latestSnapshot == null)
            {
                using (var stream = _store.OpenStream(aggregateId, 0, int.MaxValue))
                {
					//throw if it is a new
                    course = Reconstitute(stream.CommittedEvents);
                }
            }
            else
            {
                using (var stream = _store.OpenStream(latestSnapshot, int.MaxValue))
                {
					//throw if it is a new
                    course = Reconstitute(stream.CommittedEvents, latestSnapshot.Payload as Entities.Course);
                }
            }
			return course;
        }

        public Entities.Course Reconstitute(ICollection<EventMessage> events, Entities.Course course = null)
        {
            course = course ?? new Entities.Course();

            foreach (var eventMessage in events)
            {
                var eventBody = eventMessage.Body;

                if (eventBody is Events.CourseCreated)
                {
                    course = Apply(eventBody as Events.CourseCreated,course);
                }
                if (eventBody is Events.CourseAssociatedWithProgram)
                {
                    course = Apply(eventBody as Events.CourseAssociatedWithProgram, course);
                }
                if (eventBody is Events.CourseDeleted)
                {
                    course = Apply(eventBody as Events.CourseDeleted,course);
                }
                if (eventBody is Events.CourseDisassociatedWithProgram)
                {
                    course = Apply(eventBody as Events.CourseDisassociatedWithProgram, course);
                }
                if (eventBody is Events.CourseInfoUpdated)
                {
                    course = Apply(eventBody as Events.CourseInfoUpdated, course);
                }
                if (eventBody is Events.CourseSegmentAdded)
                {
                    course = Apply(eventBody as Events.CourseSegmentAdded, course);
                }
                if (eventBody is Events.CourseSegmentUpdated)
                {
                    course = Apply(eventBody as Events.CourseSegmentUpdated, course);
                }
            }

            return course;
        }

        private Entities.Course Apply<T>(T msg, Entities.Course course) where T:CourseCreated
        {
			 
            		course.Id = msg.AggregateId;
					course.Name = msg.Name;
					course.Code = msg.Code;
					course.Description = msg.Description;
					course.ActiveFlag = msg.ActiveFlag;
	        return course;
        }

        private Entities.Course Apply(Events.CourseAssociatedWithProgram msg, Entities.Course course)
        {
            course.Programs.Add(new Program
                {
                    Id = msg.ProgramId
                });
            return course;
        }

        private Entities.Course Apply(CourseDeleted msg, Entities.Course course)
        {
	        course.ActiveFlag = false;
            return course;
        }

        private Entities.Course Apply(Events.CourseDisassociatedWithProgram msg, Entities.Course course)
        {
            var program = course.Programs.FirstOrDefault(p => p.Id.Equals(msg.ProgramId));
            if (program != null)
            {
                course.Programs.RemoveAt(course.Programs.IndexOf(program));
            }

            return course;
        }

        private Entities.Course Apply(Events.CourseInfoUpdated msg, Entities.Course course)
        {
            course.Name = msg.Name;
            course.Code = msg.Code;
            course.Description = msg.Description;

            return course;
        }

        private Entities.Course Apply(Events.CourseSegmentAdded msg, Entities.Course course)
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

        private Entities.Course Apply(Events.CourseSegmentUpdated msg, Entities.Course course)
        {
            var segment = course.SegmentIndex[msg.SegmentId];
            //segment.ParentSegmentId = msg.ParentSegmentId;//todo:if parent segment changed??
            segment.Name = msg.Name;
            segment.Description = msg.Description;
            segment.Type = msg.Type;
            return course;
        }
    }

}
