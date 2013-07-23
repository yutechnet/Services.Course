using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Outcomes
{
    public class UpdateModelOnOutcomeVersionPublished : IHandle<OutcomeVersionPublished>
    {
        private readonly IRepository _repository;
        private readonly IOutcomeFactory _outcomeFactory;

        public UpdateModelOnOutcomeVersionPublished(IRepository repository, IOutcomeFactory outcomeFactory)
        {
            _repository = repository;
            _outcomeFactory = outcomeFactory;
        }

        public void Handle(IDomainEvent domainEvent)
        {
            var e = domainEvent as OutcomeVersionPublished;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }

            var entity = _repository.Query<Entities.LearningOutcome>().SingleOrDefault(r => r.Id.Equals(e.AggregateId));

            if (entity == null)
            {
                throw new NotFoundException(string.Format("Learning outcome {0} not found.", e.AggregateId));
            }

            entity.Publish(e.PublishNote);
            
            _repository.Update(entity);
        }
    }
}
