using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Autofac.Extras.Moq;
using AutoMapper;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.CourseAggregates;
using BpeProducts.Services.Course.Domain.ProgramAggregates;
using Castle.Core.Internal;
using Moq;
using NUnit.Framework;
using Services.Assessment.Contract;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Entities
{
    public class ProgramTests
    {
        private Program _program;
        private Domain.CourseAggregates.Course _course;

        [SetUp]
        public void SetUp()
        {
            _program=new Program(){};
            Mapper.CreateMap<UpdateProgramRequest, Program>();
        }


        [Test]
        public void Can_update_program()
        {
            var updateProgramRequest = new UpdateProgramRequest()
                {
                    Description="des modified",
                    GraduationRequirements="new requirements",
                    Name="new name",
                    OrganizationId=Guid.NewGuid(),
                    ProgramType="new type"
                };
           _program.Update(updateProgramRequest);
           Assert.That(_program.Description, Is.EqualTo(updateProgramRequest.Description));
           Assert.That(_program.GraduationRequirements, Is.EqualTo(updateProgramRequest.GraduationRequirements));
           Assert.That(_program.Name, Is.EqualTo(updateProgramRequest.Name));
           Assert.That(_program.ProgramType, Is.EqualTo(updateProgramRequest.ProgramType));
           Assert.That(_program.OrganizationId, Is.EqualTo(updateProgramRequest.OrganizationId));
        }

        [Test]
        public void Can_not_update_program_published()
        {
            var updateProgramRequest = new UpdateProgramRequest()
            {
                Description = "des modified",
                GraduationRequirements = "new requirements",
                Name = "new name",
                OrganizationId = Guid.NewGuid(),
                ProgramType = "new type"
            };
            _program.Publish("hello");
            Assert.Throws<BadRequestException>(()=>_program.Update(updateProgramRequest));
        }

        [Test]
        public void Can_delete_program()
        {
            _program.Delete();
            Assert.That(_program.IsDeleted, Is.EqualTo(true));
        }

        [Test]
        public void Can_not_delete_program_published()
        {
            _program.Publish("hello");
            Assert.Throws<BadRequestException>(() => _program.Delete());
        }
    }
}
