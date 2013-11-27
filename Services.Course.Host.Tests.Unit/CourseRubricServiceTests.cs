using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Courses;
using BpeProducts.Services.Course.Domain.Repositories;
using Moq;
using NUnit.Framework;
using Services.Assessment.Contract;

namespace BpeProducts.Services.Course.Host.Tests.Unit
{
	[TestFixture]
	public class CourseRubricServiceTests
	{
		private CourseRubricService _courseRubricService;
		private Mock<ICourseRepository> _repoMock;
		private Mock<IAssessmentClient> _assessmentClientMock;

		[SetUp]
		public void SetUp()
		{
			Mapper.CreateMap<Domain.Courses.CourseRubric, CourseRubricInfo>();
			_repoMock = new Mock<ICourseRepository>();
			_assessmentClientMock = new Mock<IAssessmentClient>();
			_courseRubricService = new CourseRubricService(_repoMock.Object, _assessmentClientMock.Object);
		}

		[Test]
		public void Can_add_rubric()
		{
			var courseMock = new Mock<Domain.Courses.Course>();
			var courseRubricId = Guid.NewGuid();
			var rubricId = Guid.NewGuid();

			courseMock.Setup(
				c => c.AddCourseRubric(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CourseRubricRequest>()))
					  .Returns(new CourseRubric { Id = courseRubricId, RubricId = rubricId });
			_repoMock.Setup(r => r.GetOrThrow(It.IsAny<Guid>())).Returns(courseMock.Object);

			var rubricDto = new RubricInfoResponse();
			_assessmentClientMock.Setup(a => a.GetRubric(It.IsAny<Guid>())).Returns(rubricDto);

			var rubricAssocationDto = new CourseRubricRequest {RubricId = Guid.NewGuid()};
			var result = _courseRubricService.AddRubric(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), rubricAssocationDto);

			Assert.That(result.Id, Is.EqualTo(courseRubricId));
			Assert.That(result.RubricId, Is.EqualTo(rubricId));
		}

		[Test]
		public void Add_rubric_fails_if_rubric_does_not_exist()
		{
			_repoMock.Setup(r => r.GetOrThrow(It.IsAny<Guid>())).Returns(It.IsAny<Domain.Courses.Course>());
			_assessmentClientMock.Setup(a => a.GetRubric(It.IsAny<Guid>())).Throws(new NotFoundException("Rubric doesn't exist"));

			var rubricAssocationDto = new CourseRubricRequest { RubricId = Guid.NewGuid() };
			Assert.Throws(typeof(NotFoundException), () => _courseRubricService.AddRubric(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), rubricAssocationDto));
		}

		//[Test]
		//public void Add_rubric_fails_if_rubric_is_not_published()
		//{
		//	var courseMock = new Mock<Domain.Courses.Course>();
		//	var courseRubricId = Guid.NewGuid();
		//	var rubricId = Guid.NewGuid();

		//	courseMock.Setup(
		//		c => c.AddCourseRubric(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CourseRubricRequest>()))
		//			  .Returns(new CourseRubric { Id = courseRubricId, RubricId = rubricId });
		//	_repoMock.Setup(r => r.GetOrThrow(It.IsAny<Guid>())).Returns(courseMock.Object);

		//	//TODO: Waiting for story to complete that establishes versioning/publishing on rubics
		//	var rubricDto = new RubricInfoResponse {IsPublished = false};
		//	_assessmentClientMock.Setup(a => a.GetRubric(It.IsAny<Uri>(), It.IsAny<Guid>())).Returns(rubricDto);

		//	var rubricAssocationDto = new CourseRubricRequest { RubricId = Guid.NewGuid() };
		//	Assert.Throws(typeof(BadRequestException), () => _courseRubricService.AddRubric(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), rubricAssocationDto));
		//}

		[Test]
		public void Can_get_rubric()
		{
			var courseRubricId = Guid.NewGuid();
			var rubricId = Guid.NewGuid();

			_repoMock.Setup(r => r.GetCourseRubric(courseRubricId))
			         .Returns(new CourseRubric {RubricId = rubricId});

			var result = _courseRubricService.Get(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), courseRubricId);

			Assert.That(result.RubricId, Is.EqualTo(rubricId));
		}
	}
}
