using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BpeProducts.Common.NHibernate.Version;
using BpeProducts.Common.WebApi.Attributes;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;

namespace BpeProducts.Services.Course.Host.Controllers
{
    public class VersionController : ApiController
    {
        private readonly IVersionHandler _versionHandler;

        public VersionController(IVersionHandler versionHandler)
        {
            _versionHandler = versionHandler;
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [HttpPost]
        public HttpResponseMessage CreateVersion(string entityType, VersionRequest request)
        {
            var entityTypeName = entityType == "outcome" ? "learningoutcome" : entityType;

            var entity = _versionHandler.CreateVersion(entityTypeName, request.ParentVersionId, request.VersionNumber);
            HttpResponseMessage response = base.Request.CreateResponse(HttpStatusCode.Created);

            string uri = Url.Link("DefaultApi", new { controller = entityType, id = entity.Id });
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
        public void PublishVersion(string entityType, Guid id, PublishRequest request)
        {
            var entityTypeName = entityType == "outcome" ? "learningoutcome" : entityType;
            _versionHandler.PublishVersion(entityTypeName, id, request.PublishNote);
        }
    }
}
