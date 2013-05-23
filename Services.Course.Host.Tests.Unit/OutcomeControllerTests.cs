using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Repositories;
using BpeProducts.Services.Course.Host.App_Start;
using BpeProducts.Services.Course.Host.Controllers;
using Moq;
using NHibernate;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit
{
    [TestFixture]
    public class OutcomeControllerTests
    {
        private Mock<ILearningOutcomeRepository> _mockLearningOutcomeRepository;
        private OutcomeController _outcomeController;
		private Mock<IRepository> _repositoryOfObjecWithOutcomes;

	    [SetUp]
        public void SetUp()
        {
            _mockLearningOutcomeRepository = new Mock<ILearningOutcomeRepository>();
	        _repositoryOfObjecWithOutcomes = new Mock<IRepository>();
            _outcomeController = new OutcomeController(_mockLearningOutcomeRepository.Object,_repositoryOfObjecWithOutcomes.Object);

            var httpConfiguration = new HttpConfiguration();
            _outcomeController.Request = new HttpRequestMessage();
            _outcomeController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, httpConfiguration);

            MapperConfig.ConfigureMappers();
            WebApiConfig.Register(httpConfiguration);
        }

        [Test]
        public void Throws_NotFoundException_When_LearningOutcome_Is_Not_Found()
        {
            var statusCode =
                Assert.Throws<HttpResponseException>(() => _outcomeController.Get(Guid.NewGuid())).Response.StatusCode;
            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.NotFound));

            statusCode =
                Assert.Throws<HttpResponseException>(() => _outcomeController.Put(Guid.NewGuid(), new OutcomeRequest())).Response.StatusCode;
            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.NotFound));

            statusCode =
                            Assert.Throws<HttpResponseException>(() => _outcomeController.Delete(Guid.NewGuid())).Response.StatusCode;
            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void Add_New_LearningOutcome()
        {
            var outcomeRequest = new OutcomeRequest
                {
                    Description = "SomeDescription"
                };

            //_outcomeController.Request = new HttpRequestMessage(HttpMethod.Post, "http://server.com/foos");
            ////The line below was needed in WebApi RC as null config caused an issue after upgrade from Beta
            //_outcomeController.Configuration = new System.Web.Http.HttpConfiguration(new System.Web.Http.HttpRouteCollection());

            var response = _outcomeController.Post(outcomeRequest);

            _mockLearningOutcomeRepository.Verify(o => o.Add(It.Is<LearningOutcome>(x => x.Description == outcomeRequest.Description)));
        }

		[Test]
		public void Add_a_new_LearningOutcome_and_associate_with_a_program()
		{
			var outcomeRequest = new OutcomeRequest
			{
				Description = "SomeDescription"
			};
			var program = new Program
				{
					Id = Guid.NewGuid()
				};

			_repositoryOfObjecWithOutcomes.Setup(s => s.Query<IHaveOutcomes>()).Returns((new List<Program> { program }).AsQueryable());
			
			

			var response = _outcomeController.Post("program",program.Id,outcomeRequest);

			_mockLearningOutcomeRepository.Verify(o => o.Add(It.Is<LearningOutcome>(x => x.Description == outcomeRequest.Description)));
		}

        [Test]
        public void Return_Existing_LearningOutcome()
        {
            var learningOutcome1 = new LearningOutcome {Id = Guid.NewGuid(), Description = "SomeDescription", ActiveFlag = true};
            var learningOutcome2 = new LearningOutcome
                {
                    Id = Guid.NewGuid(),
                    Description = "OtherDescription",
                    ActiveFlag = false
                };
            var learningOutcomes = new List<LearningOutcome> {learningOutcome1, learningOutcome2};
            _mockLearningOutcomeRepository.Setup(o => o.GetAll()).Returns(learningOutcomes.AsQueryable);

            var outcomeResponse = _outcomeController.Get(learningOutcome1.Id);

            _mockLearningOutcomeRepository.Verify(o => o.GetAll(), Times.Once());
            Assert.That(outcomeResponse.Description, Is.EqualTo(learningOutcome1.Description));
			
        }

        [Test]
        public void Updates_Existing_LearningOutcome()
        {
            var learningOutcome = new LearningOutcome { Description = "SomeDescription" };
            _mockLearningOutcomeRepository.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(learningOutcome);

            var outcomeRequest = new OutcomeRequest {Description = "OtherDescription"};

            _outcomeController.Put(Guid.NewGuid(), outcomeRequest);

            _mockLearningOutcomeRepository.Verify(o => o.GetById(It.IsAny<Guid>()));
            _mockLearningOutcomeRepository.Verify(o => o.Update(It.Is<LearningOutcome>(x => x.Description == outcomeRequest.Description)));
        }

        [Test]
        public void Soft_Delete_Existing_LearningOutcome()
        {
            var learningOutcome = new LearningOutcome { Description = "SomeDescription" };
            _mockLearningOutcomeRepository.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(learningOutcome);

            _outcomeController.Delete(Guid.NewGuid());

            _mockLearningOutcomeRepository.Verify(o => o.GetById(It.IsAny<Guid>()));
            _mockLearningOutcomeRepository.Verify(o => o.Update(It.Is<LearningOutcome>(x => x.ActiveFlag == false)));
        }
    }
}
