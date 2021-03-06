﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate;
using BpeProducts.Common.NHibernate.Version;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.CourseAggregates;
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
