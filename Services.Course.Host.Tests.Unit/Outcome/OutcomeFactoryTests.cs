using System;
using Autofac.Features.Indexed;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Outcomes;
using EventStore;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Outcome
{
    [TestFixture]
    public class OutcomeFactoryTests
    {
        private OutcomeFactory _outcomeFactory;
        private Mock<IStoreEvents> _mockStoreEvents;
        private Mock<IIndex<string, IPlayEvent>> _mockIIndex;
        private Mock<Common.NHibernate.IRepository> _mockLearningOutcomeRepository;

        [SetUp]
        public void SetUp()
        {
            _mockStoreEvents = new Mock<IStoreEvents>();
            _mockIIndex = new Mock<IIndex<string, IPlayEvent>>();
            _mockLearningOutcomeRepository = new Mock<Common.NHibernate.IRepository>();

            _outcomeFactory = new OutcomeFactory(_mockStoreEvents.Object, _mockIIndex.Object, _mockLearningOutcomeRepository.Object);
        }

        [Test]
        public void Can_build_learing_outcome_from_request()
        {
            var request = new OutcomeRequest
                {
                    Description = "hello world",
                    TenantId = 1
                };


            var learningOutcome = _outcomeFactory.Build(request);

            Assert.That(learningOutcome.Description, Is.EqualTo(request.Description));
            Assert.That(learningOutcome.TenantId, Is.EqualTo(request.TenantId));
            Assert.That(learningOutcome.ActiveFlag, Is.True);
            Assert.That(learningOutcome.Id, Is.Not.EqualTo(Guid.Empty));
        }
    }
}
