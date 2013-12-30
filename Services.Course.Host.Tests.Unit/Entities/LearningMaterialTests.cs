using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Courses;
using Moq;
using NUnit.Framework;
using Services.Assessment.Contract;
using OutcomeInfo = Services.Assessment.Contract.OutcomeInfo;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Entities
{
    [TestFixture]
    public class LearningMaterialTests
    {
        private Domain.Courses.Course _course;
        private CourseSegment _courseSegment;
        private LearningMaterial _learningMaterial;
        private Guid _assessmentId;

        private Mock<IAssessmentClient> _mockAssessmentClient;


        [SetUp]
        public void SetUp()
        {
            Mapper.CreateMap<LearningMaterialRequest, LearningMaterial>();
            _mockAssessmentClient = new Mock<IAssessmentClient>();
            _course = new Domain.Courses.Course();
            _courseSegment = _course.AddSegment(Guid.NewGuid(), new SaveCourseSegmentRequest { });
            _learningMaterial = _courseSegment.AddLearningMaterial(new LearningMaterialRequest());
            _assessmentId = Guid.NewGuid();
        }


        [Test]
        public void Can_get_Outcomes()
        {
            var supportOutcomes = new List<OutcomeInfo> { new OutcomeInfo() { Id = Guid.NewGuid() }, new OutcomeInfo() { Id = Guid.NewGuid() } };
            _mockAssessmentClient.Setup(a => a.GetSupportingOutcomes(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(supportOutcomes);
            var retunSupportOutComes = _learningMaterial.GetOutcomes(_mockAssessmentClient.Object);
            Assert.That(supportOutcomes.Count, Is.EqualTo(retunSupportOutComes.Count));
        }

        [Test]
        public void Can_clone_learning_material_0utcomes()
        {
            var supportOutcomes = new List<OutcomeInfo> { new OutcomeInfo() { Id = Guid.NewGuid() }, new OutcomeInfo() { Id = Guid.NewGuid() } };
            _mockAssessmentClient.Setup(a => a.GetSupportingOutcomes(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(supportOutcomes);
            _learningMaterial.CloneLearningMaterialOutcomes(_mockAssessmentClient.Object);
            _mockAssessmentClient.Verify(a => a.SupportsOutcome("learningmaterial", It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Exactly(supportOutcomes.Count));

        }
    }
}
