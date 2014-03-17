using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Courses;
using Moq;
using NUnit.Framework;
using Services.Assessment.Contract;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Entities
{
    [TestFixture]
    public class CourseSegmentTests
    {
        private Domain.Courses.Course _course;
        private CourseSegment _courseSegment;

        private Mock<IAssessmentClient> _mockAssessmentClient;
        [SetUp]
        public void SetUp()
        {
            Mapper.CreateMap<LearningMaterialRequest, LearningMaterial>();
            Mapper.CreateMap<UpdateLearningMaterialRequest, LearningMaterial>();
            _mockAssessmentClient = new Mock<IAssessmentClient>();
            _course = new Domain.Courses.Course();
            _courseSegment = _course.AddSegment(Guid.NewGuid(), new SaveCourseSegmentRequest { });
            _courseSegment.AddLearningMaterial(new LearningMaterialRequest());
            _courseSegment.AddLearningMaterial(new LearningMaterialRequest());
        }

        [Test]
        public void Can_add_learning_material()
        {
            var courseSegment = new CourseSegment();

            var assetId = Guid.NewGuid();
            const bool isRequired = false;
            const string instruction = "new description";
            const string customAttribute = "new CustomAttribute";
            var learningMaterialRequest = new LearningMaterialRequest()
            {
                Instruction = instruction,
                IsRequired = isRequired,
                AssetId = assetId,
                CustomAttribute = customAttribute
            };
            var learningMaterial = courseSegment.AddLearningMaterial(learningMaterialRequest);

            Assert.That(learningMaterial.Instruction, Is.EqualTo(instruction));
            Assert.That(learningMaterial.AssetId, Is.EqualTo(assetId));
            Assert.That(learningMaterial.CustomAttribute, Is.EqualTo(customAttribute));
            Assert.That(courseSegment.LearningMaterials.Single(), Is.EqualTo(learningMaterial));
        }

        [Test]
        public void Can_delete_learning_material()
        {
            var courseSegment = new CourseSegment();

            var assetId = Guid.NewGuid();
            const bool isRequired = false;
            const string instruction = "new description";
            const string customAttribute = "customAttribute";

            var learningMaterialRequest = new LearningMaterialRequest()
            {
                Instruction = instruction,
                IsRequired = isRequired,
                AssetId = assetId,
                CustomAttribute = customAttribute
            };
            var learningMaterial = courseSegment.AddLearningMaterial(learningMaterialRequest);
            Assert.That(courseSegment.LearningMaterials.Single(), Is.EqualTo(learningMaterial));
            courseSegment.DeleteLearningMaterial(learningMaterial.Id);
            Assert.That(learningMaterial.ActiveFlag, Is.False);
        }

        [Test]
        public void Can_update_learning_material()
        {
            var courseSegment = new CourseSegment();

            var assetId = Guid.NewGuid();
            var updateAssetId = Guid.NewGuid();
            const bool isRequired = false;
            const bool updateIsRequired = false;
            const string instruction = "new instruction";
            const string updateInstruction = "update instruction";
            const string customAttribute = "customAttribute";
            const string updateCustomAttribute = "update customAttribute";
            var learningMaterialRequest = new LearningMaterialRequest()
            {
                Instruction = instruction,
                IsRequired = isRequired,
                AssetId = assetId,
                CustomAttribute = customAttribute
            };

            var updateLearningMaterialRequest = new UpdateLearningMaterialRequest()
            {
                Instruction = updateInstruction,
                IsRequired = updateIsRequired,
                AssetId = updateAssetId,
                CustomAttribute = updateCustomAttribute
            };
            var learningMaterial = courseSegment.AddLearningMaterial(learningMaterialRequest);
            Assert.That(courseSegment.LearningMaterials.Single(), Is.EqualTo(learningMaterial));
            courseSegment.UpdateLearningMaterial(learningMaterial.Id, updateLearningMaterialRequest);
            Assert.That(learningMaterial.AssetId, Is.EqualTo(updateAssetId));
            Assert.That(learningMaterial.Instruction, Is.EqualTo(updateInstruction));
            Assert.That(learningMaterial.IsRequired, Is.EqualTo(updateIsRequired));
            Assert.That(learningMaterial.CustomAttribute, Is.EqualTo(updateCustomAttribute));

        }

        [Test]
        public void Can_clone_Outcomes()
        {
            _courseSegment.CloneOutcomes(_mockAssessmentClient.Object);
            _mockAssessmentClient.Verify(a => a.CloneEntityOutcomes(It.IsAny<SupportingEntityType>(), It.IsAny<Guid>(), It.IsAny<CloneEntityOutcomeRequest>()));
        }

    }
}
