using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.OData.Query;
using System.Xml.Linq;
using AutoMapper;
using BpeProducts.Common.WebApi.Attributes;
using BpeProducts.Common.WebApi.Authorization;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using NHibernate;
using NHibernate.Criterion;

namespace BpeProducts.Services.Course.Host.Controllers
{
    //    [DefaultHttpRouteConvention]
    [Authorize]
    public class CourseController : ApiController
    {
        private readonly ICourseService _courseService;
        private readonly ICourseSegmentService _courseSegmentService;

        public CourseController(ICourseService courseService, ICourseSegmentService courseSegmentService)
        {
            _courseService = courseService;
            _courseSegmentService = courseSegmentService;
        }

        // GET api/programs
        public IEnumerable<CourseInfoResponse> Get(ODataQueryOptions options)
        {
            return _courseService.Search(Request.RequestUri.Query);
        }

        // GET api/courses/5
        public CourseInfoResponse Get(Guid id)
        {
            return _courseService.Get(id);
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        // POST api/courses
        public HttpResponseMessage Post(SaveCourseRequest request)
        {
            var courseInfoResponse = _courseService.Create(request);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, courseInfoResponse);

            string uri = Url.Link("DefaultApi", new {id = courseInfoResponse.Id});
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
            return response;
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        // PUT api/courses/5
        public void Put(Guid id, SaveCourseRequest request)
        {
            _courseService.Update(id, request);
        }

        [Transaction]
        // DELETE api/courses/5
        public void Delete(Guid id)
        {
            _courseService.Delete(id);
        }

        #region Course Segment Management

        // courses/<courseId>/segments
        [HttpGet]
        public IEnumerable<CourseSegmentInfo> Segments(Guid courseId)
        {
            // returns the root course segments, including their childrent segments
            return Get(courseId).Segments;
        }

        // courses/<courseId>/segments
        [HttpPost]
        [Transaction]
        public HttpResponseMessage Segments(Guid courseId, SaveCourseSegmentRequest saveCourseSegmentRequest)
        {
            var courseSegment = _courseSegmentService.Create(courseId, saveCourseSegmentRequest);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
            var newSegmentId = courseSegment.Id;
            string uri = Url.Link("CourseSegmentsApi", new {segmentId = newSegmentId});
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }

            return response;

        }

        // courses/<courseId>/segments/<segmentId>
        [HttpPut]
        [Transaction]
        public void Segments(Guid courseId, Guid segmentId, SaveCourseSegmentRequest saveCourseSegmentRequest)
        {
            _courseSegmentService.Update(courseId, segmentId, saveCourseSegmentRequest);
        }

        // courses/<courseId>/segments/<segmentId>
        [HttpGet]
        public CourseSegmentInfo Segments(Guid courseId, Guid segmentId)
        {
            return _courseSegmentService.Get(courseId, segmentId);
        }

        // GET courses/<courseId>/segments/<segmentId>/segments -- returns children of the specified segment
        [HttpGet]
        public IEnumerable<CourseSegmentInfo> SubSegments(Guid courseId, Guid segmentId)
        {
            return _courseSegmentService.GetSubSegments(courseId, segmentId);
        }

        // POST courses/<courseId>/segments/<segmentId>/segments -- creates a child segment for a segment
        [HttpPost]
        [Transaction]
        public HttpResponseMessage SubSegments(Guid courseId, Guid segmentId, SaveCourseSegmentRequest saveCourseSegmentRequest)
        {
            var courseSegment = _courseSegmentService.Create(courseId, segmentId, saveCourseSegmentRequest);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

            string uri = Url.Link("CourseSegmentsApi", new
                {
                    action = "segments",
                    segmentId = courseSegment.Id
                });
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }

            return response;

        }

        // PUT courses/<courseId>/segments/<segmentId>/segments -- reorders the children segments
        [HttpPut]
        public void SubSegments(Guid courseId, Guid segmentId, IEnumerable<SaveCourseSegmentRequest> childrentSegments)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}