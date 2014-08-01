using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BpeProducts.Common.Ioc.Extensions;
using BpeProducts.Common.WebApi.NHibernate;
using BpeProducts.Common.WebApi.Validation;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.CourseAggregates;

namespace BpeProducts.Services.Course.Host.Controllers
{
    [Authorize]
    public class LearningMaterialController : ApiController
    {
        private readonly ILearningMaterialService _learningMaterialService;
        private readonly string _regExPattern = ConfigurationManager.AppSettings["Regex.Course"];
        private readonly string _rewriteUrl = ConfigurationManager.AppSettings["RewriteUrl.Course"];

        public LearningMaterialController(ILearningMaterialService learningMaterialService)
        {
            _learningMaterialService = learningMaterialService;
        }

        [Transaction]
        [ArgumentsNotNull]
        [ValidateModelState]
        [Route("course/{courseId:guid}/learningmaterial")]
        public HttpResponseMessage Post(Guid courseId, LearningMaterialRequest request)
        {
            var learningMaterial = _learningMaterialService.AddLearningMaterial(courseId, request);

            var response = Request.CreateResponse(HttpStatusCode.Created, learningMaterial);

            var uri = Url.Link("GetCourseLearningMaterial", new { courseId, learningMaterialId = learningMaterial.Id });
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
        [Route("course/{courseId:guid}/learningmaterial/{learningMaterialId:guid}")]
        public void Put(Guid courseId, Guid learningMaterialId, UpdateLearningMaterialRequest request)
        {
            _learningMaterialService.UpdateLearningMaterial(courseId, learningMaterialId, request);
        }


        [Route("course/{courseId:guid}/learningmaterial/{learningMaterialId:guid}", Name = "GetCourseLearningMaterial")]
        public LearningMaterialInfo Get(Guid courseId, Guid learningMaterialId)
        {
            return _learningMaterialService.Get(courseId, learningMaterialId);
        }

        [Transaction]
        [Route("course/{courseId:guid}/learningmaterial/{learningMaterialId:guid}")]
        public void Delete(Guid courseId, Guid learningMaterialId)
        {
            _learningMaterialService.Delete(courseId, learningMaterialId);
        }

        [Transaction]
        [ArgumentsNotNull]
        [ValidateModelState]
        [Route("course/{courseId:guid}/segment/{segmentId:guid}/learningmaterial")]
        public HttpResponseMessage Post(Guid courseId, Guid segmentId, LearningMaterialRequest request)
        {
            var learningMaterial = _learningMaterialService.AddLearningMaterial(courseId, segmentId, request);

            var response = Request.CreateResponse(HttpStatusCode.Created, learningMaterial);

            var uri = Url.Link("GetLearningMaterial", new { courseId, segmentId, learningMaterialId = learningMaterial.Id });
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
        [Route("course/{courseId:guid}/segment/{segmentId:guid}/learningmaterial/{learningMaterialId:guid}")]
        public void Put(Guid courseId, Guid segmentId, Guid learningMaterialId, UpdateLearningMaterialRequest request)
        {
            _learningMaterialService.UpdateLearningMaterial(courseId, segmentId, learningMaterialId, request);
        }


        [Route("course/{courseId:guid}/segment/{segmentId:guid}/learningmaterial/{learningMaterialId:guid}", Name = "GetLearningMaterial")]
        public LearningMaterialInfo Get(Guid courseId, Guid segmentId, Guid learningMaterialId)
        {
            return _learningMaterialService.Get(courseId, segmentId, learningMaterialId);
        }

        [Transaction]
        [Route("course/{courseId:guid}/segment/{segmentId:guid}/learningmaterial/{learningMaterialId:guid}")]
        public void Delete(Guid courseId, Guid segmentId, Guid learningMaterialId)
        {
            _learningMaterialService.Delete(courseId, segmentId, learningMaterialId);
        }


    }
}
