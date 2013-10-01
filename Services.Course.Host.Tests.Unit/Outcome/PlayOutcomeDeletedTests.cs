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
    public class PlayOutcomeDeletedTests
    {
        private PlayOutcomeDeleted _handler;

        [SetUp]
        public void SetUp()
        {
            _handler = new PlayOutcomeDeleted();
        }

        [Test]
        public void Can_play_outcome_deleted_event()
        {
            var msg = new OutcomeDeleted {AggregateId = Guid.NewGuid()};
            var learningOutcome = new LearningOutcome
                {
                    ActiveFlag = true
                };

            var outcome = _handler.Apply(msg, learningOutcome);

            Assert.That(outcome.ActiveFlag, Is.False);
        }
    }
}
