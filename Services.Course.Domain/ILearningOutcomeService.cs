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
        OutcomeResponse Get(Guid outcomeId);
        OutcomeResponse Get(string entityType, Guid entityId, Guid outcommeId);
        IEnumerable<OutcomeResponse> Get(string entityType, Guid entityId);

        OutcomeResponse Create(OutcomeRequest request);
        OutcomeResponse Create(string entityType, Guid entityId, OutcomeRequest request);

        void Update(Guid outcomeId, OutcomeRequest request);
        void Update(string entityType, Guid entityId, Guid outcomeId, OutcomeRequest request);

        void Delete(Guid outcomeId);
        void Delete(string entityType, Guid entityId, Guid outcomeId);
    }
}
