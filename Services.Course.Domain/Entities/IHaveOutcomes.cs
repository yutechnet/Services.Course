using System;
using System.Collections.Generic;

namespace BpeProducts.Services.Course.Domain.Entities
{
	public interface IHaveOutcomes
	{
		IList<LearningOutcome> SupportingOutcomes { get; }
		Guid Id { get; set; }

	    LearningOutcome SupportOutcome(LearningOutcome outcome);
	}
}