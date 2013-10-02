using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Outcomes;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Outcome
{
    [TestFixture]
    public class UpdateModelOnOutcomeCreationTests
    {
        private UpdateModelOnOutcomeCreation _handler;
        private Mock<IRepository> _mockRepository;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<IRepository>();
            _handler = new UpdateModelOnOutcomeCreation(_mockRepository.Object);            
        }

        [Test]
        public void Can_update_model_upon_outcome_created()
        {
            var @event = new OutcomeCreated
                {
                    AggregateId = Guid.NewGuid(),
                    Description = "hello world",
                    ActiveFlag = true,
                    Outcome = new LearningOutcome()
                };


            _handler.Handle(@event);

            _mockRepository.Verify(x => x.Save(@event.Outcome));
        }

        [Test]
        public void Can_check_for_correct_domain_event()
        {
            var @event = new OutcomeDeleted();
            Assert.Throws<InvalidOperationException>(() => _handler.Handle(@event));
        }
    }
}
