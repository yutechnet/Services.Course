using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Outcomes
{
    public class UpdateModelOnOutcomeVersionCreation : IHandle<OutcomeVersionCreated>
    {
        private readonly IRepository _repository;
        private readonly IOutcomeFactory _outcomeFactory;

        public UpdateModelOnOutcomeVersionCreation(IRepository repository, IOutcomeFactory outcomeFactory)
        {
            _repository = repository;
            _outcomeFactory = outcomeFactory;
        }

        public void Handle(IDomainEvent domainEvent)
        {
            var e = domainEvent as OutcomeVersionCreated;
            if (e == null)
            {
                throw new InvalidOperationException("Invalid domain event.");
            }

            var entity = _repository.Query<Entities.LearningOutcome>().SingleOrDefault(r => r.Id.Equals(e.ParentEntityId));

            var newVersion = _outcomeFactory.BuildNewVersion(entity, e.VersionNumber);
            _repository.Save(newVersion);
        }
    }
}
