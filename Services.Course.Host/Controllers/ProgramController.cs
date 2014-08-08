using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BpeProducts.Common.Contract;
using BpeProducts.Common.Ioc.Extensions;
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
        private readonly string _regExPattern = ConfigurationManager.AppSettings["Regex.Course"];
        private readonly string _rewriteUrl = ConfigurationManager.AppSettings["RewriteUrl.Course"];

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
            return _programService.Get(programId);
		}

		[Transaction]
        [ArgumentsNotNull]
		[ValidateModelState]
        [Route("program")]
		public HttpResponseMessage Post(SaveProgramRequest request)
		{
		    var programResponse = _programService.Create(request);
            var response = Request.CreateResponse(HttpStatusCode.Created, programResponse);
            var uri = Url.Link("GetProgram", new { programId = programResponse.Id });
            uri = uri.UrlRewrite(_regExPattern, _rewriteUrl);

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

        [Transaction]
        [ArgumentsNotNull]
        [ValidateModelState]
        [HttpPost]
        [Route("program/version", Name = "CreateProgramVersion")]
        public HttpResponseMessage CreateVersion(VersionRequest request)
        {
            var programResponse = _programService.CreateVersion(request.ParentVersionId, request.VersionNumber); 
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

            string uri = Url.Link("GetProgram", new { programId = programResponse.Id });
            uri = uri.UrlRewrite(_regExPattern, _rewriteUrl);

            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
            return response;
        }

        [Transaction]
        [ArgumentsNotNull]
        [ValidateModelState]
        [HttpPut]
        [Route("program/{programId:guid}/publish")]
        public void PublishVersion(Guid programId, PublishRequest request)
        {
            _programService.PublishVersion(programId, request.PublishNote);
        }

        [Transaction]
        [ArgumentsNotNull]
        [ValidateModelState]
        [HttpPut]
        [Route("program/{programId:guid}/activate")]
        public void UpdateActiviationStatus(Guid programId, ActivationRequest request)
        {
            _programService.UpdateActiviationStatus(programId, request);
        }
	}
}

