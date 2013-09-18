using System;
using System.Collections.Generic;
using Autofac.Features.Indexed;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using EventStore;

namespace BpeProducts.Services.Course.Domain.Outcomes
{
    public class OutcomeFactory : VersionFactory<LearningOutcome>, IOutcomeFactory
    {
        private readonly IRepository _learningOutcomeRepository;

        public OutcomeFactory(IStoreEvents store, IIndex<string, IPlayEvent> index, IRepository learningOutcomeRepository)
            : base(store, index,learningOutcomeRepository)
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
                    TenantId = request.TenantId
                };

            return outcome;
        }
    }
}