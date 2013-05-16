using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Repositories;
using EventStore;

namespace BpeProducts.Services.Course.Domain
{
    public class CourseFactory : ICourseFactory
    {
        private CourseEventStore _store;
        public Entities.Course Create(SaveCourseRequest request)
        {
            //TODO: get tenant id
            var course = new Entities.Course {Id = Guid.NewGuid(), ActiveFlag = true};
            Mapper.Map(request, course);
            return course;
        }
        public Entities.Course Reconstitute(Guid aggregateId)
        {
            _store = new CourseEventStore();
            Entities.Course course;

            // Get the latest snapshot
            Snapshot latestSnapshot = _store.Advanced.GetSnapshot(aggregateId, int.MaxValue);
            if (latestSnapshot == null)
            {
                using (var stream = _store.OpenStream(aggregateId, 0, int.MaxValue))
                {
					//throw if it is a new
                    course = Reconstitute(stream);
                }
            }
            else
            {
                using (var stream = _store.OpenStream(latestSnapshot, int.MaxValue))
                {
					//throw if it is a new
                    course = Reconstitute(stream, latestSnapshot.Payload as Entities.Course);
                }
            }

            return course;
        }

        public Entities.Course Reconstitute(IEventStream stream, Entities.Course course = null)
        {
            course = course ?? new Entities.Course();

            foreach (var eventMessage in stream.CommittedEvents)
            {
                var @event = eventMessage.Body;

                if (@event is Events.CourseCreated)
                {
                    course = Handle(@event as Events.CourseCreated);
                }
                if (@event is Events.CourseAssociatedWithProgram)
                {
                    course = Handle(@event as Events.CourseAssociatedWithProgram, course);
                }
                if (@event is Events.CourseDeleted)
                {
                    course = Handle(@event as Events.CourseDeleted);
                }
                if (@event is Events.CourseDisassociatedWithProgram)
                {
                    course = Handle(@event as Events.CourseDisassociatedWithProgram, course);
                }
                if (@event is Events.CourseInfoUpdated)
                {
                    course = Handle(@event as Events.CourseInfoUpdated, course);
                }
                if (@event is Events.CourseSegmentAdded)
                {
                    course = Handle(@event as Events.CourseSegmentAdded, course);
                }
                if (@event is Events.CourseSegmentUpdated)
                {
                    course = Handle(@event as Events.CourseSegmentUpdated, course);
                }
            }

            return course;
        }

        private Entities.Course Handle(Events.CourseCreated msg)
        {
            return new Entities.Course
                {
                    Id = msg.AggregateId,
                    Name = msg.Name,
                    Code = msg.Code,
                    Description = msg.Description,
					ActiveFlag = msg.ActiveFlag
                };
        }

        private Entities.Course Handle(Events.CourseAssociatedWithProgram msg, Entities.Course course)
        {
            course.Programs.Add(new Program
                {
                    Id = msg.ProgramId
                });
            return course;
        }

        private Entities.Course Handle(Events.CourseDeleted msg)
        {
            return null;
        }

        private Entities.Course Handle(Events.CourseDisassociatedWithProgram msg, Entities.Course course)
        {
            var program = course.Programs.FirstOrDefault(p => p.Id.Equals(msg.ProgramId));
            if (program != null)
            {
                course.Programs.RemoveAt(course.Programs.IndexOf(program));
            }

            return course;
        }

        private Entities.Course Handle(Events.CourseInfoUpdated msg, Entities.Course course)
        {
            course.Name = msg.Name;
            course.Code = msg.Code;
            course.Description = msg.Description;

            return course;
        }

        private Entities.Course Handle(Events.CourseSegmentAdded msg, Entities.Course course)
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

        private Entities.Course Handle(Events.CourseSegmentUpdated msg, Entities.Course course)
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
