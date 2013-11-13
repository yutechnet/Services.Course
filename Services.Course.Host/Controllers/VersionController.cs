using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting.Web.Http;
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
        [POST("{entityType}/version", RouteName = "CreateVersion")]
        public HttpResponseMessage CreateVersion(string entityType, VersionRequest request)
        {
            var entityTypeName = entityType == "outcome" ? "learningoutcome" : entityType;

            var entity = _versionHandler.CreateVersion(entityTypeName, request.ParentVersionId, request.VersionNumber);
            HttpResponseMessage response = base.Request.CreateResponse(HttpStatusCode.Created);

            string uri = Url.Link("CreateVersion", new { entityType });
            if (uri != null)
            {
                uri = uri.Replace("version", entity.Id.ToString());
                response.Headers.Location = new Uri(uri);
            }
            return response;
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [HttpPut]
        [PUT("{entityType}/{entityId:guid}/publish")]
        public void PublishVersion(string entityType, Guid entityId, PublishRequest request)
        {
            var entityTypeName = entityType == "outcome" ? "learningoutcome" : entityType;
            _versionHandler.PublishVersion(entityTypeName, entityId, request.PublishNote);
        }
    }
}
