using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Validation;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Publishing
{
    public class CoursePublishValidatorTests
    {
        private Guid _assessmentId;

        private CoursePublishValidator _validator;
	    private AutoMock _autoMock;

	    [SetUp]
        public void SetUp()
        {
            _assessmentId = Guid.NewGuid();

            _validator = new CoursePublishValidator(new LearningActivityPublishValidator());
	        _autoMock = AutoMock.GetLoose();
        }
		
        [Test]
        public void Can_validate_course_for_publishability()
        {
			var course = _autoMock.Create<Domain.Courses.Course>();
            var seg1 = course.AddSegment(new SaveCourseSegmentRequest());
            course.AddLearningActivity(seg1.Id, new SaveCourseLearningActivityRequest
                {
                    AssessmentId = _assessmentId, AssessmentType = "Essay"
                });

            IEnumerable<string> brokenRules;
            var isValid = course.Validate(_validator, out brokenRules);

            Assert.That(isValid, Is.True);
            Assert.That(brokenRules.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Can_find_broken_rules_before_publishing()
        {
			var course = _autoMock.Create<Domain.Courses.Course>();
            var seg1 = course.AddSegment(new SaveCourseSegmentRequest());
            course.AddLearningActivity(seg1.Id, new SaveCourseLearningActivityRequest
            {
                AssessmentId = Guid.Empty,
                AssessmentType = "Essay"
            });

            IEnumerable<string> brokenRules;
            var isValid = course.Validate(_validator, out brokenRules);

            Assert.That(isValid, Is.False);
            Assert.That(brokenRules.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Can_find_multiple_broken_rules_before_publishing()
        {
			var course = _autoMock.Create<Domain.Courses.Course>();
            var seg1 = course.AddSegment(new SaveCourseSegmentRequest());
            course.AddLearningActivity(seg1.Id, new SaveCourseLearningActivityRequest
            {
                AssessmentId = Guid.Empty,
                AssessmentType = "Essay"
            });

            course.AddLearningActivity(seg1.Id, new SaveCourseLearningActivityRequest
            {
                AssessmentId = Guid.NewGuid(),
                AssessmentType = "Essay"
            });

            IEnumerable<string> brokenRules;
            var isValid = course.Validate(_validator, out brokenRules);

            Assert.That(isValid, Is.False);
            Assert.That(brokenRules.Count(), Is.EqualTo(1));
        }
    }
}
