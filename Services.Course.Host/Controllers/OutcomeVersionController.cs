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
using BpeProducts.Services.Course.Domain.Outcomes;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Host.Controllers
{
    public class OutcomeVersionController : ApiController
    {
        private readonly IRepository _repository;
        private readonly IDomainEvents _domainEvents;
	    private readonly IOutcomeFactory _outcomeFactory;

        public OutcomeVersionController(IRepository repository, IDomainEvents domainEvents, IOutcomeFactory outcomeFactory)
        {
            _repository = repository;
            _domainEvents = domainEvents;
            _outcomeFactory = outcomeFactory;
        }


        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [HttpPut]
        public void PublishVersion(Guid id, CoursePublishRequest request)
        {
            var courseInDb = _outcomeFactory.Reconstitute(id);

            if (courseInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            _domainEvents.Raise<OutcomeVersionPublished>(new OutcomeVersionPublished
            {
                AggregateId = id,
                PublishNote = request.PublishNote
            });
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [HttpPost]
        public HttpResponseMessage CreateVersion(string entityType, CourseVersionRequest request)
        {
            var outcomeInDb = _repository.Query<Domain.Entities.LearningOutcome>().FirstOrDefault(c => c.Id.Equals(request.ParentVersionId) && c.ActiveFlag.Equals(true));
            if (outcomeInDb == null)
            {
                throw new HttpResponseException(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        ReasonPhrase = string.Format("Parent version {0} not found.", request.ParentVersionId)
                    });
            }

            var versionExists =
                _repository.Query<Domain.Entities.Course>()
                                 .Any(
                                     c =>
                                     c.OriginalEntityId.Equals(outcomeInDb.OriginalEntityId) && c.ActiveFlag.Equals(true) &&
                                     c.VersionNumber.Equals(request.VersionNumber));
            if (versionExists)
            {
                throw new HttpResponseException(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.Conflict,
                        ReasonPhrase = string.Format("Version {0} for the course {1} already exists", request.VersionNumber, outcomeInDb.OriginalEntityId)
                    });
            }

            var courseId = Guid.NewGuid();
            _domainEvents.Raise<OutcomeVersionCreated>(new OutcomeVersionCreated
                {
                    AggregateId = courseId,
                    IsPublished = false,
                    OriginalEntityId = outcomeInDb.OriginalEntityId,
                    ParentEntityId = request.ParentVersionId,
                    VersionNumber = request.VersionNumber
                });

            var courseInfoResponse = Mapper.Map<CourseInfoResponse>(_repository.Get<Domain.Entities.Course>(courseId));
            HttpResponseMessage response = base.Request.CreateResponse(HttpStatusCode.Created, courseInfoResponse);

            string uri = Url.Link("DefaultApi", new { controller = "outcome", id = courseInfoResponse.Id });
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
            return response;
        }

    }
}
