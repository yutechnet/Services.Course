using System;

namespace BpeProducts.Services.Course.Domain.Repositories
{
    public interface ILearningOutcomeRepository
    {
        Entities.LearningOutcome Get(Guid outcomeId);
        Entities.LearningOutcome Load(Guid outcomeId);
        void Save(Entities.LearningOutcome outcome);
        void Delete(Entities.LearningOutcome outcome);
        Entities.LearningOutcome GetVersion(Guid originalEntityId, string versionNumber);
    }
}