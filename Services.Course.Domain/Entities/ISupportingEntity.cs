using System;
using System.Collections.Generic;

namespace BpeProducts.Services.Course.Domain.Entities
{
	public interface ISupportingEntity
	{
        Guid Id { get; set; }

		IList<LearningOutcome> SupportedOutcomes { get; }

        void SupportOutcome(LearningOutcome outcome);
        void UnsupportOutcome(LearningOutcome outcome);
	}
}