using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.Indexed;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Courses;
using BpeProducts.Services.Course.Domain.Courses.Events;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using EventStore;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit
{
	

	 [TestFixture]
	public class CourseFactoryTests
	{

		 private ICourseFactory _factory;

		 [SetUp]
		 public void Setup()
		 {
			

			 var containerBuilder = new ContainerBuilder();
			
			 containerBuilder.Register(e => new Mock<IStoreEvents>().Object);
		     containerBuilder.Register(e => new Mock<ICourseRepository>().Object);

			 containerBuilder.RegisterType<CourseFactory>().As<ICourseFactory>();
				
			 containerBuilder.RegisterType<PlayCourseCreated>().Keyed<IPlayEvent>(typeof(CourseCreated).Name);
			 containerBuilder.RegisterType<PlayCourseAssociatedWithProgram>().Keyed<IPlayEvent>(typeof(CourseAssociatedWithProgram).Name);
			 containerBuilder.RegisterType<PlayCourseDeleted>().Keyed<IPlayEvent>(typeof(CourseDeleted).Name);
			 containerBuilder.RegisterType<PlayCourseDisassociatedWithProgram>().Keyed<IPlayEvent>(typeof(CourseDisassociatedWithProgram).Name);
			 containerBuilder.RegisterType<PlayCourseInfoUpated>().Keyed<IPlayEvent>(typeof(CourseInfoUpdated).Name);
			 containerBuilder.RegisterType<PlayCourseSegmentAdded>().Keyed<IPlayEvent>(typeof(CourseSegmentAdded).Name);
			 containerBuilder.RegisterType<PlayCourseSegmentUpdated>().Keyed<IPlayEvent>(typeof(CourseSegmentUpdated).Name);
			 
		
			 var container = containerBuilder.Build();
			 
			 _factory = container.Resolve<ICourseFactory>();
			
		 }
		 [Test]
		 public void Can_recreate_a_course()
		 {
		     var courseId = Guid.NewGuid();
			 var courseCreated = new CourseCreated
			 {
                 AggregateId = courseId,
                 Course = new Domain.Entities.Course { Id = courseId, TenantId = 1 },

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
