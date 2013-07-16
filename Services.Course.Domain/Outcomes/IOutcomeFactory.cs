using System;
using System.Collections.Generic;
using BpeProducts.Services.Course.Contract;
using EventStore;

namespace BpeProducts.Services.Course.Domain.Outcomes
{
	public interface IOutcomeFactory
	{
		Entities.LearningOutcome Build(OutcomeRequest request);
        Entities.LearningOutcome Reconstitute(Guid aggregateId);
        Entities.LearningOutcome Reconstitute(ICollection<EventMessage> stream, Entities.LearningOutcome outcome = null);
	}
}