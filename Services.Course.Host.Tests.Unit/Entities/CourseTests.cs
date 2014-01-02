using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Autofac.Extras.Moq;
using AutoMapper;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Courses;
using BpeProducts.Services.Course.Domain.Entities;
using Moq;
using NUnit.Framework;
using Services.Assessment.Contract;
using ServiceStack.Common.Extensions;
using OutcomeInfo = Services.Assessment.Contract.OutcomeInfo;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Entities
{
    public class CourseTests
    {
        private Mock<IAssessmentClient> _assessmentClientMock;
        [SetUp]
        public void SetUp()
        {
            _assessmentClientMock = new Mock<IAssessmentClient>();
            Mapper.CreateMap<LearningMaterialRequest, Domain.Courses.LearningMaterial>();
            _coursePublisher = new Mock<ICoursePublisher>();
        }
        [Test]
        public void Can_add_top_level_segments()
        {
            var segmentCount = 5;

            var course = GetCourse();

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
            var course = GetCourse();

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
            var course = GetCourse();

            CourseSegment lastSegment = null;
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
            prerequisiste.Publish("", _coursePublisher.Object);
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
            prerequisiste.Publish("", _coursePublisher.Object);
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
            prerequisiste.Publish("", _coursePublisher.Object);
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
            prerequisiteCourse.Publish("", _coursePublisher.Object);

            Assert.DoesNotThrow(() => course.Name = "name");
            Assert.DoesNotThrow(() => course.Description = "description");
            Assert.DoesNotThrow(() => course.CourseType = ECourseType.Competency);
            Assert.DoesNotThrow(() => course.Code = "code");
            Assert.DoesNotThrow(() => course.SetPrograms(new List<Program>()));
            Assert.DoesNotThrow(() => course.AddPrerequisite(prerequisiteCourse));
            Assert.DoesNotThrow(() => course.RemovePrerequisite(Guid.NewGuid()));
            Assert.DoesNotThrow(() => course.AddSegment(Guid.NewGuid(), Guid.Empty, new SaveCourseSegmentRequest()));

            course.Publish("note", _coursePublisher.Object);

            Assert.Throws<ForbiddenException>(() => course.Name = "name");
            Assert.Throws<ForbiddenException>(() => course.Description = "description");
            Assert.Throws<ForbiddenException>(() => course.CourseType = ECourseType.Competency);
            Assert.Throws<ForbiddenException>(() => course.Code = "code");
            Assert.Throws<ForbiddenException>(() => course.SetPrograms(new List<Program>()));
            Assert.Throws<ForbiddenException>(() => course.AddPrerequisite(prerequisiteCourse));
            Assert.Throws<ForbiddenException>(() => course.RemovePrerequisite(Guid.NewGuid()));
            Assert.Throws<ForbiddenException>(() => course.AddSegment(Guid.NewGuid(), Guid.Empty, new SaveCourseSegmentRequest()));
            Assert.Throws<ForbiddenException>(() => course.AddLearningMaterial(Guid.NewGuid(), new LearningMaterialRequest()));
            Assert.Throws<ForbiddenException>(() => course.DeleteLearningMaterial(Guid.NewGuid(), Guid.NewGuid()));
        }

        [Test]
        public void Can_add_learning_activity_to_segment()
        {
            var course = GetCourse();

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
            var course = GetCourse();

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
            var course = GetCourse();

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
            var course = GetCourse();

            var learningOutcome1 = new LearningOutcome { Id = Guid.NewGuid() };
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
            var course = GetCourse();

            Assert.Throws<BadRequestException>(
                () => course.AddSegment(Guid.NewGuid(), Guid.NewGuid(), new SaveCourseSegmentRequest()));
        }

        [Test]
        public void Can_check_for_missing_segment_during_update()
        {
            var course = GetCourse();

            Assert.Throws<NotFoundException>(() => course.UpdateSegment(Guid.NewGuid(), new SaveCourseSegmentRequest()));
            Assert.Throws<NotFoundException>(
                () => course.ReorderSegment(Guid.NewGuid(), new UpdateCourseSegmentRequest(), 1));

        }

        [Test]
        public void Can_build_section_request()
        {
            var sectionRequest = new CourseSectionRequest
                {
                    Name = "SectionName",
                    Code = "SectionCode",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(1),
                    OrganizationId = Guid.NewGuid(),
                };

            var course = GetCourse();
            course.Publish("It's published", _coursePublisher.Object);

            var request = course.GetSectionRequest(sectionRequest, _assessmentClientMock.Object);

            Assert.That(request.Name, Is.EqualTo(sectionRequest.Name));
            Assert.That(request.Code, Is.EqualTo(sectionRequest.Code));
            Assert.That(request.OrganizationId, Is.EqualTo(sectionRequest.OrganizationId));
            Assert.That(request.StartDate, Is.EqualTo(sectionRequest.StartDate));
            Assert.That(request.EndDate, Is.EqualTo(sectionRequest.EndDate));
            Assert.That(request.CourseId, Is.EqualTo(course.Id));
            Assert.That(request.TenantId, Is.EqualTo(course.TenantId));
        }

        [Test]
        public void Can_not_build_section_request_from_unpublished_course()
        {
            var course = GetCourse();

            Assert.Throws<BadRequestException>(() => course.GetSectionRequest(new CourseSectionRequest(), _assessmentClientMock.Object));
        }

        [Test]
        public void Can_build_section_request_with_subsections()
        {
            var sectionRequest = new CourseSectionRequest
            {
                Name = "SectionName",
                Code = "SectionCode",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1)
            };

            var course = GetCourse();
            var seg1Id = Guid.NewGuid();
            var seg11Id = Guid.NewGuid();
            var seg2Id = Guid.NewGuid();
            var seg21Id = Guid.NewGuid();
            var seg22Id = Guid.NewGuid();
            var seg221Id = Guid.NewGuid();

            var seg1 = course.AddSegment(seg1Id, new SaveCourseSegmentRequest { Name = "1" });
            var seg11 = course.AddSegment(seg11Id, seg1Id, new SaveCourseSegmentRequest { Name = "11" });
            var seg2 = course.AddSegment(seg2Id, new SaveCourseSegmentRequest { Name = "2" });
            var seg21 = course.AddSegment(seg21Id, seg2Id, new SaveCourseSegmentRequest { Name = "21" });
            var seg22 = course.AddSegment(seg22Id, seg2Id, new SaveCourseSegmentRequest { Name = "22" });
            var seg221 = course.AddSegment(seg221Id, seg22Id, new SaveCourseSegmentRequest { Name = "221" });

            course.Publish("It's published", _coursePublisher.Object);

            var request = course.GetSectionRequest(sectionRequest, _assessmentClientMock.Object);

            Assert.That(request.Segments.Count, Is.EqualTo(2));

            Assert.That(request.Segments.ElementAt(0).Name, Is.EqualTo(seg1.Name));
            Assert.That(request.Segments.ElementAt(0).ChildSegments.Count, Is.EqualTo(1));

            Assert.That(request.Segments.ElementAt(0).ChildSegments.ElementAt(0).Name, Is.EqualTo(seg11.Name));
            Assert.That(request.Segments.ElementAt(0).ChildSegments.ElementAt(0).ChildSegments.Count, Is.EqualTo(0));

            Assert.That(request.Segments.ElementAt(1).Name, Is.EqualTo(seg2.Name));
            Assert.That(request.Segments.ElementAt(1).ChildSegments.Count, Is.EqualTo(2));

            Assert.That(request.Segments.ElementAt(1).ChildSegments.ElementAt(0).Name, Is.EqualTo(seg21.Name));
            Assert.That(request.Segments.ElementAt(1).ChildSegments.ElementAt(0).ChildSegments.Count, Is.EqualTo(0));

            Assert.That(request.Segments.ElementAt(1).ChildSegments.ElementAt(1).Name, Is.EqualTo(seg22.Name));
            Assert.That(request.Segments.ElementAt(1).ChildSegments.ElementAt(1).ChildSegments.Count, Is.EqualTo(1));

            Assert.That(request.Segments.ElementAt(1).ChildSegments.ElementAt(1).ChildSegments.ElementAt(0).Name, Is.EqualTo(seg221.Name));
            Assert.That(request.Segments.ElementAt(1).ChildSegments.ElementAt(1).ChildSegments.ElementAt(0).ChildSegments.Count, Is.EqualTo(0));
        }

        [Test]
        public void Can_build_section_request_with_subsection_and_learning_activity()
        {
            var sectionRequest = new CourseSectionRequest
            {
                Name = "SectionName",
                Code = "SectionCode",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1)
            };

            var course = GetCourse();

            var seg1Id = Guid.NewGuid();
            var la1Id = Guid.NewGuid();

            var cs = new SaveCourseSegmentRequest
                {
                    Name = "S1",
                    Description = "SegmentDescription",
                    Type = "SomeType",
                    DisplayOrder = 5,
                };

            course.AddSegment(seg1Id, cs);

            var weight = Random.Next(1000);
            var points = Random.Next(weight);

            var cla = new SaveCourseLearningActivityRequest
                {
                    Name = "LA1",
                    Type = RandomEnumValue<CourseLearningActivityType>(),
                    Weight = weight,
                    MaxPoint = points,
                    ActiveDate = Random.Next(50000),
                    InactiveDate = Random.Next(50000),
                    DueDate = Random.Next(50000),
                    IsGradeable = Random.Next(1) == 1,
                    IsExtraCredit = Random.Next(1) == 1,
                    CustomAttribute = Random.Next().ToString(CultureInfo.InvariantCulture)
                };

            course.AddLearningActivity(seg1Id, cla, la1Id);

            course.Publish("It's published", _coursePublisher.Object);

            var request = course.GetSectionRequest(sectionRequest, _assessmentClientMock.Object);

            var segment = request.Segments.First();
            var learningActivity = request.Segments.First().LearningActivities.First();

            Assert.That(segment.Name, Is.EqualTo(cs.Name));
            Assert.That(segment.Description, Is.EqualTo(cs.Description));
            Assert.That(segment.Type, Is.EqualTo(cs.Type));
            Assert.That(segment.DisplayOrder, Is.EqualTo(cs.DisplayOrder));

            Assert.That(learningActivity.Name, Is.EqualTo(cla.Name));
            Assert.That((int)learningActivity.Type, Is.EqualTo((int)cla.Type));
            Assert.That(learningActivity.Weight, Is.EqualTo(cla.Weight));
            Assert.That(learningActivity.MaxPoint, Is.EqualTo(cla.MaxPoint));
            Assert.That(learningActivity.ActiveDate, Is.EqualTo(cla.ActiveDate));
            Assert.That(learningActivity.InactiveDate, Is.EqualTo(cla.InactiveDate));
            Assert.That(learningActivity.DueDate, Is.EqualTo(cla.DueDate));
            Assert.That(learningActivity.IsGradeable, Is.EqualTo(cla.IsGradeable));
            Assert.That(learningActivity.IsExtraCredit, Is.EqualTo(cla.IsExtraCredit));
            Assert.That(learningActivity.CustomAttribute, Is.EqualTo(cla.CustomAttribute));
        }

        [Test]
        public void Can_build_section_request_with_subsections_and_learning_activities()
        {
            var sectionRequest = new CourseSectionRequest
            {
                Name = "SectionName",
                Code = "SectionCode",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1)
            };

            var course = GetCourse();

            var seg1Id = Guid.NewGuid();
            var seg2Id = Guid.NewGuid();
            var la1Id = Guid.NewGuid();
            var la2Id = Guid.NewGuid();
            var la3Id = Guid.NewGuid();

            course.AddSegment(seg1Id, new SaveCourseSegmentRequest { Name = "S1" });
            course.AddSegment(seg2Id, seg1Id, new SaveCourseSegmentRequest { Name = "S2" });
            var la1 = course.AddLearningActivity(seg1Id, new SaveCourseLearningActivityRequest { Name = "LA1" }, la1Id);
            var la2 = course.AddLearningActivity(seg2Id, new SaveCourseLearningActivityRequest { Name = "LA2" }, la2Id);
            var la3 = course.AddLearningActivity(seg2Id, new SaveCourseLearningActivityRequest { Name = "LA2" }, la3Id);

            course.Publish("It's published", _coursePublisher.Object);

            var request = course.GetSectionRequest(sectionRequest, _assessmentClientMock.Object);

            Assert.That(request.Segments.ElementAt(0).LearningActivities.Count, Is.EqualTo(1));
            Assert.That(request.Segments.ElementAt(0).LearningActivities.ElementAt(0).Name, Is.EqualTo(la1.Name));

            Assert.That(request.Segments.ElementAt(0).ChildSegments.ElementAt(0).LearningActivities.Count, Is.EqualTo(2));
            Assert.That(request.Segments.ElementAt(0).ChildSegments.ElementAt(0).LearningActivities.ElementAt(0).Name, Is.EqualTo(la2.Name));
            Assert.That(request.Segments.ElementAt(0).ChildSegments.ElementAt(0).LearningActivities.ElementAt(1).Name, Is.EqualTo(la3.Name));
        }

        [Test]
        public void Can_add_learning_material()
        {
            var course = GetCourse();

            var seg1Id = Guid.NewGuid();
            var seg2Id = Guid.NewGuid();
            var assetId = Guid.NewGuid();
            const bool isRequird = false;
            const string instruction = "learning material";

            course.AddSegment(seg1Id, new SaveCourseSegmentRequest { Name = "S1" });
            course.AddSegment(seg2Id, seg1Id, new SaveCourseSegmentRequest { Name = "S2" });

            var returnedMaterial = course.AddLearningMaterial(seg2Id, new LearningMaterialRequest { Instruction = instruction, AssetId = assetId, IsRequired = isRequird });

            var internalMaterial = course.Segments.First(s => s.Id == seg2Id)
                      .LearningMaterials.Single();

            Assert.That(course.Segments.SelectMany(s => s.LearningMaterials).Count(), Is.EqualTo(1));

            Assert.That(internalMaterial, Is.EqualTo(returnedMaterial));
            Assert.That(internalMaterial.Instruction, Is.EqualTo(instruction));
            Assert.That(internalMaterial.AssetId, Is.EqualTo(assetId));
            Assert.That(internalMaterial.IsRequired, Is.EqualTo(isRequird));
        }

        [Test]
        public void Get_error_if_adding_learning_material_to_unknown_section()
        {
            var course = GetCourse();

            var seg1Id = Guid.NewGuid();
            var seg2Id = Guid.NewGuid();
            var assetId = Guid.NewGuid();
            const bool isRequird = false;
            const string instruction = "learning material";

            course.AddSegment(seg1Id, new SaveCourseSegmentRequest { Name = "S1" });
            course.AddSegment(seg2Id, seg1Id, new SaveCourseSegmentRequest { Name = "S2" });

            Assert.Throws<NotFoundException>(() => course.AddLearningMaterial(Guid.NewGuid(), new LearningMaterialRequest { Instruction = instruction, AssetId = assetId, IsRequired = isRequird }));
        }

        [Test]
        public void Can_clone_learning_material_0utcomes()
        {
            var course = GetCourse();
            const int segmentCount = 5;
            for (int i = 0; i < segmentCount; i++)
            {
                var courseSegment = course.AddSegment(Guid.NewGuid(), new SaveCourseSegmentRequest { });
                courseSegment.AddLearningMaterial(new LearningMaterialRequest());
                courseSegment.AddLearningMaterial(new LearningMaterialRequest());
            }
            var supportOutcomes = new List<OutcomeInfo> { new OutcomeInfo() { Id = Guid.NewGuid() }, new OutcomeInfo() { Id = Guid.NewGuid() } };
            _assessmentClientMock.Setup(a => a.GetSupportingOutcomes(It.IsAny<Guid>(), It.IsAny<string>())).Returns(supportOutcomes);
            course.CloneLearningMaterialOutcomes(_assessmentClientMock.Object);
            var learningMaterialsCount = 0;
            course.Segments.ForEach(s => learningMaterialsCount += s.LearningMaterials.Count);
            _assessmentClientMock.Verify(a => a.SupportsOutcome("learningmaterial", It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Exactly(supportOutcomes.Count * learningMaterialsCount));

        }

        static readonly Random Random = new Random();
        private Mock<ICoursePublisher> _coursePublisher;

        static T RandomEnumValue<T>()
        {
            return Enum
                .GetValues(typeof(T))
                .Cast<T>()
                .OrderBy(x => Random.Next())
                .FirstOrDefault();
        }

        Course.Domain.Courses.Course GetCourse()
        {

            //var course = new Domain.Courses.Course
            //{
            //	TenantId = 999999,
            //	OrganizationId = Guid.NewGuid(),
            //};

            var amoq = AutoMock.GetLoose();
            var course = amoq.Create<Course.Domain.Courses.Course>();
            course.OrganizationId = Guid.NewGuid();
            course.TenantId = 999999;
            return course;
        }
    }
}
