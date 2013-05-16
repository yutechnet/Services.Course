using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml.Linq;
using AutoMapper;
using BpeProducts.Common.WebApi.Attributes;
using BpeProducts.Common.WebApi.Authorization;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using ServiceStack.Text;

namespace BpeProducts.Services.Course.Host.Controllers
{
//    [DefaultHttpRouteConvention]
    [Authorize]
    public class CoursesController : ApiController
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IDomainEvents _domainEvents;
	    private readonly ICourseFactory _courseFactory;

	    public CoursesController(ICourseRepository courseRepository, IDomainEvents domainEvents,ICourseFactory courseFactory)
        {
            _courseRepository = courseRepository;
            _domainEvents = domainEvents;
	        _courseFactory = courseFactory;
        }

        // GET api/courses
        public IEnumerable<CourseInfoResponse> Get()
        {
            return _courseRepository.GetAll()
                                    .Select(c => Mapper.Map<CourseInfoResponse>(c))
                                    .ToList();
        }

        // GET api/courses/5
        public CourseInfoResponse Get(Guid id)
        {
            Domain.Entities.Course course =
                _courseRepository.GetAll().FirstOrDefault(c => c.Id.Equals(id) && c.ActiveFlag.Equals(true));
            if (course == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var courseResponse=  Mapper.Map<CourseInfoResponse>(course);
            courseResponse.Segments = course.Segments;
            return courseResponse;
        }

        [HttpGet]
        public CourseInfoResponse GetByCode(string code)
        {
            Domain.Entities.Course course =
                _courseRepository.GetAll().FirstOrDefault(c => c.Code == code && c.ActiveFlag.Equals(true));
            if (course == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return Mapper.Map<CourseInfoResponse>(course);
        }

        [HttpGet]
        public CourseInfoResponse GetByName(string name)
        {
            Domain.Entities.Course course =
                _courseRepository.GetAll().FirstOrDefault(c => c.Name == name && c.ActiveFlag.Equals(true));
            if (course == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return Mapper.Map<CourseInfoResponse>(course);
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [ClaimsAuthorize("CourseCreate")]
        // POST api/courses
        public HttpResponseMessage Post(SaveCourseRequest request)
        {
            var course = _courseFactory.Create(request);
            _domainEvents.Raise<CourseCreated>(new CourseCreated
	            {
		            AggregateId = course.Id,
		            Code = course.Code,
		            Description = course.Description,
		            Name = course.Name,
					ActiveFlag = course.ActiveFlag,
		            Course = course
	            });

            var courseInfoResponse = Mapper.Map<CourseInfoResponse>(_courseRepository.GetById(course.Id));
            HttpResponseMessage response = base.Request.CreateResponse(HttpStatusCode.Created, courseInfoResponse);

            string uri = Url.Link("DefaultApi", new { id = courseInfoResponse.Id });
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
            // We do not allow creation of a new resource by PUT.
            Domain.Entities.Course courseInDb = _courseRepository.GetById(id);

            if (courseInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            Mapper.Map(request, courseInDb);
            var oldCourse = courseInDb;

            courseInDb.Programs.Clear();
            foreach (Guid programId in request.ProgramIds)
            {
                var program = _courseRepository.Load<Program>(programId);
                courseInDb.Programs.Add(program);
            }

            _courseRepository.Update(courseInDb);
            var newCourse = courseInDb;

            _domainEvents.Raise<CourseUpdated>(new CourseUpdated
	            {
		            AggregateId = id,
		            Old = oldCourse,
		            New = newCourse
	            });
        }

        [Transaction]
        // DELETE api/courses/5
        public void Delete(Guid id)
        {
            Domain.Entities.Course courseInDb = _courseFactory.Reconstitute(id);

            if (courseInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            
            _domainEvents.Raise<CourseDeleted>(new CourseDeleted
	            {
		            AggregateId = id,
				});
            
        }

        #region Course Segment Management

        // courses/<courseId>/segments
        [HttpGet]
        public IEnumerable<CourseSegment> Segments(Guid courseId)
        {
            // returns the root course segments, including their childrent segments
            return Get(courseId).Segments;
        }

        // courses/<courseId>/segments
        [HttpPost]
        [Transaction]
        public HttpResponseMessage Segments(Guid courseId, SaveCourseSegmentRequest saveCourseSegmentRequest)
        {
            // saves a root segment
            var course = _courseFactory.Reconstitute(courseId);
            if (course.Id == Guid.Empty) throw new HttpResponseException(HttpStatusCode.NotFound);
            
            HttpResponseMessage response = base.Request.CreateResponse(HttpStatusCode.Created);
            var newSegmentId = Guid.NewGuid();
            string uri = Url.Link("CourseSegmentsApi", new { segmentId = newSegmentId});
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            } 
            
            _domainEvents.Raise<CourseSegmentAdded>(new CourseSegmentAdded
	            {
		            AggregateId = courseId,
		            Description = saveCourseSegmentRequest.Description,
		            Name = saveCourseSegmentRequest.Name,
		            ParentSegmentId = Guid.Empty,
		            Id = newSegmentId,
		            Type = saveCourseSegmentRequest.Type
	            });

            return response;

        }

        // courses/<courseId>/segments/<segmentId>
        [HttpPut]
        public void Segments(Guid courseId, Guid segmentId, Contract.SaveCourseSegmentRequest saveCourseSegmentRequest)
        {
            // Updates the specified segment
            var courseInDb = _courseFactory.Reconstitute(courseId);
            if (courseInDb.Id == Guid.Empty)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var courseSegment = courseInDb.SegmentIndex[segmentId];
            Mapper.Map(saveCourseSegmentRequest,courseSegment);
            courseSegment.Id = segmentId;
            
            _domainEvents.Raise<CourseSegmentUpdated>(new CourseSegmentUpdated
	            {
		            AggregateId = courseId,
		            Description = courseSegment.Description,
		            Name = courseSegment.Name,
		            ParentSegmentId = courseSegment.ParentSegmentId,
		            SegmentId = courseSegment.Id,
		            Type = courseSegment.Type
	            });
        }

        // courses/<courseId>/segments/<segmentId>
        [HttpGet]
        public CourseSegment Segments(Guid courseId, Guid segmentId)
        {
            // returns the specified segments (and its children)
            Domain.Entities.Course courseInDb = _courseRepository.GetById(courseId);

            if (courseInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return courseInDb.SegmentIndex[segmentId];
        }

        // GET courses/<courseId>/segments/<segmentId>/segments -- returns children of the specified segment
        [HttpGet]
        public IEnumerable<Contract.CourseSegment> SubSegments(Guid courseId, Guid segmentId)
        {
            Domain.Entities.Course courseInDb = _courseRepository.GetById(courseId);

            if (courseInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return courseInDb.SegmentIndex[segmentId].ChildrenSegments;
        }

        // POST courses/<courseId>/segments/<segmentId>/segments -- creates a child segment for a segment
        [HttpPost]
        [Transaction]
        public HttpResponseMessage SubSegments(Guid courseId, Guid segmentId, Contract.SaveCourseSegmentRequest saveCourseSegmentRequest)
        {
            var course = _courseFactory.Reconstitute(courseId);
            if (course.Id == Guid.Empty||course.SegmentIndex[segmentId]==null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
          
            var newSegmentId = Guid.NewGuid();

            HttpResponseMessage response = base.Request.CreateResponse(HttpStatusCode.Created);

            string uri = Url.Link("CourseSegmentsApi", new
                {
                    action = "segments",
                    segmentId = newSegmentId
                });
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }

            //raise domain event
            _domainEvents.Raise<CourseSegmentAdded>(new CourseSegmentAdded
	            {
		            AggregateId = courseId,
		            Name = saveCourseSegmentRequest.Name,
		            Description = saveCourseSegmentRequest.Description,
		            ParentSegmentId = segmentId,
		            Type = saveCourseSegmentRequest.Type,
		            Id = newSegmentId
	            });

            return response;

        }

        // PUT courses/<courseId>/segments/<segmentId>/segments -- reorders the children segments
        [HttpPut]
        public void SubSegments(Guid courseId, Guid segmentId, IEnumerable<Contract.SaveCourseSegmentRequest> childrentSegments)
        {

        }

        #endregion
    }
}