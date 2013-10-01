using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Outcomes;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Outcome
{
    [TestFixture]
    public class PlayOutcomeCreatedTests
    {
        private PlayOutcomeCreated _handler;

        [SetUp]
        public void SetUp()
        {
            _handler = new PlayOutcomeCreated();    
        }

        [Test]
        public void Can_apply_outcome_creatd_event()
        {
            var learningOutcome = new LearningOutcome
                {
                    Id = Guid.NewGuid(),
                    Description = "hello",
                    ActiveFlag = false
                };
            var msg = new OutcomeCreated
                {
                    ActiveFlag = true,
                    AggregateId = Guid.NewGuid(),
                    Description = "hello world",
                    Outcome = learningOutcome
                };

            var outcome = _handler.Apply(msg, learningOutcome);

            Assert.That(outcome.Id, Is.EqualTo(msg.AggregateId));
            Assert.That(outcome.Description, Is.EqualTo(msg.Description));
            Assert.That(outcome.ActiveFlag, Is.EqualTo(msg.ActiveFlag));
        }
    }
}
