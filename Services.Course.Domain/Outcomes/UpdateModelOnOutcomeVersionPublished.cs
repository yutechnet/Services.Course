using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;

namespace BpeProducts.Services.Course.Domain.Outcomes
{
    public class UpdateModelOnOutcomeVersionPublished : IHandle<OutcomeVersionPublished>
    {
        public void Handle(IDomainEvent domainEvent)
        {
            throw new NotImplementedException();
        }
    }
}
