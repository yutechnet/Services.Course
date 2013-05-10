using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using BpeProducts.Common.WebApi.Attributes;
using BpeProducts.Common.WebApi.Authorization;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;
using CourseSegment = BpeProducts.Services.Course.Contract.CourseSegment;

namespace BpeProducts.Services.Course.Host.Controllers
{
//    [DefaultHttpRouteConvention]
    [Authorize]
    public class CoursesController : ApiController
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IDomainEvents _domainEvents;

        public CoursesController(ICourseRepository courseRepository, IDomainEvents domainEvents)
        {
            _courseRepository = courseRepository;
            _domainEvents = domainEvents;
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

            return Mapper.Map<CourseInfoResponse>(course);
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
            var course = Mapper.Map<Domain.Entities.Course>(request);
            // Make sure the course is active by default
            course.ActiveFlag = true;
            // No duplicate check whatsoever 
            var id = (Guid) _courseRepository.Add(course);

            var courseInfoResponse = Mapper.Map<CourseInfoResponse>(_courseRepository.GetById(id));
            HttpResponseMessage response = base.Request.CreateResponse(HttpStatusCode.Created, courseInfoResponse);

            string uri = Url.Link("DefaultApi", new {id = courseInfoResponse.Id});
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }

            _domainEvents.Raise<CourseCreated>(new CourseCreated
            {
                AggregateId = id,
                Code = course.Code,
                Description = course.Description,
                Name = course.Name
            });

            return response;
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        // PUT api/courses/5
        public void Put(Guid id, SaveCourseRequest request)
        {
            var courseFromFactory = new CourseFactory().Create(id);


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
            Domain.Entities.Course courseInDb = _courseRepository.GetById(id);

            if (courseInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            // logical delete
            courseInDb.ActiveFlag = false;
            _courseRepository.Update(courseInDb);

            _domainEvents.Raise<CourseDeleted>(new CourseDeleted
                {
                    AggregateId = id
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
        public void Segments(Guid courseId, CourseSegment courseSegment)
        {
            // saves a root segment
            Domain.Entities.Course courseInDb = _courseRepository.GetById(courseId);

            if (courseInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var newSegment = Mapper.Map<Domain.Entities.CourseSegment>(courseSegment);
            courseInDb.Segments.Add(newSegment);

            _courseRepository.Update(courseInDb);

            _domainEvents.Raise<CourseSegmentAdded>(new CourseSegmentAdded
                {
                    AggregateId = courseId,
                    Description = newSegment.Description,
                    Name = newSegment.Name,
                    ParentSegmentId = Guid.Empty,
                    SegmentId = newSegment.Id,
                    Type = newSegment.Type
                });

        }

        // courses/<courseId>/segments/<segmentId>
        [HttpPut]
        public void Segments(Guid course, Guid segmentId, Contract.CourseSegment courseSegment)
        {
            // Updates the specified segment
        }

        // courses/<courseId>/segments/<segmentId>
        [HttpGet]
        public Contract.CourseSegment Segments(Guid courseId, Guid segmentId)
        {
            // returns the specified segments (and its children)
            return new CourseSegment();
        }

        // GET courses/<courseId>/segments/<segmentId>/segments -- returns children of the specified segment
        [HttpGet]
        public IEnumerable<Contract.CourseSegment> SubSegments(Guid courseId, Guid segmentId)
        {
            return new List<CourseSegment>();
        }

        // POST courses/<courseId>/segments/<segmentId>/segments -- creates a child segment for a segment
        [HttpPost]
        public void SubSegments(Guid courseId, Guid segmentId, Contract.CourseSegment courseSegment)
        {
            return;
        }
        // PUT courses/<courseId>/segments/<segmentId>/segments -- reorders the children segments
        [HttpPut]
        public void SubSegments(Guid courseId, Guid segmentId, IEnumerable<Contract.CourseSegment> childrentSegments)
        {

        }

        #endregion
    }
}