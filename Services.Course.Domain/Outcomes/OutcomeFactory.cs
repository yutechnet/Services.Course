using System;
using System.Collections.Generic;
using Autofac.Features.Indexed;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Events;
using EventStore;

namespace BpeProducts.Services.Course.Domain.Outcomes
{
    public class OutcomeFactory : VersionFactory<LearningOutcome>, IOutcomeFactory
    {
        public OutcomeFactory(IStoreEvents store, IIndex<string, IPlayEvent> index) : base(store, index)
        {
        }

        public LearningOutcome Build(OutcomeRequest request)
        {
            var outcomeId = Guid.NewGuid();
            return new LearningOutcome
                {
                    Id = outcomeId,
                    ActiveFlag = true,
                    OriginalEntityId = outcomeId,
                    Description = request.Description,
                    VersionNumber = new Version(1, 0, 0, 0).ToString(),
                    TenantId = request.TenantId
                };
        }

        public LearningOutcome BuildNewVersion(LearningOutcome entity, string version)
        {
            return new LearningOutcome
            {
                Id =  Guid.NewGuid(),
                Description = entity.Description,
                Outcomes = new List<LearningOutcome>(entity.Outcomes),
                ActiveFlag = true,
                OriginalEntityId = entity.OriginalEntityId,
                ParentEntityId = entity.Id,
                TenantId = entity.TenantId,
                VersionNumber = version
            };
        }
    }
}