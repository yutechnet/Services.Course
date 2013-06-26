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
        private readonly ILearningOutcomeRepository _learningOutcomeRepository;
        private readonly IDomainEvents _domainEvents;
	    private readonly IOutcomeFactory _outcomeFactory;

        public OutcomeVersionController(ILearningOutcomeRepository learningOutcomeRepository, IDomainEvents domainEvents, IOutcomeFactory outcomeFactory)
        {
            _learningOutcomeRepository = learningOutcomeRepository;
            _domainEvents = domainEvents;
            _outcomeFactory = outcomeFactory;
        }


        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [HttpPut]
        public void PublishVersion(Guid id, PublishRequest request)
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
        public HttpResponseMessage CreateVersion(VersionRequest request)
        {
            var outcomeInDb = _learningOutcomeRepository.Load(request.ParentVersionId);
            if (outcomeInDb == null)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = string.Format("Parent version {0} not found.", request.ParentVersionId)
                });
            }

            var newVersion = _outcomeFactory.BuildNewVersion(outcomeInDb, request.VersionNumber);

            _domainEvents.Raise<OutcomeVersionCreated>(new OutcomeVersionCreated
                {
                    AggregateId = newVersion.Id,
                    NewVersion = newVersion
                });

            var outcomeResponse = Mapper.Map<OutcomeResponse>(newVersion);
            HttpResponseMessage response = base.Request.CreateResponse(HttpStatusCode.Created, outcomeResponse);

            string uri = Url.Link("DefaultApi", new { controller = "outcome", id = outcomeResponse.Id });
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
            return response;
        }

    }
}
