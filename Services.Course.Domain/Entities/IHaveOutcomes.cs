using System;
using System.Collections.Generic;

namespace BpeProducts.Services.Course.Domain.Entities
{
	public interface IHaveOutcomes
	{
		IList<LearningOutcome> Outcomes { get; set; }
		Guid Id { get; set; }
	}
}