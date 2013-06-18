using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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

    }
}
