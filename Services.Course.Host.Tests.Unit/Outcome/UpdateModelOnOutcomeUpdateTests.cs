using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Outcomes;
using BpeProducts.Services.Course.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Outcome
{
    [TestFixture]
    public class UpdateModelOnOutcomeUpdateTests
    {
        private UpdateModelOnOutcomeUpdate _handler;
        private Mock<ILearningOutcomeRepository> _mockRepository;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<ILearningOutcomeRepository>();
            _handler = new UpdateModelOnOutcomeUpdate(_mockRepository.Object);
        }

        [Test]
        public void Can_update_model_upon_outcome_updated()
        {
            var outcome = new LearningOutcome();

            _mockRepository.Setup(x => x.Get(It.IsAny<Guid>())).Returns(outcome);

            var @event = new OutcomeUpdated
                {
                    AggregateId = Guid.NewGuid(),
                    Description = "hello world"
                };

            _handler.Handle(@event);

            _mockRepository.Verify(x => x.Save(It.Is<LearningOutcome>(y => y.Description == @event.Description)));
        }

        [Test]
        public void Can_check_for_correct_domain_event()
        {
            var @event = new OutcomeCreated();
            Assert.Throws<InvalidOperationException>(() => _handler.Handle(@event));
        }
    }
}
