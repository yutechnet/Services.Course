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
		public void Can_add_rubric_association()
		{
			var rubricId = Guid.NewGuid();
			var rubricAssociationRequest = new RubricAssociationRequest {RubricId = rubricId};

			var learningActivity = new CourseLearningActivity {Type = CourseLearningActivityType.Custom};
			var rubricAssociation = learningActivity.AddRubricAssociation(rubricAssociationRequest);

			Assert.That(rubricAssociation.RubricId, Is.EqualTo(rubricId));
			Assert.That(learningActivity.RubricAssociations.Count, Is.EqualTo(1));
			Assert.That(learningActivity.RubricAssociations.First().RubricId, Is.EqualTo(rubricId));
		}

		[Test]
		public void Rubric_association_is_only_allowed_for_custom_learningActivities()
		{
			var rubricId = Guid.NewGuid();
			var rubricAssociationRequest = new RubricAssociationRequest { RubricId = rubricId };

			var learningActivity = new CourseLearningActivity {Type = CourseLearningActivityType.Assessment};
			Assert.Throws(typeof(BadRequestException), () => learningActivity.AddRubricAssociation(rubricAssociationRequest));
		}
	}
}
