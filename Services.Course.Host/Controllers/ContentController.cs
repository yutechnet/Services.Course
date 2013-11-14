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
    public class ContentController : ApiController
    {
        private readonly IContentService _contentService;

        public ContentController(IContentService contentService)
        {
            _contentService = contentService;
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [HttpPost]
        [POST("course/{courseId:guid}/segments/{segmentId:guid}/learningactivity/{learningActivityId:guid}/content")]
        public HttpResponseMessage Post(Guid courseId, Guid segmentId, Guid learningActivityId, ContentRequest request)
        {
            var content = _contentService.AddContent(courseId, segmentId, learningActivityId, request);

            var response = Request.CreateResponse(HttpStatusCode.Created, content);

            var uri = Url.Link("GetContent", new { courseId, segmentId, learningActivityId, contentId = content.Id });
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
            return response;
        }

        [HttpGet]
        [GET("course/{courseId:guid}/segments/{segmentId:guid}/learningactivity/{learningActivityId:guid}/content/{contentId:guid}", RouteName = "GetContent")]
        public LearningMaterialInfo Get(Guid courseId, Guid segmentId, Guid learningActivityId, Guid contentId)
        {
            return _contentService.Get(courseId, segmentId, learningActivityId, contentId);
        }
    }
}
