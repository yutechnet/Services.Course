using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting.Web.Http;
using BpeProducts.Common.WebApi.Attributes;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Courses;

namespace BpeProducts.Services.Course.Host.Controllers
{
    [Authorize]
    public class LearningMaterialController : ApiController
    {
        private readonly ILearningMaterialService _learningMaterialService;

        public LearningMaterialController(ILearningMaterialService learningMaterialService)
        {
            _learningMaterialService = learningMaterialService;
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [SetSamlTokenInBootstrapContext]
        [HttpPost]
        [POST("course/{courseId:guid}/segments/{segmentId:guid}/learningmaterial")]
        public HttpResponseMessage Post(Guid courseId, Guid segmentId, LearningMaterialRequest request)
        {
            var learningMaterial = _learningMaterialService.AddLearningMaterial(courseId, segmentId, request);

            var response = Request.CreateResponse(HttpStatusCode.Created, learningMaterial);

            var uri = Url.Link("GetLearningMaterial", new { courseId, segmentId, learningMaterialId = learningMaterial.Id });
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
            return response;
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [SetSamlTokenInBootstrapContext]
        [HttpPut]
        [PUT("course/{courseId:guid}/segments/{segmentId:guid}/learningmaterial/{learningMaterialId:guid}")]
        public void Put(Guid courseId, Guid segmentId, Guid learningMaterialId, UpdateLearningMaterialRequest request)
        {
            _learningMaterialService.UpdateLearningMaterial(courseId, segmentId, learningMaterialId, request);

        }

        [HttpGet]
        [GET("course/{courseId:guid}/segments/{segmentId:guid}/learningmaterial/{learningMaterialId:guid}", RouteName = "GetLearningMaterial")]
        public LearningMaterialInfo Get(Guid courseId, Guid segmentId, Guid learningMaterialId)
        {
            return _learningMaterialService.Get(courseId, segmentId,learningMaterialId);
        }

        [Transaction]
        [HttpDelete]
        [DELETE("course/{courseId:guid}/segments/{segmentId:guid}/learningmaterial/{learningMaterialId:guid}")]
        public void Delete(Guid courseId, Guid segmentId,Guid learningMaterialId)
        {
            _learningMaterialService.Delete(courseId, segmentId,learningMaterialId);
        }
    }
}
