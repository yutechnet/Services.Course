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
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Host.Controllers
{
    public class OutcomeController : ApiController
    {
        private readonly ILearningOutcomeRepository _learningOutcomeRepository;

        public OutcomeController(ILearningOutcomeRepository learningOutcomeRepository)
        {
            _learningOutcomeRepository = learningOutcomeRepository;
        }

        public OutcomeResponse Get(Guid id)
        {
            var learningOutcome =
                _learningOutcomeRepository.GetAll().FirstOrDefault(l => l.Id == id && l.ActiveFlag);
            if (learningOutcome == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return Mapper.Map<OutcomeResponse>(learningOutcome);
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [ClaimsAuthorize()]
        public HttpResponseMessage Post(OutcomeRequest request)
        {
            var learningOutcome = Mapper.Map<LearningOutcome>(request);
            _learningOutcomeRepository.Add(learningOutcome);

            var outcomeResponse = Mapper.Map<OutcomeResponse>(learningOutcome);
            var response = base.Request.CreateResponse(HttpStatusCode.Created, outcomeResponse);

            string uri = Url.Link("DefaultApi", new { id = learningOutcome.Id });
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
            return response;

        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        public void Put(Guid id, OutcomeRequest request)
        {
            var learningOutcome = _learningOutcomeRepository.GetById(id);
            if (learningOutcome == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            learningOutcome.Description = request.Description;
            _learningOutcomeRepository.Update(learningOutcome);
        }

        [Transaction]
        public void Delete(Guid id)
        {
            var learningOutcome = _learningOutcomeRepository.GetById(id);
            if (learningOutcome == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            learningOutcome.ActiveFlag = false;
            _learningOutcomeRepository.Update(learningOutcome);
        }
    }
}
