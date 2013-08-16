using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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

        // courses/<courseId>/segments
        [Transaction]
        public HttpResponseMessage Post(Guid courseId, SaveCourseSegmentRequest saveCourseSegmentRequest)
        {
            var courseSegment = _courseSegmentService.Create(courseId, saveCourseSegmentRequest);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
            var newSegmentId = courseSegment.Id;
            string uri = Url.Link("CourseSegments", new { segmentId = newSegmentId });
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }

            return response;
        }

        // courses/<courseId>/segments/<segmentId>
        [Transaction]
        public void Put(Guid courseId, Guid segmentId, SaveCourseSegmentRequest saveCourseSegmentRequest)
        {
            _courseSegmentService.Update(courseId, segmentId, saveCourseSegmentRequest);
        }

        // courses/<courseId>/segments/<segmentId>
        [Transaction]
        public void Delete(Guid courseId, Guid segmentId)
        {
            _courseSegmentService.Delete(courseId, segmentId);
        }

        // courses/<courseId>/segments
        public IEnumerable<CourseSegmentInfo> Get(Guid courseId)
        {
            // returns the root course segments, including their childrent segments
            return _courseSegmentService.Get(courseId);
        }

        // courses/<courseId>/segments/<segmentId>
        public CourseSegmentInfo Get(Guid courseId, Guid segmentId)
        {
            return _courseSegmentService.Get(courseId, segmentId);
        }
    }
}