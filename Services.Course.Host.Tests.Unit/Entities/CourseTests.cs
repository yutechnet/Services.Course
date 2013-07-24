using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Courses;
using BpeProducts.Services.Course.Domain.Entities;
using EventStore;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Entities
{
    public class CourseTests
    {
        [Test]
        public void Can_add_top_level_segments()
        {
            var segmentCount = 5;
            var course = new Domain.Courses.Course
                {
                    OrganizationId = Guid.NewGuid(),
                    TenantId = 999999
                };

            var segmentId = Guid.NewGuid();
            for (int i = 0; i < segmentCount; i++)
            {
                var request = new SaveCourseSegmentRequest
                {
                    Name = "Week" + i,
                    Description = "Week " + i,
                    Type = "Weekly"
                };

                course.AddSegment(segmentId, Guid.Empty, request);

                var segment = course.Segments.First(s => s.Name == request.Name);
                Assert.That(segment.Id, Is.EqualTo(segmentId));
                Assert.That(segment.Course, Is.EqualTo(course));
                Assert.That(segment.ParentSegment, Is.Null);

                Assert.That(segment.Name, Is.EqualTo(request.Name));
                Assert.That(segment.ParentSegment, Is.Null);
                Assert.That(segment.Description, Is.EqualTo(request.Description));
                Assert.That(segment.TenantId, Is.EqualTo(course.TenantId));
                Assert.That(segment.Type, Is.EqualTo(request.Type));
                Assert.That(segment.ChildSegments, Is.Empty);
            }
            
            Assert.That(course.Segments.Count == segmentCount);
        }

        [Test]
        public void Can_add_multiple_sub_segments()
        {
            var course = new Domain.Courses.Course
            {
                OrganizationId = Guid.NewGuid(),
                TenantId = 999999
            };

            var request = new SaveCourseSegmentRequest
            {
                Name = "Week 1",
                Description = "Week 1",
                Type = "Weekly"
            };
            var segmentId = Guid.NewGuid();
            course.AddSegment(segmentId, Guid.Empty, request);

            var parentSegmentId = segmentId;
            var segmentCount = 5;
            for (int i = 0; i < segmentCount; i++)
            {
                segmentId = Guid.NewGuid();
                request = new SaveCourseSegmentRequest
                {
                    Name = "Assigment" + i,
                    Description = "Assigment " + i,
                    Type = "Assignment"
                };

                course.AddSegment(segmentId, parentSegmentId, request);

                var segment = course.Segments.First().ChildSegments.First(s => s.Name == request.Name);
                Assert.That(segment.Id, Is.EqualTo(segmentId));
                Assert.That(segment.Course, Is.EqualTo(course));
                Assert.That(segment.ParentSegment, Is.Not.Null);
                Assert.That(segment.ParentSegment, Is.EqualTo(course.Segments.First()));

                Assert.That(segment.Name, Is.EqualTo(request.Name));
                Assert.That(segment.Description, Is.EqualTo(request.Description));
                Assert.That(segment.TenantId, Is.EqualTo(course.TenantId));
                Assert.That(segment.Type, Is.EqualTo(request.Type));
                Assert.That(segment.ChildSegments, Is.Empty);
            }

            Assert.That(course.Segments.First().ChildSegments.Count, Is.EqualTo(segmentCount));
            
        }

        [Test]
        public void Can_add_nested_course_segments()
        {
            var segmentCount = 5;

             var course = new Domain.Courses.Course
            {
                OrganizationId = Guid.NewGuid(),
                TenantId = 999999
            };

            Domain.Courses.CourseSegment lastSegment = null;
            for (int i = 0; i < segmentCount; i++)
            {
                var request = new SaveCourseSegmentRequest
                    {
                        Name = "Segment",
                        Description = "Segment",
                        Type = "Assignment"
                    };

                var newSegment = course.AddSegment(Guid.NewGuid(), lastSegment == null ? Guid.Empty : lastSegment.Id, request);

                Assert.That(course.Segments.Contains(newSegment));
                Assert.That(newSegment.ParentSegment, Is.EqualTo(lastSegment));

                if (lastSegment != null)
                {
                    Assert.That(lastSegment.ChildSegments.Contains(newSegment));
                }
                
                lastSegment = newSegment;
            }
        }

        [Test]
        public void Can_add_course_prerequisites()
        {
            AutoMock autoMock = AutoMock.GetLoose();
            var courseFactory = autoMock.Create<CourseFactory>();

            var course = courseFactory.Create(new SaveCourseRequest());

            Assert.That(course.Prerequisites, Is.Empty);

            var prerequisiste = courseFactory.Create(new SaveCourseRequest());
            course.SetPrerequisites(new List<Domain.Courses.Course> { prerequisiste });

            Assert.That(course.Prerequisites.Count, Is.EqualTo(1));
            Assert.That(course.Prerequisites.First(), Is.EqualTo(prerequisiste));
        }

        [Test]
        public void Cannot_modify_published_course()
        {
            AutoMock autoMock = AutoMock.GetLoose();
            var courseFactory = autoMock.Create<CourseFactory>();
            var course = courseFactory.Create(new SaveCourseRequest());

            Assert.DoesNotThrow(() => course.Name = "name" );
            Assert.DoesNotThrow(() => course.Description = "description");
            Assert.DoesNotThrow(() => course.CourseType = ECourseType.Competency);
            Assert.DoesNotThrow(() => course.Code = "code");
            Assert.DoesNotThrow(() => course.SetPrograms(new List<Program>()));
            Assert.DoesNotThrow(() => course.SetPrerequisites(new List<Domain.Courses.Course>()));
            Assert.DoesNotThrow(() => course.AddSegment(Guid.NewGuid(), Guid.Empty, new SaveCourseSegmentRequest()));
            
            course.Publish("note");

            Assert.Throws<ForbiddenException>(() => course.Name = "name");
            Assert.Throws<ForbiddenException>(() => course.Description = "description");
            Assert.Throws<ForbiddenException>(() => course.CourseType = ECourseType.Competency);
            Assert.Throws<ForbiddenException>(() => course.Code = "code");
            Assert.Throws<ForbiddenException>(() => course.SetPrograms(new List<Program>()));
            Assert.Throws<ForbiddenException>(() => course.SetPrerequisites(new List<Domain.Courses.Course>()));
            Assert.Throws<ForbiddenException>(() => course.AddSegment(Guid.NewGuid(), Guid.Empty, new SaveCourseSegmentRequest()));
        }
    }
}
