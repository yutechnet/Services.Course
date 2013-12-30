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
        private Course.Domain.Courses.Course _course;
        private Course.Domain.Courses.CourseSegment _courseSegment;
        private LearningMaterial _learningMaterial;
        private Guid _assessmentId;

        private Mock<IAssessmentClient> _assessmentClient;


        [SetUp]
        public void SetUp()
        {
            Mapper.CreateMap<LearningMaterialRequest, LearningMaterial>();

            _assessmentClient = new Mock<IAssessmentClient>();

             _course=new Domain.Courses.Course();
             _learningMaterial = new LearningMaterial();
            _courseSegment=_course.AddSegment(Guid.NewGuid(), new SaveCourseSegmentRequest { });
            _courseSegment.AddLearningMaterial(new LearningMaterialRequest());
            _assessmentId = Guid.NewGuid();
        }


        [Test]
        public void Can_get_Outcomes()
        {
            var supportOutcomes = new List<OutcomeInfo> { new OutcomeInfo() { Id = Guid.NewGuid() }, new OutcomeInfo() { Id = Guid.NewGuid() } };
            _assessmentClient.Setup(a => a.GetSupportingOutcomes(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(supportOutcomes);

          var outComes =  _learningMaterial.GetOutcomes(_assessmentClient.Object);
        }
    }
}
