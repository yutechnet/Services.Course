using System;
using AutoMapper;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.CourseAggregates;
using Moq;
using NUnit.Framework;
using BpeProducts.Services.Assessment.Contract;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Entities
{
    [TestFixture]
    public class LearningMaterialTests
    {
        private Domain.CourseAggregates.Course _course;
        private CourseSegment _courseSegment;
        private LearningMaterial _learningMaterial;
        private Guid _assessmentId;

        private Mock<IAssessmentClient> _mockAssessmentClient;


        [SetUp]
        public void SetUp()
        {
            Mapper.CreateMap<LearningMaterialRequest, LearningMaterial>();
            _mockAssessmentClient = new Mock<IAssessmentClient>();
            _course = new Domain.CourseAggregates.Course();
            _courseSegment = _course.AddSegment(new SaveCourseSegmentRequest { });
            _learningMaterial = _courseSegment.AddLearningMaterial(new LearningMaterialRequest());
            _assessmentId = Guid.NewGuid();
        }

        [Test]
        public void Can_clone_Outcomes()
        {
            _learningMaterial.CloneOutcomes(_mockAssessmentClient.Object);
            _mockAssessmentClient.Verify(a => a.CloneEntityOutcomes(It.IsAny<SupportingEntityType>(), It.IsAny<Guid>(), It.IsAny<CloneEntityOutcomeRequest>()));
        }
    }
}
