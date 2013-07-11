using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using BpeProducts.Common.WebApi.Attributes;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Outcomes;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Host.Controllers
{
    public class OutcomeVersionController : ApiController
    {
        private readonly IHandleVersioning<LearningOutcome> _versionHandler;

        public OutcomeVersionController(IHandleVersioning<Domain.Entities.LearningOutcome> versionHandler)
        {
            _versionHandler = versionHandler;
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [HttpPut]
        public void PublishVersion(Guid id, PublishRequest request)
        {
            _versionHandler.PublishVersion(id, request.PublishNote);
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [HttpPost]
        public HttpResponseMessage CreateVersion(VersionRequest request)
        {
            var newVersion = _versionHandler.CreateVersion(request);
            var outcomeResponse = Mapper.Map<OutcomeResponse>(newVersion);
            HttpResponseMessage response = base.Request.CreateResponse(HttpStatusCode.Created, outcomeResponse);

            string uri = Url.Link("DefaultApi", new { controller = "outcome", id = outcomeResponse.Id });
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
            return response;
        }

    }
}
