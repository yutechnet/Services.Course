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
    public class PlayOutcomeVersionCreatedTests
    {
        private PlayOutcomeVersionCreated _handler;

        [SetUp]
        public void SetUp()
        {
            _handler = new PlayOutcomeVersionCreated();
        }

        [Test]
        public void Can_play_outcome_version_created_event()
        {
            var msg = new OutcomeVersionCreated
                {
                    AggregateId = Guid.NewGuid(),
                    NewVersion = new LearningOutcome
                        {
                            Id = Guid.NewGuid(),
                            Description = "hello world"
                        }
                };

            var learningOutcome = _handler.Apply(msg, new LearningOutcome());

            Assert.That(learningOutcome, Is.EqualTo(msg.NewVersion));
        }
    }
}
