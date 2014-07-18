using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Autofac.Extras.Moq;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.CourseAggregates;
using BpeProducts.Services.Course.Domain.ProgramAggregates;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit
{
    [TestFixture]
    public class CourseFactoryTests
    {
        [SetUp]
        public void Setup()
        {
            Mapper.CreateMap<CourseSegment, CourseSegment>();
            Mapper.CreateMap<LearningMaterialRequest, LearningMaterial>();
            Mapper.CreateMap<UpdateLearningMaterialRequest, LearningMaterial>();
        }

        [Test]
        public void Can_create_course_from_scratch()
        {
            AutoMock autoMock = AutoMock.GetLoose();

            var factory = autoMock.Create<TestCourseFactory>();
            var request = new SaveCourseRequest
                {
                    Name = "CourseName",
                    Code = "CourseCode",
                    Description = "CourseDescription",
                    OrganizationId = Guid.NewGuid(),
                    TenantId = 999999,
                    MetaData = "{someData}",
                    ExtensionAssets = new List<Guid>{Guid.NewGuid()}
                };

            Domain.CourseAggregates.Course course = factory.BuildFromScratch(request);
            Assert.That(course.Code, Is.EqualTo(request.Code));
            Assert.That(course.Name, Is.EqualTo(request.Name));
            Assert.That(course.Description, Is.EqualTo(request.Description));
            Assert.That(course.CourseType, Is.EqualTo(request.CourseType));
            Assert.That(course.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(course.IsTemplate, Is.EqualTo(request.IsTemplate));
            Assert.That(course.OrganizationId, Is.EqualTo(request.OrganizationId));
            Assert.That(course.TenantId, Is.EqualTo(request.TenantId));
            Assert.That(course.IsDeleted, Is.EqualTo(false));
            Assert.That(course.VersionNumber, Is.EqualTo(new Version(1, 0, 0, 0).ToString()));
            Assert.That(course.MetaData, Is.EqualTo(request.MetaData));
            CollectionAssert.AreEquivalent(request.ExtensionAssets, course.ExtensionAssets);
        }

        [Test]
        public void Can_create_course_from_template()
        {
            AutoMock autoMock = AutoMock.GetLoose();

            // Mock<IGraphValidator> graphValidator = autoMock.Mock<IGraphValidator>();
            var factory = autoMock.Create<TestCourseFactory>();

            var template = new Domain.CourseAggregates.Course
                {
                    Id = Guid.NewGuid(),
                    Code = "SomeCode",
                    Name = "SomeName",
                    Description = "SomeDescription",
                    CourseType = ECourseType.Competency,
                    OrganizationId = Guid.NewGuid(),
                    TenantId = 999999,
                    MetaData = "{templateData}",
                    ExtensionAssets = new List<Guid> { Guid.NewGuid() }
                };

            var learningMaterialRequest1 = new LearningMaterialRequest
            {
                AssetId = Guid.NewGuid(),
                Instruction = "instruction 1"
            };

            var learningMaterialRequest2 = new LearningMaterialRequest
            {
                AssetId = Guid.NewGuid(),
                Instruction = "instruction 2"
            };

            template.AddSegment(Guid.Empty, new SaveCourseSegmentRequest());

            var courseSegment2 = template.AddSegment(Guid.Empty, new SaveCourseSegmentRequest());
            courseSegment2.AddLearningMaterial(learningMaterialRequest1);
            courseSegment2.AddLearningMaterial(learningMaterialRequest2);

            template.AddSegment(courseSegment2.Id, new SaveCourseSegmentRequest());

            template.SetPrograms(new List<Program>
				{
					new Program {Id = Guid.NewGuid()},
					new Program {Id = Guid.NewGuid()}
				});

            var request = new CreateCourseFromTemplateRequest
                {
                    Name = "CourseName",
                    Code = "CourseCode",
                    Description = "CourseDescription",
                    OrganizationId = Guid.NewGuid(),
                };

            Domain.CourseAggregates.Course course = factory.BuildFromTemplate(template, request);

            var actualSegmentWithLearningMaterials = course.Segments.FirstOrDefault(s => s.LearningMaterials.Count > 0);
            if (actualSegmentWithLearningMaterials != null)
                Assert.That(courseSegment2.LearningMaterials.Count, Is.EqualTo(actualSegmentWithLearningMaterials.LearningMaterials.Count));
            else
            {
                throw new Exception("build course from template error!");
            }

            Assert.That(course.Code, Is.EqualTo(request.Code));
            Assert.That(course.Name, Is.EqualTo(request.Name));
            Assert.That(course.Description, Is.EqualTo(request.Description));
            Assert.That(course.CourseType, Is.EqualTo(template.CourseType));
            Assert.That(course.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(course.IsTemplate, Is.EqualTo(request.IsTemplate));
            Assert.That(course.OrganizationId, Is.EqualTo(request.OrganizationId));
            Assert.That(course.TenantId, Is.EqualTo(template.TenantId));
            Assert.That(course.IsDeleted, Is.EqualTo(false));
            Assert.That(course.VersionNumber, Is.EqualTo(new Version(1, 0, 0, 0).ToString()));
            Assert.That(course.MetaData, Is.EqualTo(template.MetaData));
            CollectionAssert.AreEquivalent(template.ExtensionAssets, course.ExtensionAssets);

            // TODO: Validate CourseSegments, Programs, LearningOutcomes being cloned
            Assert.That(course.Segments.Count, Is.EqualTo(template.Segments.Count));
            foreach (CourseSegment segment in course.Segments)
            {
                Assert.IsFalse(template.Segments.Contains(segment));
                Assert.That(segment.Id, Is.Not.EqualTo(Guid.Empty));
            }

            CourseSegment segmentsWithChild = course.Segments.First(s => s.ChildSegments.Count > 0);
            Assert.That(segmentsWithChild.ChildSegments.Count, Is.EqualTo(1));

            CourseSegment childSegment = segmentsWithChild.ChildSegments.First();
            Assert.That(childSegment.ParentSegment, Is.EqualTo(segmentsWithChild));
            Assert.That(course.Segments.Contains(childSegment));

            // Validating that the program associations are cloned
            Assert.That(course.Programs.Count, Is.EqualTo(template.Programs.Count));
            foreach (Program program in course.Programs)
            {
                Assert.That(template.Programs.Any(p => p.Id == program.Id));
            }
        }
    }

    internal class TestCourseFactory : CourseFactory
    {
        public TestCourseFactory(ICourseRepository courseRepository, IAssessmentClient assessmentClient) : base(courseRepository, assessmentClient)
        {
        }

        public new Domain.CourseAggregates.Course BuildFromTemplate(Domain.CourseAggregates.Course template, CreateCourseFromTemplateRequest request)
        {
            return base.BuildFromTemplate(template, request);
        }

        public new Domain.CourseAggregates.Course BuildFromScratch(SaveCourseRequest request)
        {
            return base.BuildFromScratch(request);
        }
    }
}