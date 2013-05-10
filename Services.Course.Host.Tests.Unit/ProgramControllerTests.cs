using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Host.Controllers;
using Moq;
using NHibernate;
using NHibernate.Linq;
using NUnit.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Unit
{
    public static class SessionExtension
    {
        public static IQueryable Result; 
        public static IQueryable Query<T>(this ISession session)
        {
            return Result;
        }
    }
	[TestFixture]
	public class ProgramControllerTests
	{
		private ProgramsController _programsController;
		private Mock<ISession> _session;
		private Guid[] _programIds;

		[SetUp]
		public void SetUp()
		{

			_programIds =  new[]{Guid.NewGuid(),Guid.NewGuid(),Guid.NewGuid()};
			_session=new Mock<ISession>();
			var programs = new List<Program>
				{
					new Program
						{ 
							Id=_programIds[0],
							ActiveFlag = true
						},
					new Program
						{
							Id=_programIds[1],
							ActiveFlag = false
						},
					new Program
						{
							Id=_programIds[2],
							ActiveFlag = false
						}
				}.AsQueryable();

		    SessionExtension.Result = programs;
            _programsController = new ProgramsController(_session.Object);
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
