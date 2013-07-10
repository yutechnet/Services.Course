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
    public class CourseVersionController : ApiController
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IDomainEvents _domainEvents;
	    private readonly ICourseFactory _courseFactory;

        public CourseVersionController(ICourseRepository courseRepository, IDomainEvents domainEvents, ICourseFactory courseFactory)
        {
            _courseRepository = courseRepository;
            _domainEvents = domainEvents;
	        _courseFactory = courseFactory;
        }


        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [HttpPut]
        public void PublishVersion(Guid id, PublishRequest request)
        {
            var courseInDb = _courseFactory.Reconstitute(id);

            if (courseInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            _domainEvents.Raise<CourseVersionPublished>(new CourseVersionPublished
            {
                AggregateId = id,
                PublishNote = request.PublishNote
            });
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [HttpPost]
        public HttpResponseMessage CreateVersion(VersionRequest request)
        {
            var courseInDb = _courseRepository.Get(request.ParentVersionId);
            if (courseInDb == null)
            {
                throw new HttpResponseException(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        ReasonPhrase = string.Format("Parent version {0} not found.", request.ParentVersionId)
                    });
            }

            var newVersion = _courseFactory.BuildNewVersion(courseInDb, request.VersionNumber);

            _domainEvents.Raise<CourseVersionCreated>(new CourseVersionCreated
                {
                    AggregateId = newVersion.Id,
                    NewVersion = newVersion
                });

            var courseInfoResponse = Mapper.Map<CourseInfoResponse>(newVersion);
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
