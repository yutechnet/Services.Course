using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Autofac.Extras.Moq;
using AutoMapper;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.CourseAggregates;
using BpeProducts.Services.Course.Domain.ProgramAggregates;
using Castle.Core.Internal;
using Moq;
using NUnit.Framework;
using Services.Assessment.Contract;
using CourseLearningActivityType = BpeProducts.Services.Course.Contract.CourseLearningActivityType;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Entities
{
    public class CourseTests
    {
        static readonly Random Random = new Random();
        private Mock<ICoursePublisher> _coursePublisher;
        private Mock<IAssessmentClient> _assessmentClientMock;

        [SetUp]
        public void SetUp()
        {
            _assessmentClientMock = new Mock<IAssessmentClient>();
            Mapper.CreateMap<LearningMaterialRequest, LearningMaterial>();
            _coursePublisher = new Mock<ICoursePublisher>();
        }

        [Test]
        public void Can_add_top_level_segments()
        {
            var segmentCount = 5;

            var course = GetCourse();

            for (int i = 0; i < segmentCount; i++)
            {
                var request = new SaveCourseSegmentRequest
                    {
                        Name = "Week" + i,
                        Description = "Week " + i,
                        Type = "Weekly"
                    };

                var segment = course.AddSegment(Guid.Empty, request);

                Assert.That(segment.Id, Is.EqualTo(segment.Id));
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

            var segmentId = course.AddSegment(Guid.Empty, request).Id;

            var parentSegmentId = segmentId;
            var segmentCount = 5;
            for (int i = 0; i < segmentCount; i++)
            {
                request = new SaveCourseSegmentRequest
                    {
                        Name = "Assigment" + i,
                        Description = "Assigment " + i,
                        Type = "Assignment"
                    };

                segmentId = course.AddSegment(parentSegmentId, request).Id;

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

                var newSegment = course.AddSegment(lastSegment == null ? Guid.Empty : lastSegment.Id, request);

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

            var course = courseFactory.Build(new SaveCourseRequest());

            Assert.That(course.Prerequisites, Is.Empty);

            var prerequisiste = courseFactory.Build(new SaveCourseRequest());
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

            var course = courseFactory.Build(new SaveCourseRequest());

            Assert.That(course.Prerequisites, Is.Empty);

            var prerequisiste = courseFactory.Build(new SaveCourseRequest());
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

            var course = courseFactory.Build(new SaveCourseRequest());

            Assert.That(course.Prerequisites, Is.Empty);

            var prerequisiste = courseFactory.Build(new SaveCourseRequest());
            Assert.Throws<ForbiddenException>(() => course.AddPrerequisite(prerequisiste));
        }

        [Test]
        public void Can_remove_course_prerequisite()
        {
            AutoMock autoMock = AutoMock.GetLoose();
            var courseFactory = autoMock.Create<CourseFactory>();

            var course = courseFactory.Build(new SaveCourseRequest());

            Assert.That(course.Prerequisites, Is.Empty);

            var prerequisiste = courseFactory.Build(new SaveCourseRequest());
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

            var course = courseFactory.Build(new SaveCourseRequest());

            Assert.That(course.Prerequisites, Is.Empty);

            course.RemovePrerequisite(Guid.NewGuid());
            Assert.That(course.Prerequisites.Count, Is.EqualTo(0));
        }

        [Test]
        public void Cannot_modify_published_course()
        {
            AutoMock autoMock = AutoMock.GetLoose();
            var courseFactory = autoMock.Create<CourseFactory>();
            var course = courseFactory.Build(new SaveCourseRequest());
            var prerequisiteCourse = courseFactory.Build(new SaveCourseRequest());
            prerequisiteCourse.Publish("", _coursePublisher.Object);

            Assert.DoesNotThrow(() => course.Name = "name");
            Assert.DoesNotThrow(() => course.Description = "description");
            Assert.DoesNotThrow(() => course.CourseType = ECourseType.Competency);
            Assert.DoesNotThrow(() => course.Code = "code");
            Assert.DoesNotThrow(() => course.SetPrograms(new List<Program>()));
            Assert.DoesNotThrow(() => course.AddPrerequisite(prerequisiteCourse));
            Assert.DoesNotThrow(() => course.RemovePrerequisite(Guid.NewGuid()));
            Assert.DoesNotThrow(() => course.AddSegment(Guid.Empty, new SaveCourseSegmentRequest()));

            course.Publish("note", _coursePublisher.Object);

            Assert.Throws<BadRequestException>(() => course.Name = "name");
            Assert.Throws<BadRequestException>(() => course.Description = "description");
            Assert.Throws<BadRequestException>(() => course.CourseType = ECourseType.Competency);
            Assert.Throws<BadRequestException>(() => course.Code = "code");
            Assert.Throws<BadRequestException>(() => course.SetPrograms(new List<Program>()));
            Assert.Throws<BadRequestException>(() => course.AddPrerequisite(prerequisiteCourse));
            Assert.Throws<BadRequestException>(() => course.RemovePrerequisite(Guid.NewGuid()));
            Assert.Throws<BadRequestException>(() => course.AddSegment(Guid.Empty, new SaveCourseSegmentRequest()));
            Assert.Throws<BadRequestException>(() => course.AddLearningMaterial(Guid.NewGuid(), new LearningMaterialRequest()));
            Assert.Throws<BadRequestException>(() => course.DeleteLearningMaterial(Guid.NewGuid(), Guid.NewGuid()));
        }

        [Test]
        public void Can_add_learning_activity_to_segment()
        {
            var course = GetCourse();

            var request = new SaveCourseSegmentRequest
                {
                    Name = "Week 1",
                    Description = "Week 1 Description",
                    Type = "Weekly"
                };

            var segmentId = course.AddSegment(Guid.Empty, request).Id;
            var segment = course.Segments.First(s => s.Name == request.Name);

            var learningActivityRequest = new SaveCourseLearningActivityRequest
                {
                    Name = "Discussion 1",
                    //Type = "Discussion",
                    Type = CourseLearningActivityType.Discussion,
                    IsGradeable = true,
                    IsExtraCredit = false,
                    MaxPoint = 100,
                    Description = "Some Description"

                };

            var learningActivityId = course.AddLearningActivity(segmentId, learningActivityRequest).Id;

            var learningActivity =
                segment.CourseLearningActivities.First(s => s.Name == learningActivityRequest.Name);

            Assert.That(learningActivity.Id, Is.EqualTo(learningActivityId));
            Assert.That(learningActivity.Name, Is.EqualTo(learningActivityRequest.Name));
            Assert.That(learningActivity.Type, Is.EqualTo(learningActivityRequest.Type));
            Assert.That(learningActivity.IsGradeable, Is.EqualTo(learningActivityRequest.IsGradeable));
            Assert.That(learningActivity.IsExtraCredit, Is.EqualTo(learningActivityRequest.IsExtraCredit));
            Assert.That(learningActivity.MaxPoint, Is.EqualTo(learningActivityRequest.MaxPoint));
            Assert.That(learningActivity.TenantId, Is.EqualTo(course.TenantId));
            Assert.That(learningActivity.Description, Is.EqualTo(learningActivityRequest.Description));
        }

        [Test]
        public void Can_get_learning_activity_from_course()
        {
            var course = GetCourse();

            var request = new SaveCourseSegmentRequest
            {
                Name = "Week 1",
                Description = "Week 1 Description",
                Type = "Weekly"
            };

            var segmentId = course.AddSegment(Guid.Empty, request).Id;

            var learningActivityRequest = new SaveCourseLearningActivityRequest
            {
                Name = "Discussion 1",
                //Type = "Discussion",
                Type = CourseLearningActivityType.Discussion,
                IsGradeable = true,
                IsExtraCredit = false,
                MaxPoint = 100,
                Description = "desc"
            };

            var learningActivityId = course.AddLearningActivity(segmentId, learningActivityRequest).Id;
            var learningActivity = course.GetLearningActivity(segmentId, learningActivityId);

            Assert.That(learningActivity.Id, Is.EqualTo(learningActivityId));
            Assert.That(learningActivity.Name, Is.EqualTo(learningActivityRequest.Name));
            Assert.That(learningActivity.Type, Is.EqualTo(learningActivityRequest.Type));
            Assert.That(learningActivity.IsGradeable, Is.EqualTo(learningActivityRequest.IsGradeable));
            Assert.That(learningActivity.IsExtraCredit, Is.EqualTo(learningActivityRequest.IsExtraCredit));
            Assert.That(learningActivity.MaxPoint, Is.EqualTo(learningActivityRequest.MaxPoint));
            Assert.That(learningActivity.TenantId, Is.EqualTo(course.TenantId));
            Assert.That(learningActivity.Description, Is.EqualTo(learningActivityRequest.Description));
        }

        [Test]
        public void Can_check_for_missing_segment_or_learningActivity()
        {
            var course = GetCourse();

            var request = new SaveCourseSegmentRequest
                {
                    Name = "Week 1",
                    Description = "Week 1 Description",
                    Type = "Weekly"
                };

            var segmentId = course.AddSegment(Guid.Empty, request).Id;

            var learningActivityRequest = new SaveCourseLearningActivityRequest
                {
                    Name = "Discussion 1",
                    //Type = "Discussion",
                    Type = CourseLearningActivityType.Discussion,
                    IsGradeable = true,
                    IsExtraCredit = false,
                    MaxPoint = 100

                };

            var learningActivityId = course.AddLearningActivity(segmentId, learningActivityRequest).Id;

            Assert.Throws<NotFoundException>(() => course.GetLearningActivity(segmentId, Guid.NewGuid()));
            Assert.Throws<NotFoundException>(() => course.GetLearningActivity(Guid.NewGuid(), learningActivityId));

            course.DeleteLearningActivity(segmentId, learningActivityId);
            Assert.Throws<NotFoundException>(() => course.GetLearningActivity(segmentId, learningActivityId));
        }

        [Test]
        public void Can_check_for_missing_parent_segment_during_adding_segment()
        {
            var course = GetCourse();

            Assert.Throws<BadRequestException>(() => course.AddSegment(Guid.NewGuid(), new SaveCourseSegmentRequest()));
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
                    CourseCode = "SectionCode",
                    SectionCode = "SectionCode",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(1),
                    OrganizationId = Guid.NewGuid(),
                };

            var course = GetCourse();

            var learningMaterial = new LearningMaterial
            {
                Id = Guid.NewGuid(),
                AssetId = Guid.NewGuid(),
                Instruction = "test lm",
                IsRequired = false,
                ActiveFlag = true,
                Course = course
            };
            course.LearningMaterials.Add(learningMaterial);
            
            course.Publish("It's published", _coursePublisher.Object);

            var request = course.GetSectionRequest(sectionRequest, _assessmentClientMock.Object);

            Assert.That(request.Name, Is.EqualTo(sectionRequest.Name));
            Assert.That(request.CourseCode, Is.EqualTo(sectionRequest.CourseCode));
            Assert.That(request.SectionCode, Is.EqualTo(sectionRequest.SectionCode));
            Assert.That(request.OrganizationId, Is.EqualTo(sectionRequest.OrganizationId));
            Assert.That(request.StartDate, Is.EqualTo(sectionRequest.StartDate));
            Assert.That(request.EndDate, Is.EqualTo(sectionRequest.EndDate));
            Assert.That(request.CourseId, Is.EqualTo(course.Id));
            Assert.That(request.TenantId, Is.EqualTo(course.TenantId));

            var sectionLearningMaterial = course.LearningMaterials.FirstOrDefault();
            Assert.That(sectionLearningMaterial.CustomAttribute, Is.EqualTo(learningMaterial.CustomAttribute));
            Assert.That(sectionLearningMaterial.Instruction, Is.EqualTo(learningMaterial.Instruction));
            Assert.That(sectionLearningMaterial.IsRequired, Is.EqualTo(learningMaterial.IsRequired));
            Assert.That(sectionLearningMaterial.AssetId, Is.EqualTo(learningMaterial.AssetId));
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
                CourseCode = "SectionCode",
                SectionCode = "SectionCode",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1)
            };

            var course = GetCourse();

            var seg1 = course.AddSegment(new SaveCourseSegmentRequest { Name = "1" });
            var seg11 = course.AddSegment(seg1.Id, new SaveCourseSegmentRequest { Name = "11" });
            var seg2 = course.AddSegment(new SaveCourseSegmentRequest { Name = "2" });
            var seg21 = course.AddSegment(seg2.Id, new SaveCourseSegmentRequest { Name = "21" });
            var seg22 = course.AddSegment(seg2.Id, new SaveCourseSegmentRequest { Name = "22" });
            var seg221 = course.AddSegment(seg22.Id, new SaveCourseSegmentRequest { Name = "221" });

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
                CourseCode = "SectionCode",
                SectionCode = "SectionCode",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1)
            };

            var course = GetCourse();

            var cs = new SaveCourseSegmentRequest
                {
                    Name = "S1",
                    Description = "SegmentDescription",
                    Type = "SomeType",
                    DisplayOrder = 5,
                };

            var seg1 = course.AddSegment(cs);

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

            course.AddLearningActivity(seg1.Id, cla);

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
                CourseCode = "SectionCode",
                SectionCode = "SectionCode",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1)
            };

            var course = GetCourse();

            var seg1 = course.AddSegment(new SaveCourseSegmentRequest { Name = "S1" });
            var seg2 = course.AddSegment(seg1.Id, new SaveCourseSegmentRequest { Name = "S2" });
            var la1 = course.AddLearningActivity(seg1.Id, new SaveCourseLearningActivityRequest { Name = "LA1" });
            var la2 = course.AddLearningActivity(seg2.Id, new SaveCourseLearningActivityRequest { Name = "LA2" });
            var la3 = course.AddLearningActivity(seg2.Id, new SaveCourseLearningActivityRequest { Name = "LA2" });

            course.Publish("It's published", _coursePublisher.Object);

            var request = course.GetSectionRequest(sectionRequest, _assessmentClientMock.Object);

            Assert.That(request.Segments.ElementAt(0).LearningActivities.Count, Is.EqualTo(1));
            Assert.That(request.Segments.ElementAt(0).LearningActivities.ElementAt(0).Name, Is.EqualTo(la1.Name));
            Assert.That(request.Segments.ElementAt(0).LearningActivities.ElementAt(0).CourseLearningActivityId.HasValue);

            Assert.That(request.Segments.ElementAt(0).ChildSegments.ElementAt(0).LearningActivities.Count, Is.EqualTo(2));
            Assert.That(request.Segments.ElementAt(0).ChildSegments.ElementAt(0).LearningActivities.ElementAt(0).Name, Is.EqualTo(la2.Name));
            Assert.That(request.Segments.ElementAt(0).ChildSegments.ElementAt(0).LearningActivities.ElementAt(0).CourseLearningActivityId.HasValue);
            Assert.That(request.Segments.ElementAt(0).ChildSegments.ElementAt(0).LearningActivities.ElementAt(1).Name, Is.EqualTo(la3.Name));
            Assert.That(request.Segments.ElementAt(0).ChildSegments.ElementAt(0).LearningActivities.ElementAt(1).CourseLearningActivityId.HasValue);
        }

        [Test]
        public void Can_build_section_request_with_subsections_and_learning_materials()
        {
            var sectionRequest = new CourseSectionRequest
            {
                Name = "SectionName",
                CourseCode = "SectionCode",
                SectionCode = "SectionCode",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1)
            };

            var course = GetCourse();

            var seg1 = course.AddSegment(new SaveCourseSegmentRequest { Name = "S1" });
            var seg2 = course.AddSegment(seg1.Id, new SaveCourseSegmentRequest { Name = "S2" });

            var lm1 = course.AddLearningMaterial(seg1.Id, new LearningMaterialRequest { Instruction = "LM1" });
            var lm2 = course.AddLearningMaterial(seg2.Id, new LearningMaterialRequest { Instruction = "LM2" });
            var lm3 = course.AddLearningMaterial(seg2.Id, new LearningMaterialRequest { Instruction = "LM3" });

            course.Publish("It's published", _coursePublisher.Object);

            var request = course.GetSectionRequest(sectionRequest, _assessmentClientMock.Object);

            Assert.That(request.Segments.ElementAt(0).LearningMaterials.Count, Is.EqualTo(1));
            Assert.That(request.Segments.ElementAt(0).LearningMaterials.ElementAt(0).CourseLearningMaterialId.HasValue);

            Assert.That(request.Segments.ElementAt(0).ChildSegments.ElementAt(0).LearningMaterials.Count, Is.EqualTo(2));
            Assert.That(request.Segments.ElementAt(0).ChildSegments.ElementAt(0).LearningMaterials.ElementAt(0).CourseLearningMaterialId.HasValue);
            Assert.That(request.Segments.ElementAt(0).ChildSegments.ElementAt(0).LearningMaterials.ElementAt(1).CourseLearningMaterialId.HasValue);
        }

        [Test]
        public void Can_add_learning_material()
        {
            var course = GetCourse();

            var assetId = Guid.NewGuid();
            const bool isRequird = false;
            const string instruction = "learning material";

            var seg1 = course.AddSegment(new SaveCourseSegmentRequest { Name = "S1" });
            var seg2 = course.AddSegment(seg1.Id, new SaveCourseSegmentRequest { Name = "S2" });

            var returnedMaterial = course.AddLearningMaterial(seg2.Id, new LearningMaterialRequest { Instruction = instruction, AssetId = assetId, IsRequired = isRequird });

            var internalMaterial = course.Segments.First(s => s.Id == seg2.Id)
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

            var assetId = Guid.NewGuid();
            const bool isRequird = false;
            const string instruction = "learning material";

            var seg1 = course.AddSegment(new SaveCourseSegmentRequest { Name = "S1" });
            course.AddSegment(seg1.Id, new SaveCourseSegmentRequest { Name = "S2" });

            Assert.Throws<NotFoundException>(() => course.AddLearningMaterial(Guid.NewGuid(), new LearningMaterialRequest { Instruction = instruction, AssetId = assetId, IsRequired = isRequird }));
        }

        [Test]
        public void Can_clone_Outcomes()
        {
            var course = GetCourse();
            var courseSegment = course.AddSegment(new SaveCourseSegmentRequest());
            courseSegment.CourseLearningActivities.Add(new CourseLearningActivity());
            courseSegment.AddLearningMaterial(new LearningMaterialRequest());
            course.CloneOutcomes(_assessmentClientMock.Object);
            // Call to clone course learning outcomes
            _assessmentClientMock.Verify(
                a =>
                a.CloneEntityOutcomes(SupportingEntityType.Course, It.IsAny<Guid>(),
                                      It.Is<CloneEntityOutcomeRequest>(c => c.Type == SupportingEntityType.Course)));
            //Call to clone segment
            _assessmentClientMock.Verify(
               a =>
               a.CloneEntityOutcomes(SupportingEntityType.Segment, It.IsAny<Guid>(),
                                     It.Is<CloneEntityOutcomeRequest>(c => c.Type == SupportingEntityType.Segment)));
            //Call to clone learning activity
            _assessmentClientMock.Verify(
               a =>
               a.CloneEntityOutcomes(SupportingEntityType.LearningActivity, It.IsAny<Guid>(),
                                     It.Is<CloneEntityOutcomeRequest>(c => c.Type == SupportingEntityType.LearningActivity)));
            //Call to clone learning material
            _assessmentClientMock.Verify(
               a =>
               a.CloneEntityOutcomes(SupportingEntityType.LearningMaterial, It.IsAny<Guid>(),
                                     It.Is<CloneEntityOutcomeRequest>(c => c.Type == SupportingEntityType.LearningMaterial)));
        }

        [Test]
        public void Can_add_course_learning_material_to_course()
        {
            var course = GetCourse();

            var learningMaterialRequest = new LearningMaterialRequest
            {
                AssetId = Guid.NewGuid(),
                Instruction = "test lm",
                IsRequired = false
            };

            var learningMaterialId = course.AddLearningMaterial(learningMaterialRequest).Id;

            var learningMaterial =
                course.LearningMaterials.First(l => l.Instruction == learningMaterialRequest.Instruction);

            Assert.That(learningMaterial.Id, Is.EqualTo(learningMaterialId));
            Assert.That(learningMaterial.IsRequired, Is.EqualTo(learningMaterialRequest.IsRequired));
            Assert.That(learningMaterial.AssetId, Is.EqualTo(learningMaterialRequest.AssetId));
        }

        [Test]
        public void Can_update_course_learning_material_to_course()
        {

            Mapper.CreateMap<UpdateLearningMaterialRequest, LearningMaterial>();

            var course = GetCourse();

            var learningMaterialRequest = new LearningMaterialRequest
            {
                AssetId = Guid.NewGuid(),
                Instruction = "test lm",
                IsRequired = false
            };
            var updateLearningMaterialRequest = new UpdateLearningMaterialRequest
            {
                AssetId = Guid.NewGuid(),
                Instruction = "test lm update",
                IsRequired = true
            };
            var learningMaterialId = course.AddLearningMaterial(learningMaterialRequest).Id;
            course.UpdateLearningMaterial(learningMaterialId, updateLearningMaterialRequest);

            var learningMaterial =
                course.LearningMaterials.First(l => l.Id == learningMaterialId);

            Assert.That(learningMaterial.Instruction, Is.EqualTo(updateLearningMaterialRequest.Instruction));
            Assert.That(learningMaterial.IsRequired, Is.EqualTo(updateLearningMaterialRequest.IsRequired));
            Assert.That(learningMaterial.AssetId, Is.EqualTo(updateLearningMaterialRequest.AssetId));
        }

        [Test]
        public void Can_delete_course_learning_material_from_course()
        {
            var course = GetCourse();

            var learningMaterialRequest = new LearningMaterialRequest
            {
                AssetId = Guid.NewGuid(),
                Instruction = "test lm",
                IsRequired = false
            };

            var learningMaterialId = course.AddLearningMaterial(learningMaterialRequest).Id;
            course.DeleteLearningMaterial(learningMaterialId);

            Assert.That(course.LearningMaterials.Count(l => l.ActiveFlag), Is.EqualTo(0));
        }

        [Test]
        public void Get_error_if_delete_unknow_course_learning_material_from_course()
        {
            var course = GetCourse();
            Assert.Throws<NotFoundException>(() => course.DeleteLearningMaterial(Guid.NewGuid()));
        }

        [Test]
        public void CanDeleteCourse()
        {
            var course = GetCourse();
            course.Delete();

            Assert.That(course.ActiveFlag, Is.False);
        }

        [Test]
        public void CannotDeletePublishedCourse()
        {
            var course = GetCourse();
            course.Publish("note", _coursePublisher.Object);
            Assert.Throws<BadRequestException>(course.Delete);
        }

        [Test]
        public void ExtensionAssets_removes_empty_guids()
        {
            // arrange
            var guid = Guid.NewGuid();
            var course = new Domain.CourseAggregates.Course();
            
            // act
            course.ExtensionAssets = new List<Guid>{ guid, Guid.Empty };
            
            // assert
            CollectionAssert.DoesNotContain(course.ExtensionAssets, Guid.Empty);
            CollectionAssert.Contains(course.ExtensionAssets, guid);
        }

        static T RandomEnumValue<T>()
        {
            return Enum
                .GetValues(typeof(T))
                .Cast<T>()
                .OrderBy(x => Random.Next())
                .FirstOrDefault();
        }

        Domain.CourseAggregates.Course GetCourse()
        {
            var amoq = AutoMock.GetLoose();
            var course = amoq.Create<Domain.CourseAggregates.Course>();
            course.OrganizationId = Guid.NewGuid();
            course.TenantId = 999999;
            return course;
        }
    }
}
