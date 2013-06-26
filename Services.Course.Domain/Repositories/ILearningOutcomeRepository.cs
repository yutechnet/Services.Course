using System;

namespace BpeProducts.Services.Course.Domain.Repositories
{
    public interface ILearningOutcomeRepository
    {
        Entities.LearningOutcome Load(Guid programId);
        void Save(Entities.LearningOutcome outcome);
        void Delete(Entities.LearningOutcome outcome);
        Entities.LearningOutcome GetVersion(Guid originalEntityId, string versionNumber);
    }
}