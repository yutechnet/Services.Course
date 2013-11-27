using System;
using System.Linq;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Courses;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Entities
{
	[TestFixture]
	public class CourseLearningActivityTests
	{
		[SetUp]
		public void SetUp()
		{
			
		}

        [Test]
        public void Can_add_learning_material()
        {
            var learningActivity = new CourseLearningActivity { Type = CourseLearningActivityType.Custom };

            var libId = Guid.NewGuid();
            const string description = "new description";
            var learningMaterial = learningActivity.AddLearningMaterial(libId, description);

            Assert.That(learningMaterial.Description, Is.EqualTo(description));
            Assert.That(learningMaterial.LibraryItemId, Is.EqualTo(libId));
            Assert.That(learningActivity.LearningMaterials.Single(), Is.EqualTo(learningMaterial));
        }

        [Test]
        public void Can_delete_learning_material()
        {
            var learningActivity = new CourseLearningActivity { Type = CourseLearningActivityType.Custom };

            var libId = Guid.NewGuid();
            const string description = "new description";
            var learningMaterial = learningActivity.AddLearningMaterial(libId, description);

            Assert.That(learningActivity.LearningMaterials.Single(), Is.EqualTo(learningMaterial));

            learningActivity.DeleteLearningMaterial(learningMaterial.Id);
            Assert.That(learningMaterial.ActiveFlag, Is.False);
        }

		[Test]
		public void Can_add_course_rubric()
		{
			var rubricId = Guid.NewGuid();
			var courseRubricRequest = new CourseRubricRequest {RubricId = rubricId};

			var learningActivity = new CourseLearningActivity {Type = CourseLearningActivityType.Custom, IsGradeable = true};
			var courseRubric = learningActivity.AddCourseRubric(courseRubricRequest);

			Assert.That(courseRubric.RubricId, Is.EqualTo(rubricId));
			Assert.That(learningActivity.CourseRubrics.Count, Is.EqualTo(1));
			Assert.That(learningActivity.CourseRubrics.First().RubricId, Is.EqualTo(rubricId));
		}

		[Test]
		public void Course_rubric_is_only_allowed_for_custom_learningActivities()
		{
			var rubricId = Guid.NewGuid();
			var courseRubricRequest = new CourseRubricRequest { RubricId = rubricId };

			var learningActivityAssessment = new CourseLearningActivity {Type = CourseLearningActivityType.Assessment, IsGradeable = true};
			Assert.Throws(typeof(BadRequestException), () => learningActivityAssessment.AddCourseRubric(courseRubricRequest));

			var learningActivityAssignment = new CourseLearningActivity { Type = CourseLearningActivityType.Assignment, IsGradeable = true};
			Assert.Throws(typeof(BadRequestException), () => learningActivityAssignment.AddCourseRubric(courseRubricRequest));

			var learningActivityDiscussion = new CourseLearningActivity { Type = CourseLearningActivityType.Discussion, IsGradeable = true};
			Assert.Throws(typeof(BadRequestException), () => learningActivityDiscussion.AddCourseRubric(courseRubricRequest));

			var learningActivityQuiz = new CourseLearningActivity { Type = CourseLearningActivityType.Quiz, IsGradeable = true};
			Assert.Throws(typeof(BadRequestException), () => learningActivityQuiz.AddCourseRubric(courseRubricRequest));
		}

		[Test]
		public void Course_rubric_is_only_allowed_for_gradable_learningActivities()
		{
			var rubricId = Guid.NewGuid();
			var courseRubricRequest = new CourseRubricRequest { RubricId = rubricId };

			var learningActivityAssessment = new CourseLearningActivity { Type = CourseLearningActivityType.Custom, IsGradeable = false};
			Assert.Throws(typeof(BadRequestException), () => learningActivityAssessment.AddCourseRubric(courseRubricRequest));
		}

		[Test]
		public void Rubric_cannot_be_added_more_than_once()
		{
			var rubricId = Guid.NewGuid();
			var courseRubricRequest = new CourseRubricRequest { RubricId = rubricId };

			var learningActivity = new CourseLearningActivity { Type = CourseLearningActivityType.Custom, IsGradeable = true};
			learningActivity.AddCourseRubric(courseRubricRequest);
			Assert.Throws(typeof(BadRequestException), () => learningActivity.AddCourseRubric(courseRubricRequest));
		}

		[Test]
		public void Can_delete_courseRubric()
		{
			var rubricId = Guid.NewGuid();
			var courseRubricRequest = new CourseRubricRequest { RubricId = rubricId };

			var learningActivity = new CourseLearningActivity { Type = CourseLearningActivityType.Custom, IsGradeable = true};
			learningActivity.AddCourseRubric(courseRubricRequest);

			Assert.That(learningActivity.CourseRubrics.Count, Is.EqualTo(1));

			learningActivity.DeleteCourseRubric(rubricId);

			Assert.That(learningActivity.CourseRubrics.First().ActiveFlag, Is.EqualTo(false));
		}

		[Test]
		public void Attempt_to_remove_courseRubric_that_doesnt_exist_throws_notFoundException()
		{
			var rubricId = Guid.NewGuid();
			var learningActivity = new CourseLearningActivity { Type = CourseLearningActivityType.Custom, IsGradeable = true};

			Assert.That(learningActivity.CourseRubrics.Count, Is.EqualTo(0));

			Assert.Throws(typeof(NotFoundException), () => learningActivity.DeleteCourseRubric(rubricId));
		}

		[Test]
		public void Attempt_to_alter_learningActivity_is_allowed_so_long_as_type_and_gradability_are_appropriate()
		{
			var rubricId = Guid.NewGuid();
			var courseRubricRequest = new CourseRubricRequest { RubricId = rubricId };

			var learningActivity = new CourseLearningActivity { Type = CourseLearningActivityType.Custom, IsGradeable = true };
			learningActivity.AddCourseRubric(courseRubricRequest);

			Assert.DoesNotThrow(() => learningActivity.Type = CourseLearningActivityType.Custom);
			Assert.DoesNotThrow(() => learningActivity.IsGradeable = true);
		}

		[Test]
		public void Attempt_to_alter_learningActivity_type_when_rubric_assigned_throws_badRequest()
		{
			var rubricId = Guid.NewGuid();
			var courseRubricRequest = new CourseRubricRequest { RubricId = rubricId };

			var learningActivity = new CourseLearningActivity { Type = CourseLearningActivityType.Custom, IsGradeable = true};
			learningActivity.AddCourseRubric(courseRubricRequest);
			
			Assert.Throws(typeof(BadRequestException), () => learningActivity.Type = CourseLearningActivityType.Discussion);
		}

		[Test]
		public void Attempt_to_alter_learningActivity_gradability_when_rubric_assigned_throws_badRequest()
		{
			var rubricId = Guid.NewGuid();
			var courseRubricRequest = new CourseRubricRequest { RubricId = rubricId };

			var learningActivity = new CourseLearningActivity { Type = CourseLearningActivityType.Custom, IsGradeable = true};
			learningActivity.AddCourseRubric(courseRubricRequest);

			Assert.Throws(typeof(BadRequestException), () => learningActivity.IsGradeable = false);
		}
	}
}
