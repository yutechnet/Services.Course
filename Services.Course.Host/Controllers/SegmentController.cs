using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting.Web.Http;
using BpeProducts.Common.WebApi.Attributes;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;

namespace BpeProducts.Services.Course.Host.Controllers
{
    //    [DefaultHttpRouteConvention]
    [Authorize]
    public class SegmentController : ApiController
    {
        private readonly ICourseSegmentService _courseSegmentService;

        public SegmentController(ICourseSegmentService courseSegmentService)
        {
            _courseSegmentService = courseSegmentService;
        }
      
        [Transaction]
        [HttpPost]
        [POST("course/{courseId:guid}/segments")]
        public HttpResponseMessage Post(Guid courseId, SaveCourseSegmentRequest saveCourseSegmentRequest)
        {
            var courseSegment = _courseSegmentService.Create(courseId, saveCourseSegmentRequest);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
            var newSegmentId = courseSegment.Id;
            string uri = Url.Link("GetSegment", new { segmentId = newSegmentId });
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }

            return response;
        }

        [Transaction]
        [HttpPut]
        [PUT("course/{courseId:guid}/segments/{segmentId:guid}")]
        public void Put(Guid courseId, Guid segmentId, SaveCourseSegmentRequest saveCourseSegmentRequest)
        {
            _courseSegmentService.Update(courseId, segmentId, saveCourseSegmentRequest);
        }

        [Transaction]
        [HttpPut]
        [PUT("course/{courseId:guid}/segments")]
        public void Put(Guid courseId, IList<UpdateCourseSegmentRequest> updateCourseSegmentRequest)
        {
            _courseSegmentService.Update(courseId, updateCourseSegmentRequest);
        }

        [Transaction]
        [HttpDelete]
        [DELETE("course/{courseId:guid}/segments/{segmentId:guid}")]
        public void Delete(Guid courseId, Guid segmentId)
        {
            _courseSegmentService.Delete(courseId, segmentId);
        }

        [HttpGet]
        [GET("course/{courseId:guid}/segments")]
        public IEnumerable<CourseSegmentInfo> Get(Guid courseId)
        {
            // returns the root course segments, including their childrent segments
            return _courseSegmentService.Get(courseId);
        }

        [HttpGet]
        [GET("course/{courseId:guid}/segments/{segmentId:guid}", RouteName = "GetSegment")]
        public CourseSegmentInfo Get(Guid courseId, Guid segmentId)
        {
            return _courseSegmentService.Get(courseId, segmentId);
        }
    }
}