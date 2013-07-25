using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;

namespace BpeProducts.Services.Course.Domain
{
    public interface ILearningOutcomeService
    {
        OutcomeInfo Get(Guid outcomeId);
        OutcomeInfo Get(string entityType, Guid entityId, Guid outcommeId);
        IEnumerable<OutcomeInfo> Get(string entityType, Guid entityId);

        OutcomeInfo Create(OutcomeRequest request);
        OutcomeInfo Create(string entityType, Guid entityId, OutcomeRequest request);

        void Update(Guid outcomeId, OutcomeRequest request);
        void Update(string entityType, Guid entityId, Guid outcomeId, OutcomeRequest request);

        void Delete(Guid outcomeId);
        void Delete(string entityType, Guid entityId, Guid outcomeId);
        LearningOutcome AddSupportingOutcome(Guid supportingOutcomeId, Guid supportedOutcomeId);
        void RemoveSupportingOutcome(Guid supportingOutcomeId, Guid supportedOutcomeId);
        List<OutcomeInfo> GetSupportingOutcomes(Guid supportingOutcomeId);
    }
}
