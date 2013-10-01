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
    public class PlayOutcomeUpdatedTests
    {
        private PlayOutcomeUpdated _handler;

        [SetUp]
        public void SetUp()
        {
            _handler = new PlayOutcomeUpdated();    
        }

        [Test]
        public void Can_play_outcome_updated_event()
        {
            var msg = new OutcomeUpdated
                {
                    AggregateId = Guid.NewGuid(),
                    Description = "hello world"
                };
            var learningOutcome = new LearningOutcome
            {
                Description = "hello"
            };

            var outcome = _handler.Apply(msg, learningOutcome);

            Assert.That(outcome.Description, Is.EqualTo(msg.Description));
        }
    }
}
