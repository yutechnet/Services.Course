using System;
using System.Collections.Generic;
using Autofac.Features.Indexed;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using EventStore;

namespace BpeProducts.Services.Course.Domain.Outcomes
{
    public class OutcomeFactory : VersionFactory<LearningOutcome>, IOutcomeFactory
    {
        private readonly ILearningOutcomeRepository _learningOutcomeRepository;

        public OutcomeFactory(IStoreEvents store, IIndex<string, IPlayEvent> index, ILearningOutcomeRepository learningOutcomeRepository ) : base(store, index)
        {
            _learningOutcomeRepository = learningOutcomeRepository;
        }

        public LearningOutcome Build(OutcomeRequest request)
        {
            var outcomeId = Guid.NewGuid();
            var outcome = new LearningOutcome
                {
                    Id = outcomeId,
                    ActiveFlag = true,
                    Description = request.Description,
                    VersionNumber = new Version(1, 0, 0, 0).ToString(),
                    TenantId = request.TenantId
                };
            outcome.SetOriginalEntity(outcome);
            return outcome;
        }

        public LearningOutcome BuildNewVersion(LearningOutcome entity, string version)
        {
            var existing = _learningOutcomeRepository.GetVersion(entity.OriginalEntity.Id, version);

            if (existing != null)
                throw new BadRequestException(string.Format("Version {0} already exists for Outcome {1}", version, entity.OriginalEntity.Id));

            return entity.CreateVersion(version);
        }
    }
}