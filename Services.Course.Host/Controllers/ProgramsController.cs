using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData.Query;
using AutoMapper;
using BpeProducts.Common.WebApi.Attributes;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.OData;

namespace BpeProducts.Services.Course.Host.Controllers
{
	[Authorize]
	public class ProgramsController : ApiController
	{
		private ISession _session;
		public ProgramsController(ISession session)
		{
			_session = session;
		}

		// GET api/programs
		public IEnumerable<ProgramResponse> Get(ODataQueryOptions options)
		{
			var queryString = Request.RequestUri.Query.Split('?');
			ICriteria criteria = _session.ODataQuery<Program>(queryString.Length>1?queryString[1]:"");
			criteria.Add(Expression.Eq("ActiveFlag", true));
			var programs = criteria.List<Program>();
			var programResponses = new List<ProgramResponse>();
			Mapper.Map(programs, programResponses);
			return programResponses;
		}

		// GET api/programs/5
		public ProgramResponse Get(Guid id)
		{
            var program = _session.Query<Program>().SingleOrDefault(p => p.ActiveFlag && p.Id == id);
			if (program == null)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound);
			}
			var programResponse = new ProgramResponse();
			Mapper.Map(program, programResponse);
			return programResponse;
		}

		[Transaction]
		[CheckModelForNull]
		[ValidateModelState]
		// POST api/programs
		public HttpResponseMessage Post(SaveProgramRequest request)
		{
			var program = Mapper.Map<Program>(request);
			_session.Save(program);
			var programResponse = Mapper.Map<ProgramResponse>(program);
			var response = Request.CreateResponse(HttpStatusCode.Created, programResponse);
			var uri = Url.Link("DefaultApi", new { id = programResponse.Id });
			if (uri != null)
			{
				response.Headers.Location = new Uri(uri);
			}
			return response;
		}

		[Transaction]
		[CheckModelForNull]
		[ValidateModelState]
		// PUT api/programs/5
		public HttpResponseMessage Put(Guid id, SaveProgramRequest request)
		{
			// We do not allow creation of a new resource by PUT.
			var programInDb = _session.Get<Program>(id);

			if (programInDb == null)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound);
			}

			Mapper.Map(request, programInDb);
			_session.Update(programInDb);
			var response = Request.CreateResponse(HttpStatusCode.OK);
			return response;
		}

		[Transaction]
		// DELETE api/programs/5
		public void Delete(Guid id)
		{
			try
			{
				var programInDb = _session.Load<Program>(id);
				// logical delete
				programInDb.ActiveFlag = false;
				_session.Update(programInDb);
			}
			catch (Exception)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound);
			}
		}
	}
}

