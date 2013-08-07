using System;
using System.Collections.Generic;
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

        public LearningOutcome Get(Guid outcomeId)
        {
            return _session.Get<LearningOutcome>(outcomeId);
        }

        public Dictionary<Guid, List<LearningOutcome>> GetBySupportingEntities(IList<Guid> entityIds)
        {
            var groupedOutcomes = (from se in _session.Query<ISupportingEntity>()
                            from o in se.SupportedOutcomes
                            where entityIds.Contains(se.Id)
                            group o by se.Id into g
                            select new {EntityId = g.Key, LearningOutcomes = g}).ToList();

            return groupedOutcomes.ToDictionary(outcomeGroup => outcomeGroup.EntityId, outcomeGroup => new List<LearningOutcome>(outcomeGroup.LearningOutcomes));
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

        public void Save(LearningOutcome outcome)
        {
            _session.SaveOrUpdate(outcome);
        }

        public void Delete(LearningOutcome outcome)
        {
            outcome.ActiveFlag = false;
            _session.SaveOrUpdate(outcome);
        }

        public LearningOutcome GetVersion(Guid originalEntityId, string versionNumber)
        {
            var version = (from c in _session.Query<LearningOutcome>()
                           where c.OriginalEntity.Id == originalEntityId && c.VersionNumber == versionNumber
                           select c).FirstOrDefault();

            return version;
        }
    }
}