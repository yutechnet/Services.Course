using System;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Handlers
{
    [TestFixture]
    public class CourseEventPersisterHandlerTests
    {
        private Mock<IStoreCourseEvents> _mockCourseEventStore;
        private EventPersisterHandler _eventPersisterHandler;

        [SetUp]
        public void SetUp()
        {
            _mockCourseEventStore = new Mock<IStoreCourseEvents>();
            _eventPersisterHandler = new EventPersisterHandler(_mockCourseEventStore.Object);
        }

        [Test]
        public void Persist_event_to_event_store()
        {
            var domainEvent = new FakeDomainEvent();
            _eventPersisterHandler.Handle(domainEvent);

            _mockCourseEventStore.Verify(c => c.Store(domainEvent as IDomainEvent), Times.Once());
        }
    }

    public class FakeDomainEvent : IDomainEvent
    {
        public Guid AggregateId { get; set; }
    }
}
