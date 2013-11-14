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
        [HttpPost]
        [POST("course/{courseId:guid}/segments/{segmentId:guid}/learningactivity/{learningActivityId:guid}/learningmaterial")]
        public HttpResponseMessage Post(Guid courseId, Guid segmentId, Guid learningActivityId, LearningMaterialRequest request)
        {
            var learningMaterial = _learningMaterialService.AddLearningMaterial(courseId, segmentId, learningActivityId, request);

            var response = Request.CreateResponse(HttpStatusCode.Created, learningMaterial);

            var uri = Url.Link("GetLearningMaterial", new { courseId, segmentId, learningActivityId, learningMaterialId = learningMaterial.Id });
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
            return response;
        }

        [HttpGet]
        [GET("course/{courseId:guid}/segments/{segmentId:guid}/learningactivity/{learningActivityId:guid}/learningmaterial/{learningMaterialId:guid}", RouteName = "GetLearningMaterial")]
        public LearningMaterialInfo Get(Guid courseId, Guid segmentId, Guid learningActivityId, Guid learningMaterialId)
        {
            return _learningMaterialService.Get(courseId, segmentId, learningActivityId, learningMaterialId);
        }
    }
}
