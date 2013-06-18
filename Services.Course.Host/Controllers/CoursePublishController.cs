using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using BpeProducts.Common.WebApi.Attributes;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Host.Controllers
{
    public class CoursePublishController : ApiController
    {
        private readonly IRepository _courseRepository;
        private readonly IDomainEvents _domainEvents;
	    private readonly ICourseFactory _courseFactory;

        public CoursePublishController(IRepository courseRepository, IDomainEvents domainEvents, ICourseFactory courseFactory)
        {
            _courseRepository = courseRepository;
            _domainEvents = domainEvents;
	        _courseFactory = courseFactory;
        }


        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [HttpPut]
        public void Publish(Guid id, CoursePublishRequest request)
        {
            // We do not allow creation of a new resource by PUT.
            Domain.Entities.Course courseInDb = _courseFactory.Reconstitute(id);

            if (courseInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            _domainEvents.Raise<CourseVersionPublished>(new CourseVersionPublished
            {
                AggregateId = id,
                PublishNote = request.PublishNote,
                VersionNumber = request.VersionNumber
            });
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [HttpPost]
        public HttpResponseMessage CreateVersion(CourseVersionRequest request)
        {
            Domain.Entities.Course courseInDb = _courseFactory.Reconstitute(request.ParentVersionId);
            if (courseInDb == null)
            {
                throw new HttpResponseException(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        ReasonPhrase = string.Format("Parent version {0} not found.", request.ParentVersionId)
                    });
            }

            var courseId = Guid.NewGuid();
            _domainEvents.Raise<CourseVersionCreated>(new CourseVersionCreated
                {
                    AggregateId = courseId,
                    IsPublished = false,
                    OriginalCourseId = courseInDb.OriginalEntityId,
                    ParentCourseId = request.ParentVersionId,
                    VersionNumber = request.VersionNumber
                });

            var courseInfoResponse = Mapper.Map<CourseInfoResponse>(_courseRepository.Get<Domain.Entities.Course>(courseId));
            HttpResponseMessage response = base.Request.CreateResponse(HttpStatusCode.Created, courseInfoResponse);

            string uri = Url.Link("DefaultApi", new { controller = "courses", id = courseInfoResponse.Id });
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
            return response;
        }

    }
}
