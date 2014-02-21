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
using OutcomeInfo = Services.Assessment.Contract.OutcomeInfo;

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
            var learningMaterialRequest = new LearningMaterialRequest()
            {
                Instruction = instruction,
                IsRequired = isRequired,
                AssetId = assetId
            };
            var learningMaterial = courseSegment.AddLearningMaterial(learningMaterialRequest);

            Assert.That(learningMaterial.Instruction, Is.EqualTo(instruction));
            Assert.That(learningMaterial.AssetId, Is.EqualTo(assetId));
            Assert.That(courseSegment.LearningMaterials.Single(), Is.EqualTo(learningMaterial));
        }

        [Test]
        public void Can_delete_learning_material()
        {
            var courseSegment = new CourseSegment();

            var assetId = Guid.NewGuid();
            const bool isRequired = false;
            const string instruction = "new description";
            var learningMaterialRequest = new LearningMaterialRequest()
            {
                Instruction = instruction,
                IsRequired = isRequired,
                AssetId = assetId
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
            var learningMaterialRequest = new LearningMaterialRequest()
            {
                Instruction = instruction,
                IsRequired = isRequired,
                AssetId = assetId
            };

            var updateLearningMaterialRequest = new UpdateLearningMaterialRequest()
            {
                Instruction = updateInstruction,
                IsRequired = updateIsRequired,
                AssetId = updateAssetId
            };
            var learningMaterial = courseSegment.AddLearningMaterial(learningMaterialRequest);
            Assert.That(courseSegment.LearningMaterials.Single(), Is.EqualTo(learningMaterial));
            courseSegment.UpdateLearningMaterial(learningMaterial.Id, updateLearningMaterialRequest);
            Assert.That(learningMaterial.AssetId, Is.EqualTo(updateAssetId));
            Assert.That(learningMaterial.Instruction, Is.EqualTo(updateInstruction));
            Assert.That(learningMaterial.IsRequired, Is.EqualTo(updateIsRequired));

        }

        [Test]
        public void Can_clone_learning_material_0utcomes()
        {
            var supportOutcomes = new List<OutcomeInfo> { new OutcomeInfo() { Id = Guid.NewGuid() }, new OutcomeInfo() { Id = Guid.NewGuid() } };
            _mockAssessmentClient.Setup(a => a.GetSupportingOutcomes(It.IsAny<Guid>(), It.IsAny<string>())).Returns(supportOutcomes);
            _courseSegment.CloneLearningMaterialOutcomes(_mockAssessmentClient.Object);
            _mockAssessmentClient.Verify(a => a.SupportsOutcome("learningmaterial", It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Exactly(supportOutcomes.Count*_courseSegment.LearningMaterials.Count));

        }

    }
}
