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

                var newSegment = course.AddSegment(Guid.NewGuid(), lastSegment == null ? Guid.Empty : lastSegment.Id,
                                                   request);

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
        public void Can_add_course_prerequisite()
        {
            AutoMock autoMock = AutoMock.GetLoose();
            var courseFactory = autoMock.Create<CourseFactory>();

            var course = courseFactory.Create(new SaveCourseRequest());

            Assert.That(course.Prerequisites, Is.Empty);

            var prerequisiste = courseFactory.Create(new SaveCourseRequest());
            prerequisiste.Publish("");
            course.AddPrerequisite(prerequisiste);

            Assert.That(course.Prerequisites.Count, Is.EqualTo(1));
            Assert.That(course.Prerequisites.First(), Is.EqualTo(prerequisiste));
        }

        [Test]
        public void Can_add_course_prerequisite_does_not_add_duplicate_prerequisites()
        {
            AutoMock autoMock = AutoMock.GetLoose();
            var courseFactory = autoMock.Create<CourseFactory>();

            var course = courseFactory.Create(new SaveCourseRequest());

            Assert.That(course.Prerequisites, Is.Empty);

            var prerequisiste = courseFactory.Create(new SaveCourseRequest());
            prerequisiste.Publish("");
            course.AddPrerequisite(prerequisiste);
            course.AddPrerequisite(prerequisiste);

            Assert.That(course.Prerequisites.Count, Is.EqualTo(1));
            Assert.That(course.Prerequisites.First(), Is.EqualTo(prerequisiste));
        }

        [Test]
        public void Can_add_course_prerequisite_throws_exception_if_prerequisite_is_not_published()
        {
            AutoMock autoMock = AutoMock.GetLoose();
            var courseFactory = autoMock.Create<CourseFactory>();

            var course = courseFactory.Create(new SaveCourseRequest());

            Assert.That(course.Prerequisites, Is.Empty);

            var prerequisiste = courseFactory.Create(new SaveCourseRequest());
            Assert.Throws<ForbiddenException>(() => course.AddPrerequisite(prerequisiste));
        }

        [Test]
        public void Can_remove_course_prerequisite()
        {
            AutoMock autoMock = AutoMock.GetLoose();
            var courseFactory = autoMock.Create<CourseFactory>();

            var course = courseFactory.Create(new SaveCourseRequest());

            Assert.That(course.Prerequisites, Is.Empty);

            var prerequisiste = courseFactory.Create(new SaveCourseRequest());
            prerequisiste.Id = Guid.NewGuid();
            prerequisiste.Publish("");
            course.AddPrerequisite(prerequisiste);

            Assert.That(course.Prerequisites.Count, Is.EqualTo(1));
            Assert.That(course.Prerequisites.First(), Is.EqualTo(prerequisiste));

            course.RemovePrerequisite(prerequisiste.Id);
            Assert.That(course.Prerequisites.Count, Is.EqualTo(0));
        }

        [Test]
        public void Can_remove_course_prerequisite_gracefully_ignores_requests_to_remove_prereq_that_isnt_there()
        {
            AutoMock autoMock = AutoMock.GetLoose();
            var courseFactory = autoMock.Create<CourseFactory>();

            var course = courseFactory.Create(new SaveCourseRequest());

            Assert.That(course.Prerequisites, Is.Empty);

            course.RemovePrerequisite(Guid.NewGuid());
            Assert.That(course.Prerequisites.Count, Is.EqualTo(0));
        }

        [Test]
        public void Cannot_modify_published_course()
        {
            AutoMock autoMock = AutoMock.GetLoose();
            var courseFactory = autoMock.Create<CourseFactory>();
            var course = courseFactory.Create(new SaveCourseRequest());
            var prerequisiteCourse = courseFactory.Create(new SaveCourseRequest());
            prerequisiteCourse.Publish("");

            Assert.DoesNotThrow(() => course.Name = "name");
            Assert.DoesNotThrow(() => course.Description = "description");
            Assert.DoesNotThrow(() => course.CourseType = ECourseType.Competency);
            Assert.DoesNotThrow(() => course.Code = "code");
            Assert.DoesNotThrow(() => course.SetPrograms(new List<Program>()));
            Assert.DoesNotThrow(() => course.AddPrerequisite(prerequisiteCourse));
            Assert.DoesNotThrow(() => course.RemovePrerequisite(Guid.NewGuid()));
            Assert.DoesNotThrow(() => course.AddSegment(Guid.NewGuid(), Guid.Empty, new SaveCourseSegmentRequest()));

            course.Publish("note");

            Assert.Throws<ForbiddenException>(() => course.Name = "name");
            Assert.Throws<ForbiddenException>(() => course.Description = "description");
            Assert.Throws<ForbiddenException>(() => course.CourseType = ECourseType.Competency);
            Assert.Throws<ForbiddenException>(() => course.Code = "code");
            Assert.Throws<ForbiddenException>(() => course.SetPrograms(new List<Program>()));
            Assert.Throws<ForbiddenException>(() => course.AddPrerequisite(prerequisiteCourse));
            Assert.Throws<ForbiddenException>(() => course.RemovePrerequisite(Guid.NewGuid()));
            Assert.Throws<ForbiddenException>(
                () => course.AddSegment(Guid.NewGuid(), Guid.Empty, new SaveCourseSegmentRequest()));
        }

        [Test]
        public void Can_add_learning_activity_to_segment()
        {
            var course = new Domain.Courses.Course
                {
                    OrganizationId = Guid.NewGuid(),
                    TenantId = 999999
                };

            var segmentId = Guid.NewGuid();
            var request = new SaveCourseSegmentRequest
                {
                    Name = "Week 1",
                    Description = "Week 1 Description",
                    Type = "Weekly"
                };

            course.AddSegment(segmentId, Guid.Empty, request);
            var segment = course.Segments.First(s => s.Name == request.Name);

            var learningActivityId = Guid.NewGuid();
            var learningActivityRequest = new SaveCourseLearningActivityRequest
                {
                    Name = "Discussion 1",
                    //Type = "Discussion",
                    Type = CourseLearningActivityType.Discussion,
                    IsGradeable = true,
                    IsExtraCredit = false,
                    MaxPoint = 100
                    
                };
            course.AddLearningActivity(segmentId, learningActivityRequest, learningActivityId);

            var learningActivity =
                segment.CourseLearningActivities.First(s => s.Name == learningActivityRequest.Name);

            Assert.That(learningActivity.Id, Is.EqualTo(learningActivityId));
            Assert.That(learningActivity.Name, Is.EqualTo(learningActivityRequest.Name));
            Assert.That(learningActivity.Type, Is.EqualTo(learningActivityRequest.Type));
            Assert.That(learningActivity.IsGradeable, Is.EqualTo(learningActivityRequest.IsGradeable));
            Assert.That(learningActivity.IsExtraCredit, Is.EqualTo(learningActivityRequest.IsExtraCredit));
            Assert.That(learningActivity.MaxPoint, Is.EqualTo(learningActivityRequest.MaxPoint));
            Assert.That(learningActivity.TenantId, Is.EqualTo(course.TenantId));
        }

        [Test]
        public void Can_get_learning_activity_from_course()
        {
            var course = new Domain.Courses.Course
            {
                OrganizationId = Guid.NewGuid(),
                TenantId = 999999
            };

            var segmentId = Guid.NewGuid();
            var request = new SaveCourseSegmentRequest
            {
                Name = "Week 1",
                Description = "Week 1 Description",
                Type = "Weekly"
            };

            course.AddSegment(segmentId, Guid.Empty, request);
            var segment = course.Segments.First(s => s.Name == request.Name);

            var learningActivityId = Guid.NewGuid();
            var learningActivityRequest = new SaveCourseLearningActivityRequest
            {
                Name = "Discussion 1",
                //Type = "Discussion",
                Type = CourseLearningActivityType.Discussion,
                IsGradeable = true,
                IsExtraCredit = false,
                MaxPoint = 100

            };
            course.AddLearningActivity(segmentId, learningActivityRequest, learningActivityId);

            var learningActivity =
                course.GetLearningActivity(segmentId, learningActivityId);

            Assert.That(learningActivity.Id, Is.EqualTo(learningActivityId));
            Assert.That(learningActivity.Name, Is.EqualTo(learningActivityRequest.Name));
            Assert.That(learningActivity.Type, Is.EqualTo(learningActivityRequest.Type));
            Assert.That(learningActivity.IsGradeable, Is.EqualTo(learningActivityRequest.IsGradeable));
            Assert.That(learningActivity.IsExtraCredit, Is.EqualTo(learningActivityRequest.IsExtraCredit));
            Assert.That(learningActivity.MaxPoint, Is.EqualTo(learningActivityRequest.MaxPoint));
            Assert.That(learningActivity.TenantId, Is.EqualTo(course.TenantId));
        }

        [Test]
        public void Can_check_for_missing_segment_or_learningActivity()
        {
            var course = new Domain.Courses.Course
            {
                OrganizationId = Guid.NewGuid(),
                TenantId = 999999
            };

            var segmentId = Guid.NewGuid();
            var request = new SaveCourseSegmentRequest
            {
                Name = "Week 1",
                Description = "Week 1 Description",
                Type = "Weekly"
            };

            course.AddSegment(segmentId, Guid.Empty, request);
            var segment = course.Segments.First(s => s.Name == request.Name);

            var learningActivityId = Guid.NewGuid();
            var learningActivityRequest = new SaveCourseLearningActivityRequest
            {
                Name = "Discussion 1",
                //Type = "Discussion",
                Type = CourseLearningActivityType.Discussion,
                IsGradeable = true,
                IsExtraCredit = false,
                MaxPoint = 100

            };
            course.AddLearningActivity(segmentId, learningActivityRequest, learningActivityId);

            Assert.Throws<NotFoundException>(() => course.GetLearningActivity(segmentId, Guid.NewGuid()));
            Assert.Throws<NotFoundException>(() => course.GetLearningActivity(Guid.NewGuid(), learningActivityId));

            course.DeleteLearningActivity(segmentId, learningActivityId);
            Assert.Throws<NotFoundException>(() => course.GetLearningActivity(segmentId, learningActivityId));
            

        }

        [Test]
        public void Can_remove_learningoutcome_from_supporting_outcome()
        {
            var course = new Domain.Courses.Course
            {
                OrganizationId = Guid.NewGuid(),
                TenantId = 999999
            };

            var learningOutcome1 = new LearningOutcome {Id = Guid.NewGuid()};
            course.SupportOutcome(learningOutcome1);
            var learningOutcome2 = new LearningOutcome { Id = Guid.NewGuid() };
            course.SupportOutcome(learningOutcome2);
            var learningOutcome3 = new LearningOutcome { Id = Guid.NewGuid() };
            course.SupportOutcome(learningOutcome3);

            Assert.That(course.SupportedOutcomes.Count, Is.EqualTo(3));

            course.UnsupportOutcome(learningOutcome1);

            Assert.That(course.SupportedOutcomes.Count, Is.EqualTo(2));
            Assert.That(course.SupportedOutcomes.FirstOrDefault(x => x.Id == learningOutcome1.Id), Is.Null);
        }

        [Test]
        public void Can_check_for_missing_parent_segment_during_adding_segment()
        {
            var course = new Domain.Courses.Course
            {
                OrganizationId = Guid.NewGuid(),
                TenantId = 999999
            };

            Assert.Throws<BadRequestException>(
                () => course.AddSegment(Guid.NewGuid(), Guid.NewGuid(), new SaveCourseSegmentRequest()));
        }

        [Test]
        public void Can_check_for_missing_segment_during_update()
        {
            var course = new Domain.Courses.Course
            {
                OrganizationId = Guid.NewGuid(),
                TenantId = 999999
            };

            Assert.Throws<NotFoundException>(() => course.UpdateSegment(Guid.NewGuid(), new SaveCourseSegmentRequest()));
            Assert.Throws<NotFoundException>(
                () => course.ReorderSegment(Guid.NewGuid(), new UpdateCourseSegmentRequest(), 1));

        }

    }
}
