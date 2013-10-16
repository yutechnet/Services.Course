using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate;
using BpeProducts.Common.NHibernate.Version;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Courses;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Repositories;
using Moq;
using NUnit.Framework;
using Newtonsoft.Json;

namespace BpeProducts.Services.Course.Host.Tests.Unit
{
	[TestFixture]
	public class VersionableEntityFactoryTests
	{
		private Mock<IRepository> _mockRepository;
		private VersionableEntityFactory _factory;

		[SetUp]
		public void SetUp()
		{
			_mockRepository = new Mock<IRepository>();
			_factory = new VersionableEntityFactory(_mockRepository.Object);
		}

		[Test]
		public void Can_locate_type_from_entity_assembly()
		{
			_mockRepository.Setup(r => r.Get(It.IsAny<Type>(), It.IsAny<Guid>())).Returns(new LearningOutcome());

			var entityType = "course";
			var versionableEntity = _factory.Get(entityType, Guid.NewGuid());

			Assert.That(versionableEntity, Is.Not.Null);

			entityType = "learningoutcome";
			versionableEntity = _factory.Get(entityType, Guid.NewGuid());

			Assert.That(versionableEntity, Is.Not.Null);
		}

		[Test]
		public void Throws_exception_when_type_not_found()
		{
			var entityType = "nonExistingType";

			Assert.Throws<BadRequestException>(() => _factory.Get(entityType, Guid.NewGuid()));
		}

		[Test]
		public void Throws_exception_when_type_not_versionable_entity()
		{
			var entityType = "Program";

			Assert.Throws<BadRequestException>(() => _factory.Get(entityType, Guid.NewGuid()));
		}

		[Test]
		public void Can_serialize_protected_setters_of_an_entity()
		{
			var v = new TestEntity();
			
			v.Publish();
			var serialized = v.DeepClone();
			Assert.That(serialized.IsPublished);
		}

	}

	public class TestEntity:VersionableEntity
	{
		public void Publish()
		{
			this.ParentEntity = null;
			this.OriginalEntity = null;
			this.IsPublished = true;
		}
	
		protected override VersionableEntity Clone()
		{
			return this;
			//throw new NotImplementedException();
		}

	}
}
