using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using AutoMapper;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Courses;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using Moq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit
{
    [TestFixture]
    public class LearningMaterialServiceTests
    {
        private ILearningMaterialService _learningMaterialService;
        private Mock<ICourseRepository> _repoMock;
        private AutoMock _autoMock;
        private Domain.Courses.Course courseToReturn;

        [SetUp]
        public void SetUp()
        {
            _autoMock = AutoMock.GetLoose();
            _repoMock = _autoMock.Mock<ICourseRepository>();
            _learningMaterialService = _autoMock.Create<LearningMaterialService>();
            Mapper.CreateMap<LearningMaterialRequest, Domain.Courses.LearningMaterial>();
            Mapper.CreateMap<UpdateLearningMaterialRequest, Domain.Courses.LearningMaterial>();
            Mapper.CreateMap<Domain.Courses.LearningMaterial, LearningMaterialInfo>();
             courseToReturn = new Domain.Courses.Course();
            courseToReturn.Id = Guid.NewGuid();
            courseToReturn.ActiveFlag = true;
            _repoMock.Setup(c => c.GetOrThrow(It.IsAny<Guid>())).Returns(courseToReturn);
        }

        [Test]
        public void Can_add_course_learning_material_to_course()
        {
            var learningMaterialRequest = new LearningMaterialRequest
            {
                AssetId = Guid.NewGuid(),
                Instruction = "test lm",
                IsRequired = false
            };
            var learningMaterial = _learningMaterialService.AddLearningMaterial(courseToReturn.Id, learningMaterialRequest);
            Assert.That(learningMaterial.CourseId, Is.EqualTo(courseToReturn.Id));
            Assert.That(learningMaterial.Instruction, Is.EqualTo(learningMaterialRequest.Instruction));
            Assert.That(learningMaterial.AssetId, Is.EqualTo(learningMaterialRequest.AssetId));
            Assert.That(learningMaterial.IsRequired, Is.EqualTo(learningMaterialRequest.IsRequired));
        }

        [Test]
        public void Can_get_course_learning_material_to_course()
        {
            var learningMaterial = new LearningMaterial
            {
                Id = Guid.NewGuid(),
                AssetId = Guid.NewGuid(),
                Instruction = "test lm",
                IsRequired = false,
                ActiveFlag = true,
                Course = courseToReturn
            };
            courseToReturn.LearningMaterials.Add(learningMaterial);
            _repoMock.Setup(c => c.GetLearningMaterial(It.IsAny<Guid>())).Returns(learningMaterial);
            var learningMaterialInfoReturn = _learningMaterialService.Get(courseToReturn.Id, learningMaterial.Id);
            Assert.That(learningMaterialInfoReturn.CourseId, Is.EqualTo(learningMaterial.Course.Id));
            Assert.That(learningMaterialInfoReturn.AssetId, Is.EqualTo(learningMaterial.AssetId));
            Assert.That(learningMaterialInfoReturn.Instruction, Is.EqualTo(learningMaterial.Instruction));
            Assert.That(learningMaterialInfoReturn.IsRequired, Is.EqualTo(learningMaterial.IsRequired));
        }

        [Test]
        public void Can_update_course_learning_material_to_course()
        {
            var learningMaterialRequest = new LearningMaterialRequest
            {
                AssetId = Guid.NewGuid(),
                Instruction = "test lm",
                IsRequired = false
            };
            var updateLearningMaterialRequest = new UpdateLearningMaterialRequest
             {
                 AssetId = Guid.NewGuid(),
                 Instruction = "test lm update",
                 IsRequired = true
             };
            var learningMaterial = _learningMaterialService.AddLearningMaterial(courseToReturn.Id, learningMaterialRequest);
            _learningMaterialService.UpdateLearningMaterial(courseToReturn.Id, learningMaterial.Id, updateLearningMaterialRequest);
            var updateLearningMaterial = courseToReturn.LearningMaterials.First(l => l.Id == learningMaterial.Id);
            Assert.That(updateLearningMaterial.Instruction, Is.EqualTo(updateLearningMaterialRequest.Instruction));
            Assert.That(updateLearningMaterial.AssetId, Is.EqualTo(updateLearningMaterialRequest.AssetId));
            Assert.That(updateLearningMaterial.IsRequired, Is.EqualTo(updateLearningMaterialRequest.IsRequired));
        }

        [Test]
        public void Can_delete_course_learning_material_from_course()
        {
            var learningMaterial = new LearningMaterial
            {
                Id = Guid.NewGuid(),
                AssetId = Guid.NewGuid(),
                Instruction = "test lm",
                IsRequired = false,
                ActiveFlag = true,
                Course = courseToReturn
            };
            courseToReturn.LearningMaterials.Add(learningMaterial);
            _repoMock.Setup(c => c.GetOrThrow(It.IsAny<Guid>())).Returns(courseToReturn);
            _repoMock.Setup(c => c.GetLearningMaterial(It.IsAny<Guid>())).Returns(learningMaterial);
            _learningMaterialService.Delete(courseToReturn.Id, learningMaterial.Id);

            var learningMaterialDetete = courseToReturn.LearningMaterials.First(l => l.Id == learningMaterial.Id);
            Assert.That(learningMaterialDetete.ActiveFlag, Is.EqualTo(false));
        }

    }
}
