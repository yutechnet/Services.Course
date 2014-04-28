using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BpeProducts.Common.WebApi.NHibernate;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.CourseAggregates;

namespace BpeProducts.Services.Course.Host.Controllers
{
    [Authorize]
    public class SegmentController : ApiController
    {
        private readonly ICourseSegmentService _courseSegmentService;

        public SegmentController(ICourseSegmentService courseSegmentService)
        {
            _courseSegmentService = courseSegmentService;
        }
      
        [Transaction]
        [Route("course/{courseId:guid}/segment")]
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
        [Route("course/{courseId:guid}/segment/{segmentId:guid}")]
        public void Put(Guid courseId, Guid segmentId, SaveCourseSegmentRequest saveCourseSegmentRequest)
        {
            _courseSegmentService.Update(courseId, segmentId, saveCourseSegmentRequest);
        }

        [Transaction]
        [Route("course/{courseId:guid}/segment")]
        public void Put(Guid courseId, IList<UpdateCourseSegmentRequest> updateCourseSegmentRequest)
        {
            _courseSegmentService.Update(courseId, updateCourseSegmentRequest);
        }

        [Transaction]
        [Route("course/{courseId:guid}/segment/{segmentId:guid}")]
        public void Delete(Guid courseId, Guid segmentId)
        {
            _courseSegmentService.Delete(courseId, segmentId);
        }

        [Route("course/{courseId:guid}/segment")]
        public IEnumerable<CourseSegmentInfo> Get(Guid courseId)
        {
            // returns the root course segments, including their childrent segments
            return _courseSegmentService.Get(courseId);
        }

        [Route("course/{courseId:guid}/segment/{segmentId:guid}", Name = "GetSegment")]
        public CourseSegmentInfo Get(Guid courseId, Guid segmentId)
        {
            return _courseSegmentService.Get(courseId, segmentId);
        }
    }
}