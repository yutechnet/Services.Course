using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Autofac;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Courses;
using BpeProducts.Services.Course.Domain.Courses.Events;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using EventStore;
using Moq;
using NUnit.Framework;
using CourseSegment = BpeProducts.Services.Course.Domain.Courses.CourseSegment;

namespace BpeProducts.Services.Course.Host.Tests.Unit
{


    [TestFixture]
    public class CourseFactoryTests
    {

        private ICourseFactory _factory;
            
        [SetUp]
        public void Setup()
        {
            Mapper.CreateMap<Domain.Courses.CourseSegment, Domain.Courses.CourseSegment>();
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
                    TenantId = 999999
                };

            var course = factory.BuildFromScratch(request);
            Assert.That(course.Code, Is.EqualTo(request.Code));
            Assert.That(course.Name, Is.EqualTo(request.Name));
            Assert.That(course.Description, Is.EqualTo(request.Description));
            Assert.That(course.CourseType, Is.EqualTo(request.CourseType));
            Assert.That(course.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(course.IsTemplate, Is.EqualTo(request.IsTemplate));
            Assert.That(course.OrganizationId, Is.EqualTo(request.OrganizationId));
            Assert.That(course.TenantId, Is.EqualTo(request.TenantId));
            Assert.That(course.IsBuilt, Is.EqualTo(true));
            Assert.That(course.ActiveFlag, Is.EqualTo(true));
            Assert.That(course.VersionNumber, Is.EqualTo(new Version(1, 0, 0, 0).ToString()));
        }

        [Test]
        public void Can_create_course_from_template()
        {
            AutoMock autoMock = AutoMock.GetLoose();

            var factory = autoMock.Create<TestCourseFactory>();

            var segments = new List<CourseSegment>
                {
                    new CourseSegment { Id = Guid.NewGuid() },
                    new CourseSegment { Id = Guid.NewGuid() },
                };

            var newChildSegment = new CourseSegment {Id = Guid.NewGuid()};
            segments.First().AddSubSegment(newChildSegment);
            segments.Add(newChildSegment);

            var template = new Domain.Entities.Course
                {
                    Id = Guid.NewGuid(),
                    Code = "SomeCode",
                    Name = "SomeName",
                    Description = "SomeDescription",
                    CourseType = ECourseType.Competency,
                    IsTemplate = false,
                    OrganizationId = Guid.NewGuid(),
                    TenantId = 999999,
                    Segments = segments,
                    Programs = new List<Program>
                        {
                            new Program(),
                            new Program()
                        },
                    Outcomes = new List<LearningOutcome>
                        {
                            new LearningOutcome(),
                            new LearningOutcome
                                {
                                    Outcomes = new List<LearningOutcome>
                                        {
                                            new LearningOutcome()
                                        }
                                }
                        },
                };

            var request = new SaveCourseRequest
                {
                    Name = "CourseName",
                    Code = "CourseCode",
                    Description = "CourseDescription",
                    OrganizationId = Guid.NewGuid()
                };

            var course = factory.BuildFromTemplate(template, request);

            Assert.That(course.Code, Is.EqualTo(request.Code));
            Assert.That(course.Name, Is.EqualTo(request.Name));
            Assert.That(course.Description, Is.EqualTo(request.Description));
            Assert.That(course.CourseType, Is.EqualTo(template.CourseType));
            Assert.That(course.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(course.IsTemplate, Is.EqualTo(request.IsTemplate));
            Assert.That(course.OrganizationId, Is.EqualTo(request.OrganizationId));
            Assert.That(course.TenantId, Is.EqualTo(template.TenantId));
            Assert.That(course.IsBuilt, Is.EqualTo(true));
            Assert.That(course.ActiveFlag, Is.EqualTo(true));
            Assert.That(course.VersionNumber, Is.EqualTo(new Version(1, 0, 0, 0).ToString()));

            // TODO: Validate CourseSegments, Programs, LearningOutcomes being cloned
            Assert.That(course.Segments.Count, Is.EqualTo(template.Segments.Count));
            foreach (var segment in course.Segments)
            {
                Assert.IsFalse(template.Segments.Contains(segment));
                Assert.That(segment.Id, Is.Not.EqualTo(Guid.Empty));
            }

            var segmentsWithChild = course.Segments.First(s => s.ChildSegments.Count > 0);
            Assert.That(segmentsWithChild.ChildSegments.Count, Is.EqualTo(1));

            var childSegment = segmentsWithChild.ChildSegments.First();
            Assert.That(childSegment.ParentSegment, Is.EqualTo(segmentsWithChild));
            Assert.That(course.Segments.Contains(childSegment));

            // Validating that the program associations are cloned
            Assert.That(course.Programs.Count, Is.EqualTo(template.Programs.Count));
            foreach (var program in course.Programs)
            {
                Assert.That(template.Programs.Contains(program));
            }

            // Validating the learning outcomes are clones
            Assert.That(course.Outcomes.Count, Is.EqualTo(template.Outcomes.Count));
            foreach (var outcome in course.Outcomes)
            {
                Assert.That(template.Outcomes.Contains(outcome));
            }

            var outcomesWithChild = course.Outcomes.First(s => s.Outcomes.Count > 0);
            Assert.That(outcomesWithChild.Outcomes.Count, Is.EqualTo(1));
        }
    }

    class TestCourseFactory : CourseFactory
    {
        public TestCourseFactory(IStoreEvents store, IIndex<string, IPlayEvent> index, ICourseRepository courseRepository) : base(store, index, courseRepository)
        {
        }

        public new Domain.Entities.Course BuildFromTemplate(Domain.Entities.Course template, SaveCourseRequest request)
        {
            return base.BuildFromTemplate(template, request);
        }

        public new Domain.Entities.Course BuildFromScratch(SaveCourseRequest request)
        {
            return base.BuildFromScratch(request);
        }
    }
}
