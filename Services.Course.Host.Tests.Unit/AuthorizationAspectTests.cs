﻿using System;
using System.Net;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using BpeProducts.Common.Capabilities;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate;
using BpeProducts.Common.NHibernate.Audit;
using BpeProducts.Services.Acl.Client;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Repositories;
using Moq;
using NHibernate;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit
{
	[TestFixture]
	public class AuthorizationAspectTests
	{
		[SetUp]
		public void Setup()
		{
			_repositoryMock = new Mock<IRepository>();
			_aclClientMock = new Mock<IAclHttpClient>();
			_auditDataProviderMock = new Mock<IAuditDataProvider>(); 
			_tokenExtractor = new Mock<ITokenExtractor>();

			var builder = new ContainerBuilder();
			builder.RegisterType<TestService>().As<ITestService>()
			       .EnableInterfaceInterceptors().InterceptedBy(typeof (AuthorizeAclAspect));
			builder.RegisterType<AuthorizeAclAspect>();
			builder.Register(x => _repositoryMock.Object);
			builder.Register(x => _aclClientMock.Object);
			builder.Register(x => _auditDataProviderMock.Object);
			builder.Register(x => _tokenExtractor.Object);
			_container = builder.Build();

		}

		private IContainer _container;
		private Mock<IRepository> _repositoryMock;
		private Mock<IAclHttpClient> _aclClientMock;
		private Mock<IAuditDataProvider> _auditDataProviderMock;
		private Mock<ITokenExtractor> _tokenExtractor;

		public void Should_intercept_method()
		{
			SetupMocksAuthorize(true);

			Guid id = Guid.NewGuid();
			var svc = _container.Resolve<ITestService>();
			Assert.That(svc.DoWork(id), Is.EqualTo(id));
		}

		[Test]
		public void Should_intercept_method_and_throw_if_access_is_denied()
		{
			SetupMocksAuthorize(false);

			var svc = _container.Resolve<ITestService>();
			Assert.Throws<AuthorizationException>(() => svc.DoWork(Guid.Empty));
		}

		[Test]
		public void Should_throw_exception_if_attribute_params_not_set()
		{
			SetupMocksAuthorize(true);

			var svc = _container.Resolve<ITestService>();
			Assert.Throws<AuthorizationException>(() => svc.DoWorkNoAttributeParams(Guid.Empty));
		}

		[Test]
		public void Should_throw_exception_if_no_object_id()
		{
			SetupMocksAuthorize(true);

			var svc = _container.Resolve<ITestService>();
			Assert.Throws<AuthorizationException>(() => svc.DoWorkNoObjectIdInAttribute(Guid.Empty));
		}

		[Test]
		public void Should_throw_exception_if_missing_method_params()
		{
			SetupMocksAuthorize(true);

			var svc = _container.Resolve<ITestService>();
			Assert.Throws<AuthorizationException>(() => svc.DoWorkMissingMethodParams());
		}

		[Test]
		public void Should_throw_exception_if_no_capability()
		{
			SetupMocksAuthorize(true);

			var svc = _container.Resolve<ITestService>();
			Assert.Throws<AuthorizationException>(() => svc.DoWorkNoCapability(Guid.Empty));
		}

		[Test]
		public void Should_throw_exception_if_no_object_type()
		{
			SetupMocksAuthorize(true);

			var svc = _container.Resolve<ITestService>();
			Assert.Throws<AuthorizationException>(() => svc.DoWorkNoObjectType(Guid.Empty));
		}

		[Test]
		public void Should_get_org_id_from_org_object_when_specified()
		{
			SetupMocksAuthorize(true);
			var orgObject = new OrgObject{OrganizationId = Guid.NewGuid()};
			var svc = _container.Resolve<ITestService>();

			svc.DoWorkOrgObject(orgObject);

			_aclClientMock.Verify(m=>m.HasAccess(It.IsAny<string>(),It.IsAny<Guid>(),It.Is<Guid>(x=>x==orgObject.OrganizationId),It.IsAny<Guid>(),It.IsAny<Capability>()));
			_repositoryMock.Verify(r=>r.Get(It.IsAny<string>(),It.IsAny<object>()),Times.Never());
		}
		private void SetupMocksAuthorize(bool authorizeUser)
		{
			_repositoryMock.Setup(q => q.Get(It.IsAny<String>(), It.IsAny<Guid>())).Returns(new Course.Domain.Courses.Course() { Id = new Guid() });
			_aclClientMock.Setup(q => q.HasAccess(It.IsAny<string>(),It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Capability>())).Returns(authorizeUser);
			_auditDataProviderMock.Setup(s => s.GetUserGuid()).Returns(Guid.NewGuid);
		}
	}

	public class OrgObject:OrganizationEntity
	{
	}

	

	public interface ITestService
	{
		Guid DoWork(Guid blipId);

		Guid DoWorkNoAttributeParams(Guid blipId);

		Guid DoWorkNoObjectIdInAttribute(Guid blipId);

		Guid DoWorkMissingMethodParams();

		Guid DoWorkNoCapability(Guid blipId);

		Guid DoWorkNoObjectType(Guid blipId);

		Guid DoWorkOrgObject(OrgObject orgObject);
	}

	public class TestService : ITestService
	{
		[AuthByAcl(ObjectIdArgument = "blipId", Capability = Capability.CourseCreate, ObjectType = typeof(Course.Domain.Courses.Course))]
		public Guid DoWork(Guid blipId)
		{
			return blipId;
		}

		[AuthByAcl]
		public Guid DoWorkNoAttributeParams(Guid blipId)
		{
			return blipId;
		}

		[AuthByAcl(Capability = Capability.CourseCreate, ObjectType = typeof(Course.Domain.Courses.Course))]
		public Guid DoWorkNoObjectIdInAttribute(Guid blipId)
		{
			return blipId;
		}

		[AuthByAcl(ObjectIdArgument = "blipId", Capability = Capability.CourseCreate, ObjectType = typeof(Course.Domain.Courses.Course))]
		public Guid DoWorkMissingMethodParams()
		{
			return new Guid();
		}

		[AuthByAcl(ObjectIdArgument = "blipId", ObjectType = typeof(Course.Domain.Courses.Course))]
		public Guid DoWorkNoCapability(Guid blipId)
		{
			return blipId;
		}

		[AuthByAcl(ObjectIdArgument = "blipId", Capability = Capability.CourseCreate)]
		public Guid DoWorkNoObjectType(Guid blipId)
		{
			return blipId;
		}

		[AuthByAcl(OrganizationObject = "orgObject", Capability = Capability.CourseCreate)]
		public Guid DoWorkOrgObject(OrgObject orgObject)
		{
			return orgObject.OrganizationId;
		}
	}

	
}