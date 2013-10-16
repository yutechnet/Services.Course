using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Outcomes
{
    public class UpdateModelOnOutcomeVersionPublished : IHandle<OutcomeVersionPublished>
    {
        private readonly IRepository _repository;
        
        public UpdateModelOnOutcomeVersionPublished(IRepository repository)
        {
            _repository = repository;
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
