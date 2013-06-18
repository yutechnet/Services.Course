using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Repositories;
using BpeProducts.Services.Course.Host.Controllers;
using Moq;
using NHibernate;
using NHibernate.Linq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit
{
   
	[TestFixture]
	public class ProgramControllerTests
	{
		private ProgramsController _programsController;
		private Mock<IRepository> _repository;
		private Guid[] _programIds;
        private Guid[] _organizationIds;
	    private string[] _programTypes;

		[SetUp]
		public void SetUp()
		{

			_programIds =  new[]{Guid.NewGuid(),Guid.NewGuid(),Guid.NewGuid()};
            _organizationIds = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
		    _programTypes = new string[] {"MA", "BA", "AA"};
			_repository=new Mock<IRepository>();
			var programs = new List<Program>
				{
					new Program
						{ 
							Id=_programIds[0],
                            OrganizationId = _organizationIds[0],
                            ProgramType = _programTypes[0],
							ActiveFlag = true
						},
					new Program
						{
							Id=_programIds[1],
                            OrganizationId = _organizationIds[1],
                            ProgramType = _programTypes[1],
							ActiveFlag = false
						},
					new Program
						{
							Id=_programIds[2],
                            OrganizationId = _organizationIds[2],
                            ProgramType = _programTypes[2],
							ActiveFlag = false
						}
				}.AsQueryable();

		    
            _programsController = new ProgramsController(_repository.Object);
		}

		[TearDown]
		public void TearDown()
		{
			
		}

		[Test,Ignore("Ranji will fix this")]
		public void Can_get_program_by_id()
		{
			var response = _programsController.Get(_programIds[0]);
			Assert.That(response.Id,Is.EqualTo(_programIds[0]));
		}




	}
}

