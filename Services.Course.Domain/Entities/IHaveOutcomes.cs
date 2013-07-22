using System;
using System.Collections.Generic;

namespace BpeProducts.Services.Course.Domain.Entities
{
	public interface IHaveOutcomes
	{
		IList<LearningOutcome> Outcomes { get; }
		Guid Id { get; set; }

	    LearningOutcome AddLearningOutcome(LearningOutcome outcome);
	}
}