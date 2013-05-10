using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Repositories;
using EventStore;

namespace BpeProducts.Services.Course.Domain
{
    public class CourseFactory
    {
        Entities.Course _course;
        private CourseEventStore _store;

        public Entities.Course Create(Guid aggregateId)
        {
            _store = new CourseEventStore();
            using (var stream = _store.OpenStream(aggregateId,0,int.MaxValue))
            {
                _course = Create(stream);
            }
            return _course;
        }

        public Entities.Course Create(IEventStream stream, Entities.Course course = null)
        {
            
            _course = course ?? new Entities.Course();

            foreach (var eventMessage in stream.CommittedEvents)
            {
                var @event = eventMessage.Body;

                if (@event is Events.CourseCreated)
                {
                    _course = Handle(@event as Events.CourseCreated);
                }
                if (@event is Events.CourseAssociatedWithProgram)
                {
                    _course = Handle(@event as Events.CourseAssociatedWithProgram);
                }
                if (@event is Events.CourseDeleted)
                {
                    _course = Handle(@event as Events.CourseDeleted);
                }
                if (@event is Events.CourseDisassociatedWithProgram)
                {
                    _course = Handle(@event as Events.CourseDisassociatedWithProgram);
                }
                if (@event is Events.CourseInfoUpdated)
                {
                    _course = Handle(@event as Events.CourseInfoUpdated);
                }
                if (@event is Events.CourseSegmentAdded)
                {
                    _course = Handle(@event as Events.CourseSegmentAdded);
                }
            }

            return _course;
        }

        private Entities.Course Handle(Events.CourseCreated msg)
        {
            return _course = new Entities.Course
                {
                    Id = msg.AggregateId,
                    Name = msg.Name,
                    Code = msg.Code,
                    Description = msg.Description
                };
        }

        private Entities.Course Handle(Events.CourseAssociatedWithProgram msg)
        {
            _course.Programs.Add(new Program
                {
                    Id = msg.ProgramId
                });
            return _course;
        }

        private Entities.Course Handle(Events.CourseDeleted msg)
        {
            return _course = null;
        }

        private Entities.Course Handle(Events.CourseDisassociatedWithProgram msg)
        {
            var program = _course.Programs.FirstOrDefault(p => p.Id.Equals(msg.ProgramId));
            if (program != null)
            {
                _course.Programs.RemoveAt(_course.Programs.IndexOf(program));
            }

            return _course;
        }

        private Entities.Course Handle(Events.CourseInfoUpdated msg)
        {
            _course.Name = msg.Name;
            _course.Code = msg.Code;
            _course.Description = msg.Description;

            return _course;
        }

        private Entities.Course Handle(Events.CourseSegmentAdded msg)
        {
            // Look for the parent Segment Guid in the Segment Index and add the child.
            IList<CourseSegment> parentSegmentCollection;
            CourseSegment parentSegment = null;
            if (msg.ParentSegmentId == Guid.Empty)
            {
                parentSegmentCollection = _course.Segments;
            }
            else
            {
                parentSegment = _course.SegmentIndex[msg.ParentSegmentId];
                parentSegmentCollection = parentSegment.ChildrenSegments;
            }
            
            var segment = new CourseSegment
                {
                    ParentSegment = parentSegment, 
                    Id = msg.SegmentId,
                    Description = msg.Description,
                    Name = msg.Name
                };
            parentSegmentCollection.Add(segment);
            _course.SegmentIndex[msg.SegmentId] = segment;

            return _course;
        }
    }

}
