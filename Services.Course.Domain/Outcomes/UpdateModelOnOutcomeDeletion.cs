using System;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Outcomes
{
	public class UpdateModelOnOutcomeDeletion : IHandle<OutcomeDeleted>
	{
        private readonly ILearningOutcomeRepository _outcomeRepository;
        public UpdateModelOnOutcomeDeletion(ILearningOutcomeRepository outcomeRepository)
		{
			_outcomeRepository = outcomeRepository;
		}
		public void Handle(IDomainEvent domainEvent)
		{
            var e = domainEvent as OutcomeDeleted;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }

			var outcome = _outcomeRepository.Get(e.AggregateId);
			_outcomeRepository.Delete(outcome);
		}
	}
}