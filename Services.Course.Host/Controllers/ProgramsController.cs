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
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Repositories;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.OData;

namespace BpeProducts.Services.Course.Host.Controllers
{
	[Authorize]
	public class ProgramsController : ApiController
	{
	    private readonly IProgramService _programService;

	    public ProgramsController(IProgramService programService)
		{
		    _programService = programService;
		}

	    // GET api/programs
		public IEnumerable<ProgramResponse> Get(ODataQueryOptions options)
		{
		    return _programService.Search(Request.RequestUri.Query);
		}

		// GET api/programs/5
		public ProgramResponse Get(Guid id)
		{
		    return _programService.Search(id);
		}

		[Transaction]
		[CheckModelForNull]
		[ValidateModelState]
		// POST api/programs
		public HttpResponseMessage Post(SaveProgramRequest request)
		{
		    var programResponse = _programService.Create(request);
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
            _programService.Update(id, request);
			var response = Request.CreateResponse(HttpStatusCode.OK);
			return response;
		}

		[Transaction]
		// DELETE api/programs/5
		public void Delete(Guid id)
		{
            _programService.Delete(id);
		}
	}
}

