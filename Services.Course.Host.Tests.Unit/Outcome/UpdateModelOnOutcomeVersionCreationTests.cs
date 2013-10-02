using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Outcomes;
using BpeProducts.Services.Course.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Outcome
{
    [TestFixture]
    public class UpdateModelOnOutcomeVersionCreationTests
    {
        private UpdateModelOnOutcomeVersionCreation _handler;
        private Mock<IRepository> _mockRepository;
        private Mock<IOutcomeFactory> _mockOutcomeFactory;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<IRepository>();
            _mockOutcomeFactory = new Mock<IOutcomeFactory>();

            _handler = new UpdateModelOnOutcomeVersionCreation(_mockRepository.Object, _mockOutcomeFactory.Object);
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

        [Test]
        public void Can_check_for_correct_domain_event()
        {
            var @event = new OutcomeDeleted();
            Assert.Throws<InvalidOperationException>(() => _handler.Handle(@event));
        }
    }
}
