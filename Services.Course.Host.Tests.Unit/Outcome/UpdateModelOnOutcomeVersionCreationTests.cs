using System;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Outcomes;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Outcome
{
	[TestFixture]
	public class UpdateModelOnOutcomeVersionCreationTests
	{
		[SetUp]
		public void SetUp()
		{
			_mockRepository = new Mock<IRepository>();

			_handler = new UpdateModelOnOutcomeVersionCreation(_mockRepository.Object);
		}

		private UpdateModelOnOutcomeVersionCreation _handler;
		private Mock<IRepository> _mockRepository;

		[Test]
		public void Can_check_for_correct_domain_event()
		{
			var @event = new OutcomeDeleted();
			Assert.Throws<InvalidOperationException>(() => _handler.Handle(@event));
		}

		[Test]
		public void Can_update_model_on_outcome_version_created()
		{
			var @event = new OutcomeVersionCreated
				{
					AggregateId = Guid.NewGuid(),
					NewVersion = new LearningOutcome()
				};

			_handler.Handle(@event);

			_mockRepository.Verify(x => x.Save(@event.NewVersion));
		}
	}
}