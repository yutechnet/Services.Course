using System;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Outcomes
{
	public class UpdateModelOnOutcomeUpdate : IHandle<OutcomeUpdated>
	{
        private readonly ILearningOutcomeRepository _outcomeRepository;
        public UpdateModelOnOutcomeUpdate(ILearningOutcomeRepository outcomeRepository)
		{
			_outcomeRepository = outcomeRepository;
		}
		public void Handle(IDomainEvent domainEvent)
		{
            var e = domainEvent as OutcomeUpdated;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }

			var outcome = _outcomeRepository.Load(e.AggregateId);
		    outcome.Description = e.Description;
            _outcomeRepository.Delete(outcome);
		}
	}
}