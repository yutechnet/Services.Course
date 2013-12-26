using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using AutoMapper;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Courses;
using BpeProducts.Services.Course.Domain.Entities;
using EventStore;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Entities
{
    [TestFixture]
    public class CourseSegmentTests
    {
        [SetUp]
        public void SetUp()
        {
            Mapper.CreateMap<LearningMaterialRequest, Domain.Courses.LearningMaterial>();
            Mapper.CreateMap<UpdateLearningMaterialRequest, Domain.Courses.LearningMaterial>();
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
            courseSegment.UpdateLearningMaterial(learningMaterial.Id,updateLearningMaterialRequest);
            Assert.That(learningMaterial.AssetId, Is.EqualTo(updateAssetId));
            Assert.That(learningMaterial.Instruction, Is.EqualTo(updateInstruction));
            Assert.That(learningMaterial.IsRequired, Is.EqualTo(updateIsRequired));
          
        }

    }
}
