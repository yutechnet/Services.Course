using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Courses;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Entities
{
    [TestFixture]
    public class CourseFactoryTests
    {
        private AutoMock _autoMock;
        private CourseFactory _courseFactory;
        private Mock<IRepository> _mockRepository;

        [SetUp]
        public void SetUp()
        {
            _autoMock = AutoMock.GetLoose();
            _mockRepository = _autoMock.Mock<IRepository>();
            _courseFactory = _autoMock.Create<CourseFactory>();
        }

        [Test]
        public void Can_create_course_from_scratch()
        {
            var request = new SaveCourseRequest
                {
                    Code = "ABC123",
                    CourseType = ECourseType.Competency,
                    Credit = 1,
                    Description = "some course",
                    Name = "ABC 123"
                };

            var course = _courseFactory.Create(request);

            Assert.That(course, Is.Not.Null);
            Assert.That(course.Code, Is.EqualTo(request.Code));
            Assert.That(course.CourseType, Is.EqualTo(request.CourseType));
            Assert.That(course.Credit, Is.EqualTo(request.Credit));
            Assert.That(course.Description, Is.EqualTo(request.Description));
            Assert.That(course.Name, Is.EqualTo(request.Name));
        }
        
        [Test]
        public void Can_create_course_from_template()
        {
            var template = new Domain.Courses.Course
                {
                    Code = "123ABC",
                    Description = "template description",
                    Name = "Template Course",
                    CourseType = ECourseType.Competency,
                    Credit = 1
                };
            var learningMaterial = new LearningMaterial
            {
                Id = Guid.NewGuid(),
                AssetId = Guid.NewGuid(),
                Instruction = "test lm",
                IsRequired = false,
                ActiveFlag = true,
                Course = template
            };
            template.LearningMaterials.Add(learningMaterial);
            _mockRepository.Setup(r => r.Get<Domain.Courses.Course>(It.IsAny<Guid>())).Returns(template);

            var request = new CreateCourseFromTemplateRequest
                {
                    Code = "ABC123",
                    Description = "some description",
                    Name = "ABC 123",
                    OrganizationId = Guid.NewGuid(),
                    TemplateCourseId = Guid.NewGuid(),
                    TenantId = 1,
                    IsTemplate = true
                };


            var course = _courseFactory.Create(request);

            Assert.That(course, Is.Not.Null);
            Assert.That(course.Code, Is.EqualTo(request.Code));
            Assert.That(course.CourseType, Is.EqualTo(template.CourseType));
            Assert.That(course.Credit, Is.EqualTo(template.Credit));
            Assert.That(course.Description, Is.EqualTo(request.Description));
            Assert.That(course.Name, Is.EqualTo(request.Name));
            Assert.That(course.IsTemplate, Is.EqualTo(request.IsTemplate));

            var sectionLearningMaterial = course.LearningMaterials.FirstOrDefault();
            Assert.That(sectionLearningMaterial.CustomAttribute, Is.EqualTo(learningMaterial.CustomAttribute));
            Assert.That(sectionLearningMaterial.Instruction, Is.EqualTo(learningMaterial.Instruction));
            Assert.That(sectionLearningMaterial.IsRequired, Is.EqualTo(learningMaterial.IsRequired));
            Assert.That(sectionLearningMaterial.AssetId, Is.EqualTo(learningMaterial.AssetId));
        }

        [Test]
        public void Can_take_values_from_template_when_request_does_not_includ_them()
        {
            var template = new Domain.Courses.Course
            {
                Code = "123ABC",
                Description = "template description",
                Name = "Template Course",
                CourseType = ECourseType.Competency,
                Credit = 1
            };
            _mockRepository.Setup(r => r.Get<Domain.Courses.Course>(It.IsAny<Guid>())).Returns(template);

            var request = new CreateCourseFromTemplateRequest
            {
                OrganizationId = Guid.NewGuid(),
                TemplateCourseId = Guid.NewGuid(),
                TenantId = 1,
            };


            var course = _courseFactory.Create(request);

            Assert.That(course, Is.Not.Null);
            Assert.That(course.Code, Is.EqualTo(template.Code));
            Assert.That(course.CourseType, Is.EqualTo(template.CourseType));
            Assert.That(course.Credit, Is.EqualTo(template.Credit));
            Assert.That(course.Description, Is.EqualTo(template.Description));
            Assert.That(course.Name, Is.EqualTo(template.Name));
            Assert.That(course.IsTemplate, Is.False);

        }

        [Test]
        public void Throws_exception_when_template_course_does_not_exist()
        {
            _mockRepository.Setup(r => r.Get<Domain.Courses.Course>(It.IsAny<Guid>())).Throws(new BadRequestException("Course does not exist"));

            var request = new CreateCourseFromTemplateRequest
            {
                Code = "ABC123",
                Description = "some description",
                Name = "ABC 123",
                OrganizationId = Guid.NewGuid(),
                TemplateCourseId = Guid.NewGuid(),
                TenantId = 1,
                IsTemplate = true
            };

            Assert.Throws<BadRequestException>(() => _courseFactory.Create(request));
        }
    }
}
