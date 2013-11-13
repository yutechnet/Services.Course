using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData.Query;
using AttributeRouting.Web.Http;
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
	public class ProgramController : ApiController
	{
	    private readonly IProgramService _programService;

	    public ProgramController(IProgramService programService)
		{
		    _programService = programService;
		}

        [HttpGet]
        [GET("program")]
        public IEnumerable<ProgramResponse> GetPrograms()
        {
            return _programService.Search(Request.RequestUri.Query);
        }

        [HttpGet]
        [GET("program?{$filter}")]
        public IEnumerable<ProgramResponse> SearchPrograms()
        {
            return _programService.Search(Request.RequestUri.Query);
        }

        [HttpGet]
        [GET("program/{programId:guid}", RouteName = "GetProgram")]
        public ProgramResponse Get(Guid programId)
		{
            return _programService.Search(programId);
		}

		[Transaction]
		[CheckModelForNull]
		[ValidateModelState]
        [HttpPost]
        [POST("program")]
		public HttpResponseMessage Post(SaveProgramRequest request)
		{
		    var programResponse = _programService.Create(request);
			var response = Request.CreateResponse(HttpStatusCode.Created, programResponse);
            var uri = Url.Link("GetProgram", new { programId = programResponse.Id });
			if (uri != null)
			{
				response.Headers.Location = new Uri(uri);
			}
			return response;
		}

		[Transaction]
		[CheckModelForNull]
		[ValidateModelState]
        [HttpPut]
        [PUT("program/{programId:guid}")]
        public HttpResponseMessage Put(Guid programId, UpdateProgramRequest request)
		{
            _programService.Update(programId, request);
			var response = Request.CreateResponse(HttpStatusCode.OK);
			return response;
		}

		[Transaction]
        [HttpDelete]
        [DELETE("program/{programId:guid}")]
        public void Delete(Guid programId)
		{
            _programService.Delete(programId);
		}
	}
}

