using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Courses;
using BpeProducts.Services.Course.Domain.Validation;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Validation
{
    public class CoursePublishValidatorTests
    {
        private Guid _courseId;
        private Guid _segmentId;
        private Guid _learningActivityId;
        private Guid _assessmentId;

        private Mock<IValidator<CourseLearningActivity>> _mockLearningActivityValidator; 
        private CoursePublishValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _courseId = Guid.NewGuid();
            _segmentId = Guid.NewGuid();
            _learningActivityId = Guid.NewGuid();
            _assessmentId = Guid.NewGuid();

            _mockLearningActivityValidator = new Mock<IValidator<CourseLearningActivity>>();

            _validator = new CoursePublishValidator(new LearningActivityPublishValidator());
        }

        [Test]
        public void Can_validate_course_for_publishability()
        {
            var course = new Course.Domain.Courses.Course();
            course.AddSegment(_segmentId, new SaveCourseSegmentRequest());
            course.AddLearningActivity(_segmentId, new SaveCourseLearningActivityRequest
                {
                    AssessmentId = _assessmentId, AssessmentType = "Essay"
                }, _learningActivityId);

            IEnumerable<string> brokenRules;
            var isValid = course.Validate(_validator, out brokenRules);

            Assert.That(isValid, Is.True);
            Assert.That(brokenRules.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Can_find_broken_rules_before_publishing()
        {
            var course = new Course.Domain.Courses.Course();
            course.AddSegment(_segmentId, new SaveCourseSegmentRequest());
            course.AddLearningActivity(_segmentId, new SaveCourseLearningActivityRequest
            {
                AssessmentId = Guid.Empty,
                AssessmentType = "Essay"
            }, _learningActivityId);

            IEnumerable<string> brokenRules;
            var isValid = course.Validate(_validator, out brokenRules);

            Assert.That(isValid, Is.False);
            Assert.That(brokenRules.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Can_find_multiple_broken_rules_before_publishing()
        {
            var course = new Course.Domain.Courses.Course();
            course.AddSegment(_segmentId, new SaveCourseSegmentRequest());
            course.AddLearningActivity(_segmentId, new SaveCourseLearningActivityRequest
            {
                AssessmentId = Guid.Empty,
                AssessmentType = "Essay"
            }, Guid.NewGuid());
            course.AddLearningActivity(_segmentId, new SaveCourseLearningActivityRequest
            {
                AssessmentId = Guid.NewGuid(),
                AssessmentType = "Essay"
            }, Guid.NewGuid());

            IEnumerable<string> brokenRules;
            var isValid = course.Validate(_validator, out brokenRules);

            Assert.That(isValid, Is.False);
            Assert.That(brokenRules.Count(), Is.EqualTo(1));
        }
    }
}
