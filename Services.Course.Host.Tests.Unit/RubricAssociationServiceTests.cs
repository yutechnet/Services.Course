using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Courses;
using BpeProducts.Services.Course.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit
{
	[TestFixture]
	public class RubricAssociationServiceTests
	{
		private RubricAssociationService _rubricAssociationService;
		private Mock<ICourseRepository> _repoMock;

		[SetUp]
		public void SetUp()
		{
			Mapper.CreateMap<Domain.Courses.RubricAssociation, RubricAssociationInfo>();
			_repoMock = new Mock<ICourseRepository>();
			_rubricAssociationService = new RubricAssociationService(_repoMock.Object);
		}

		[Test]
		public void Can_add_rubric()
		{
			var courseMock = new Mock<Domain.Courses.Course>();
			var rubricAssociationId = Guid.NewGuid();
			var rubricId = Guid.NewGuid();

			courseMock.Setup(
				c => c.AddRubricAssociation(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<RubricAssociationRequest>()))
					  .Returns(new RubricAssociation { Id = rubricAssociationId, RubricId = rubricId });
			_repoMock.Setup(r => r.GetOrThrow(It.IsAny<Guid>())).Returns(courseMock.Object);

			var rubricAssocationDto = new RubricAssociationRequest {RubricId = Guid.NewGuid()};
			var result = _rubricAssociationService.AddRubric(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), rubricAssocationDto);

			Assert.That(result.Id, Is.EqualTo(rubricAssociationId));
			Assert.That(result.RubricId, Is.EqualTo(rubricId));
		}

		[Test]
		public void Can_get_rubric()
		{
			var rubricAssociationId = Guid.NewGuid();
			var rubricId = Guid.NewGuid();

			_repoMock.Setup(r => r.GetRubricAssociation(rubricAssociationId))
			         .Returns(new RubricAssociation {RubricId = rubricId});

			var result = _rubricAssociationService.Get(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), rubricAssociationId);

			Assert.That(result.RubricId, Is.EqualTo(rubricId));
		}
	}
}
