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
using BpeProducts.Common.WebApi;
using BpeProducts.Common.WebApi.Attributes;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Host.Controllers
{
    public class CoursesController : ApiController
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ITenantExtractor _tenantExtractor;

        public CoursesController(ICourseRepository courseRepository, ITenantExtractor tenantExtractor)
        {
            _courseRepository = courseRepository;
            _tenantExtractor = tenantExtractor;
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
            var course = _courseRepository.GetById(id);
            if (course == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return Mapper.Map<CourseInfoResponse>(course);
        }

        [HttpGet]
        public CourseInfoResponse GetByCode(string code)
        {
            var course = _courseRepository.GetAll().FirstOrDefault(c => c.Code == code);
            if (course == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return Mapper.Map<CourseInfoResponse>(course);
        }

        [HttpGet]
        public CourseInfoResponse GetByName(string name)
        {
            var course = _courseRepository.GetAll().FirstOrDefault(c => c.Name == name);
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
        public Guid Post(SaveCourseRequest request)
        {
            var course = Mapper.Map<Domain.Entities.Course>(request);
            course.TenantId = _tenantExtractor.GetTenantId(Request);

            if (_courseRepository.GetAll().Any(c => (c.Name.Equals(request.Name) || c.Code.Equals(request.Code))))
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Conflict,
                    ReasonPhrase = "Course already exists."
                });
            }

            return (Guid) _courseRepository.Add(course);
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
            if (_courseRepository.GetAll().Any(c => c.Id.Equals(id)))
            {
                _courseRepository.DeleteById(id);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }

    
}
