using System;
using System.Collections.Generic;
using BpeProducts.Services.Course.Domain.Entities;

namespace BpeProducts.Services.Course.Domain.Repositories
{
    public interface ILearningOutcomeRepository
    {
        LearningOutcome Get(Guid outcomeId);
        Dictionary<Guid, List<LearningOutcome>> GetBySupportingEntities(IList<Guid> entityIds);
        LearningOutcome Load(Guid outcomeId);
        void Save(LearningOutcome outcome);
        void Delete(LearningOutcome outcome);
        LearningOutcome GetVersion(Guid originalEntityId, string versionNumber);
    }
}