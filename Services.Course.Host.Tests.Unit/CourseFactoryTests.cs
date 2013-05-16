using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Events;
using EventStore;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit
{
	 [TestFixture]
	public class CourseFactoryTests
	{
		 private CourseFactory _factory;

		 [SetUp]
		 public void Setup()
		 {
			 _factory = new CourseFactory(new Mock<IStoreEvents>().Object);
		 }
		 [Test]
		 public void Can_recreate_a_course()
		 {
			 var courseCreated = new CourseCreated
			 {
				 AggregateId = Guid.NewGuid(),
				 ActiveFlag = true,
				 Code = "x1",
				 Description = "mui blah",
				 Name = "blah"

			 };
			 var messages = new List<EventMessage>();
			 var msg = new EventMessage();
			 msg.Body = courseCreated;
			 messages.Add(msg);

			 var course = _factory.Reconstitute(messages);
			 Assert.That(course.Id,Is.EqualTo(courseCreated.AggregateId));
		 }
	}
}
