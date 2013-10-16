using System;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Outcomes;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Outcome
{
	[TestFixture]
	public class OutcomeFactoryTests
	{
		[SetUp]
		public void SetUp()
		{
			_mockLearningOutcomeRepository = new Mock<IRepository>();

			_outcomeFactory = new OutcomeFactory(_mockLearningOutcomeRepository.Object);
		}

		private OutcomeFactory _outcomeFactory;

		private Mock<IRepository> _mockLearningOutcomeRepository;

		[Test]
		public void Can_build_learing_outcome_from_request()
		{
			var request = new OutcomeRequest
				{
					Description = "hello world",
					TenantId = 1
				};


			LearningOutcome learningOutcome = _outcomeFactory.Build(request);

			Assert.That(learningOutcome.Description, Is.EqualTo(request.Description));
			Assert.That(learningOutcome.TenantId, Is.EqualTo(request.TenantId));
			Assert.That(learningOutcome.ActiveFlag, Is.True);
			Assert.That(learningOutcome.Id, Is.Not.EqualTo(Guid.Empty));
		}
	}
}