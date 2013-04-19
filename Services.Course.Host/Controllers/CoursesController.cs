using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using AutoMapper;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate;
using BpeProducts.Common.WebApi;
using BpeProducts.Common.WebApi.Attributes;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Host.Controllers
{
    [Authorize]
    public class CoursesController : ApiController
    {
        private readonly ICourseRepository _courseRepository;

        public CoursesController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
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
            var course = _courseRepository.GetAll().FirstOrDefault(c => c.Id.Equals(id) && c.ActiveFlag.Equals(true));
            if (course == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return Mapper.Map<CourseInfoResponse>(course);
        }

        [HttpGet]
        public CourseInfoResponse GetByCode(string code)
        {
            var course = _courseRepository.GetAll().FirstOrDefault(c => c.Code == code && c.ActiveFlag.Equals(true));
            if (course == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return Mapper.Map<CourseInfoResponse>(course);
        }

        [HttpGet]
        public CourseInfoResponse GetByName(string name)
        {
            var course = _courseRepository.GetAll().FirstOrDefault(c => c.Name == name && c.ActiveFlag.Equals(true));
            if (course == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return Mapper.Map<CourseInfoResponse>(course);
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        // POST api/courses
        public HttpResponseMessage Post(SaveCourseRequest request)
        {
            var tenantId = request.TenantId;

            var course = Mapper.Map<Domain.Entities.Course>(request);
            // Make sure the course is active by default
            course.ActiveFlag = true;
            // No duplicate check whatsoever 
            var id = (Guid) _courseRepository.Add(course);

            //return id;

            var courseInfoResponse = Mapper.Map<CourseInfoResponse>(_courseRepository.GetById(id));
            var response = base.Request.CreateResponse<CourseInfoResponse>(HttpStatusCode.Created, courseInfoResponse);

            var uri = Url.Link("DefaultApi", new {id = courseInfoResponse.Id});
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
            var courseInDb = _courseRepository.GetById(id);
            
            if(courseInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);    
            }

            Mapper.Map(request, courseInDb);
            _courseRepository.Update(courseInDb);
        }

        [Transaction]
        // DELETE api/courses/5
        public void Delete(Guid id)
        {
            var courseInDb = _courseRepository.GetById(id);

            if (courseInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            // logical delete
            courseInDb.ActiveFlag = false;
            _courseRepository.Update(courseInDb);
        }
    }

    
}
