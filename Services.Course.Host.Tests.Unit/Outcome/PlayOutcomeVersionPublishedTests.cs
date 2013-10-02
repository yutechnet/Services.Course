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
    public class PlayOutcomeVersionPublishedTests
    {
        private PlayOutcomeVersionPublished _handler;

        [SetUp]
        public void SetUp()
        {
            _handler = new PlayOutcomeVersionPublished();
        }

        [Test]
        public void Can_play_outcome_version_published_event()
        {
            var msg = new OutcomeVersionPublished
                {
                    AggregateId = Guid.NewGuid(),
                    PublishNote = "hello world"
                };

            var learningOutcome = _handler.Apply(msg, new LearningOutcome());

            Assert.That(learningOutcome.PublishNote, Is.EqualTo(msg.PublishNote));
        }
    }
}
