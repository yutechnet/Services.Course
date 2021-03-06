﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.CourseAggregates;
using Moq;
using NUnit.Framework;
using BpeProducts.Services.Assessment.Contract;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Entities
{
    [TestFixture]
    public class LearningActivityAssessmentTests
    {
        private Domain.CourseAggregates.Course _course;
        private CourseSegment _courseSegment;
        private CourseLearningActivity _learningActivity;
        private Guid _assessmentId;

        [SetUp]
        public void SetUp()
        {
            _course = new Domain.CourseAggregates.Course();
            _courseSegment = _course.AddSegment(new SaveCourseSegmentRequest());
            _learningActivity = _course.AddLearningActivity(_courseSegment.Id, new SaveCourseLearningActivityRequest { AssessmentType = "Custom" });
            _assessmentId = Guid.NewGuid();
        }

        [Test]
        public void Can_create_learning_activity_with_assessment()
        {
            var courseLearningActivityRequest=new SaveCourseLearningActivityRequest {AssessmentId = _assessmentId, AssessmentType = "Essay"};
            var learningActivity=_course.AddLearningActivity(_courseSegment.Id, courseLearningActivityRequest);
            Assert.That(learningActivity.AssessmentId, Is.EqualTo(_assessmentId));
            Assert.That(learningActivity.AssessmentType, Is.EqualTo(AssessmentType.Essay));
        }

        [Test]
        public void Can_add_assessment_to_existing_learning_activity()
        {
            var courseLearningActivityRequest = new SaveCourseLearningActivityRequest { AssessmentId = _assessmentId,AssessmentType = "Essay"};
            var learningActivity = _course.UpdateLearningActivity(_courseSegment.Id,_learningActivity.Id, courseLearningActivityRequest);
            Assert.That(learningActivity.AssessmentId, Is.EqualTo(_assessmentId));
        }


        // TODO: Uncomment this when something magical happens, like pigs fly
        //[Test]
        //public void Should_publish_assessment_when_course_is_published()
        //{
        //    _assessmentClient.Setup(a => a.GetAssessment(_assessmentId))
        //                    .Returns(new AssessmentResponse
        //                    {
        //                        Id = _assessmentId,
        //                        AssessmentType = AssessmentType.WrittenWork
        //                    });

        //    var courseLearningActivityRequest = new SaveCourseLearningActivityRequest { AssessmentId = _assessmentId };
        //    var learningActivity = _course.AddLearningActivity(_courseSegment.Id, courseLearningActivityRequest, Guid.NewGuid());
        //    _course.Publish("make assesment service dance");
        //    _assessmentClient.Verify(a=>a.Publish(_assessmentId));
        //}
    }
}
