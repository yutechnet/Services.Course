using System;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain.Handlers
{
	public class UpdateModelOnAddingCourseLearningActivity : IHandle<CourseLearningActivityAdded>
	{
		private readonly IRepository _repository;

		public UpdateModelOnAddingCourseLearningActivity(IRepository repository)
		{
			_repository = repository;
		}

		public void Handle(IDomainEvent domainEvent)
		{
			var e = domainEvent as CourseLearningActivityAdded;
			if (e == null)
			{
				throw new InvalidOperationException("Invalid domain event.");
			}

			var course = _repository.Get<Courses.Course>(e.AggregateId);
			course.AddLearningActivity(e.SegmentId, e.Request, e.LearningActivityId);

			_repository.Save(course);
		}
	}
}