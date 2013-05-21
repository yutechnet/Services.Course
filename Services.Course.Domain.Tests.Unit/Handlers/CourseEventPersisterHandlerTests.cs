using System;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Domain.Tests.Unit.Handlers
{
    [TestFixture]
    public class CourseEventPersisterHandlerTests
    {
        private Mock<IStoreCourseEvents> _mockCourseEventStore;
        private CourseEventPersisterHandler _courseEventPersisterHandler;

        [SetUp]
        public void SetUp()
        {
            _mockCourseEventStore = new Mock<IStoreCourseEvents>();
            _courseEventPersisterHandler = new CourseEventPersisterHandler(_mockCourseEventStore.Object);
        }

        [Test]
        public void Persist_event_to_event_store()
        {
            var domainEvent = new FakeDomainEvent();
            _courseEventPersisterHandler.Handle(domainEvent);

            _mockCourseEventStore.Verify(c => c.Store(domainEvent as IDomainEvent), Times.Once());
        }
    }

    public class FakeDomainEvent : IDomainEvent
    {
        public Guid AggregateId { get; set; }
    }
}
