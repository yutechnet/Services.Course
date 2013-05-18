using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.NHibernate;
using NHibernate;

namespace BpeProducts.Services.Course.Domain.Repositories
{
    public class LearningOutcomeRepository : RepositoryBase<Entities.LearningOutcome>, ILearningOutcomeRepository
    {
        public LearningOutcomeRepository(ISession session) : base(session)
        {
        }
    }
}
