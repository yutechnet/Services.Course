using System;
using System.Linq;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.Ioc.Validation;
using BpeProducts.Services.Course.Domain.Entities;
using NHibernate;
using NHibernate.Linq;

namespace BpeProducts.Services.Course.Domain.Repositories
{
    [Validate]
    public class LearningOutcomeRepository : ILearningOutcomeRepository
    {
        private readonly ISession _session;

        public LearningOutcomeRepository(ISession session)
        {
            _session = session;
        }

        public Entities.LearningOutcome Get(Guid outcomeId)
        {
            return _session.Get<Entities.LearningOutcome>(outcomeId);
        }

        public LearningOutcome Load(Guid outcomeId)
        {
            var outcome = Get(outcomeId);

            if (outcome == null)
            {
                throw new NotFoundException(string.Format("Learing outcome {0} not found.", outcomeId));
            }

            return outcome;
        }

        public void Save(Entities.LearningOutcome outcome)
        {
            _session.SaveOrUpdate(outcome);
        }

        public void Delete(Entities.LearningOutcome outcome)
        {
            outcome.ActiveFlag = false;
            _session.SaveOrUpdate(outcome);
        }

        public Entities.LearningOutcome GetVersion(Guid originalEntityId, string versionNumber)
        {
            var version = (from c in _session.Query<Entities.LearningOutcome>()
                           where c.OriginalEntity.Id == originalEntityId && c.VersionNumber == versionNumber
                           select c).FirstOrDefault();

            return version;
        }
    }
}