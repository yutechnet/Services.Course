using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BpeProducts.Common.WebApi.NHibernate;
using BpeProducts.Common.WebApi.Validation;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;

namespace BpeProducts.Services.Course.Host.Controllers
{
	[Authorize]
	public class ProgramController : ApiController
	{
	    private readonly IProgramService _programService;

	    public ProgramController(IProgramService programService)
		{
		    _programService = programService;
		}

        [Route("program")]
        public IEnumerable<ProgramResponse> Get()
        {
            return _programService.Search(Request.RequestUri.Query);
        }

        [Route("program/{programId:guid}", Name = "GetProgram")]
        public ProgramResponse Get(Guid programId)
		{
            return _programService.Search(programId);
		}

		[Transaction]
        [ArgumentsNotNull]
		[ValidateModelState]
        [Route("program")]
		public HttpResponseMessage Post(SaveProgramRequest request)
		{
		    var programResponse = _programService.Create(request);
			var response = Request.CreateResponse(HttpStatusCode.Created);
            var uri = Url.Link("GetProgram", new { programId = programResponse.Id });
			if (uri != null)
			{
				response.Headers.Location = new Uri(uri);
			}
			return response;
		}

		[Transaction]
        [ArgumentsNotNull]
		[ValidateModelState]
        [Route("program/{programId:guid}")]
        public HttpResponseMessage Put(Guid programId, UpdateProgramRequest request)
		{
            _programService.Update(programId, request);
			var response = Request.CreateResponse(HttpStatusCode.OK);
			return response;
		}

		[Transaction]
        [Route("program/{programId:guid}")]
        public void Delete(Guid programId)
		{
            _programService.Delete(programId);
		}
	}
}

