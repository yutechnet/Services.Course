using System;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;

namespace BpeProducts.Services.Course.Domain.Outcomes
{
	public class OutcomeFactory : VersionFactory<LearningOutcome>, IOutcomeFactory
	{
		private readonly IRepository _learningOutcomeRepository;

		public OutcomeFactory(IRepository learningOutcomeRepository)
			: base(learningOutcomeRepository)
		{
			_learningOutcomeRepository = learningOutcomeRepository;
		}

		public LearningOutcome Build(OutcomeRequest request)
		{
			Guid outcomeId = Guid.NewGuid();
			var outcome = new LearningOutcome
				{
					Id = outcomeId,
					ActiveFlag = true,
					Description = request.Description,
					TenantId = request.TenantId
				};

			return outcome;
		}
	}
}