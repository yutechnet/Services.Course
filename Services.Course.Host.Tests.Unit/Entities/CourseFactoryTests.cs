using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.CourseAggregates;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Entities
{
    [TestFixture]
    public class CourseFactoryTests
    {
        private AutoMock _autoMock;
        private CourseFactory _courseFactory;
        private Mock<ICourseRepository> _mockRepository;

        [SetUp]
        public void SetUp()
        {
            _autoMock = AutoMock.GetLoose();
            _mockRepository = _autoMock.Mock<ICourseRepository>();
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
                    Name = "ABC 123",
                    MetaData = "{someData}",
                    ExtensionAssets = new List<Guid> { Guid.NewGuid() }
                };

            var course = _courseFactory.Build(request);

            Assert.That(course, Is.Not.Null);
            Assert.That(course.Code, Is.EqualTo(request.Code));
            Assert.That(course.CourseType, Is.EqualTo(request.CourseType));
            Assert.That(course.Credit, Is.EqualTo(request.Credit));
            Assert.That(course.Description, Is.EqualTo(request.Description));
            Assert.That(course.Name, Is.EqualTo(request.Name));
            Assert.That(course.MetaData, Is.EqualTo(request.MetaData));
            CollectionAssert.AreEquivalent(request.ExtensionAssets, course.ExtensionAssets);
        }
        
        [Test]
        public void Can_create_course_from_template()
        {
            var template = new Domain.CourseAggregates.Course
                {
                    Code = "123ABC",
                    Description = "template description",
                    Name = "Template Course",
                    CourseType = ECourseType.Competency,
                    Credit = 1,
                    MetaData = "{templateData}",
                    ExtensionAssets = new List<Guid> { Guid.NewGuid() },
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
            _mockRepository.Setup(r => r.GetOrThrow(It.IsAny<Guid>())).Returns(template);

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

            var course = _courseFactory.Build(request);

            Assert.That(course, Is.Not.Null);
            Assert.That(course.Code, Is.EqualTo(request.Code));
            Assert.That(course.CourseType, Is.EqualTo(template.CourseType));
            Assert.That(course.Credit, Is.EqualTo(template.Credit));
            Assert.That(course.Description, Is.EqualTo(request.Description));
            Assert.That(course.Name, Is.EqualTo(request.Name));
            Assert.That(course.IsTemplate, Is.EqualTo(request.IsTemplate));
            Assert.That(course.MetaData, Is.EqualTo(template.MetaData));
            CollectionAssert.AreEquivalent(template.ExtensionAssets, course.ExtensionAssets);

            var sectionLearningMaterial = course.LearningMaterials.First();
            Assert.That(sectionLearningMaterial.CustomAttribute, Is.EqualTo(learningMaterial.CustomAttribute));
            Assert.That(sectionLearningMaterial.Instruction, Is.EqualTo(learningMaterial.Instruction));
            Assert.That(sectionLearningMaterial.IsRequired, Is.EqualTo(learningMaterial.IsRequired));
            Assert.That(sectionLearningMaterial.AssetId, Is.EqualTo(learningMaterial.AssetId));
        }

        [Test]
        public void Can_take_values_from_template_when_request_does_not_includ_them()
        {
            var template = new Domain.CourseAggregates.Course
            {
                Code = "123ABC",
                Description = "template description",
                Name = "Template Course",
                CourseType = ECourseType.Competency,
                Credit = 1,
            };
            _mockRepository.Setup(r => r.GetOrThrow(It.IsAny<Guid>())).Returns(template);

            var request = new CreateCourseFromTemplateRequest
            {
                OrganizationId = Guid.NewGuid(),
                TemplateCourseId = Guid.NewGuid(),
                TenantId = 1,
            };


            var course = _courseFactory.Build(request);

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
            _mockRepository.Setup(r => r.GetOrThrow(It.IsAny<Guid>())).Throws(new BadRequestException("Course does not exist"));

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

            Assert.Throws<BadRequestException>(() => _courseFactory.Build(request));
        }
    }
}
