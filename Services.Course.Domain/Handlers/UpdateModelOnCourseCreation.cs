//using System.Web.Http.Odata.Query;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BpeProducts.Services.Course.Domain.Handlers
{
	public class UpdateModelOnCourseCreation : IHandle<CourseCreated>
	{
	    private readonly ICourseFactory _courseFactory;
	    private readonly ICourseRepository _courseRepository;
	    private readonly IDomainEvents _domainEvents;

	    public UpdateModelOnCourseCreation(ICourseFactory courseFactory, ICourseRepository courseRepository, IDomainEvents domainEvents)
		{
	        _courseFactory = courseFactory;
	        _courseRepository = courseRepository;
		    _domainEvents = domainEvents;
		}

	    public void Handle(IDomainEvent domainEvent)
		{
			var e = domainEvent as CourseCreated;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }

			_courseRepository.Save(e.Course);

	        if (e.Course.Template != null)
	        {
	            DeepCopySegments(e.Course.Id, Guid.Empty, e.Course.Template.Segments);

                //TODO - Copy everything else (learning objectives, required materials, programs, etc.)
	        }
		}

        private void DeepCopySegments(Guid courseId, Guid parentSegmentId, IEnumerable<CourseSegment> segments)
        {
            foreach (var segment in segments)
            {
                var newId = Guid.NewGuid();
                _domainEvents.Raise<CourseSegmentAdded>(new CourseSegmentAdded
                    {
                        AggregateId = courseId,
                        Description = segment.Description,
                        DiscussionId = segment.DiscussionId,
                        Name = segment.Name,
                        ParentSegmentId = parentSegmentId,
                        Id = newId,
                        Type = segment.Type
                    });

                if (segment.ChildrenSegments.Any())
                    DeepCopySegments(courseId, newId, segment.ChildrenSegments);
            }
        }
	}
}
