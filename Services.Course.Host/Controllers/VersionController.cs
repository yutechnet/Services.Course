//using System;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using BpeProducts.Common.WebApi.NHibernate;
//using BpeProducts.Common.WebApi.Validation;
//using BpeProducts.Services.Course.Contract;
//using BpeProducts.Services.Course.Domain;

//namespace BpeProducts.Services.Course.Host.Controllers
//{
//    public class VersionController : ApiController
//    {
//        private readonly IVersionHandler _versionHandler;

//        public VersionController(IVersionHandler versionHandler)
//        {
//            _versionHandler = versionHandler;
//        }

//        [Transaction]
//        [ArgumentsNotNull]
//        [ValidateModelState]
//        [HttpPost]
//        [Route("{entityType}/version", Name = "CreateVersion")]
//        public HttpResponseMessage CreateVersion(string entityType, VersionRequest request)
//        {
//            var entityTypeName = entityType == "outcome" ? "learningoutcome" : entityType;

//            var entity = _versionHandler.CreateVersion(entityTypeName, request.ParentVersionId, request.VersionNumber);
//            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

//            string uri = Url.Link("CreateVersion", new { entityType });
//            if (uri != null)
//            {
//                uri = uri.Replace("version", entity.Id.ToString());
//                response.Headers.Location = new Uri(uri);
//            }
//            return response;
//        }

//        [Transaction]
//        [ArgumentsNotNull]
//        [ValidateModelState]
//        [HttpPut]
//        [Route("{entityType}/{entityId:guid}/publish")]
//        public void PublishVersion(string entityType, Guid entityId, PublishRequest request)
//        {
//            var entityTypeName = entityType == "outcome" ? "learningoutcome" : entityType;
//            _versionHandler.PublishVersion(entityTypeName, entityId, request.PublishNote);
//        }
//    }
//}
