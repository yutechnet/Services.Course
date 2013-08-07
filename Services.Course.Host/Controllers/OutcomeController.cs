using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.WebApi.Attributes;
using BpeProducts.Common.WebApi.Authorization;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;

namespace BpeProducts.Services.Course.Host.Controllers
{
    public class OutcomeController : ApiController
    {
        private readonly ILearningOutcomeService _learningOutcomeService;

        public OutcomeController(ILearningOutcomeService learningOutcomeService)
        {
            _learningOutcomeService = learningOutcomeService;
        }

        [Transaction]
        public OutcomeInfo Get(Guid id)
        {
            return _learningOutcomeService.Get(id);
        }

        [Transaction]
        public OutcomeInfo Get(string entityType, Guid entityId, Guid outcomeId)
        {
            return _learningOutcomeService.Get(entityType, entityId, outcomeId);
        }

        [Transaction]
        public List<OutcomeInfo> Get(string entityType, Guid entityId)
        {
            return _learningOutcomeService.Get(entityType, entityId).ToList();
        }

        public Dictionary<Guid, List<OutcomeInfo>> GetEntityOutcomes(string entityIds)
        {
            var ids = entityIds.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList();

            try
            {
                var guids = ids.Select(Guid.Parse).ToList();
                return _learningOutcomeService.GetBySupportingEntities(guids);
            }
            catch (FormatException ex)
            {
                throw new BadRequestException("Entity ID is not in a correct format", ex);
            }
        }

        [HttpGet]
        public List<OutcomeInfo> GetSupportingOutcomes(Guid supportingOutcomeId)
        {
            return _learningOutcomeService.GetSupportingOutcomes(supportingOutcomeId);
        }
            
        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [ClaimsAuthorize()]
        public HttpResponseMessage Post(OutcomeRequest request)
        {
            var outcomeResponse = _learningOutcomeService.Create(request);
            var response = base.Request.CreateResponse(HttpStatusCode.Created, outcomeResponse);

            string uri = Url.Link("DefaultApi", new {id = outcomeResponse.Id});
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
            return response;

        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [ClaimsAuthorize()]
        public HttpResponseMessage Post(string entityType, Guid entityId, OutcomeRequest request)
        {
            var outcomeResponse = _learningOutcomeService.Create(entityType, entityId, request);

            var response = base.Request.CreateResponse(HttpStatusCode.Created, outcomeResponse);

            // /<entityType>/<entityId>/supports/<outcomeId>
            string uri = Url.Link("OutcomeApi", new
                {
                    controller = "outcome",
                    entityType = entityType,
                    entityId = entityId,
                    outcomeId = outcomeResponse.Id
                });
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
            _learningOutcomeService.Update(id, request);
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        public void Put(string entityType, Guid entityId, Guid outcomeId, OutcomeRequest request)
        {
            _learningOutcomeService.Update(entityType, entityId, outcomeId, request);
        }

        [Transaction]
        public void Delete(Guid id)
        {
            _learningOutcomeService.Delete(id);
        }

        [Transaction]
        public void Delete(string entityType, Guid entityId, Guid outcomeId)
        {
            _learningOutcomeService.Delete(entityType, entityId, outcomeId);
        }

        [Transaction]
        [HttpPost]
        public HttpResponseMessage CourseSegmentOutcome(Guid courseId, Guid segmentId, OutcomeRequest request)
        {
            var outcomeResponse = _learningOutcomeService.Create("segments", segmentId, request);

            var response = base.Request.CreateResponse(HttpStatusCode.Created, outcomeResponse);

            string uri = Url.Link("DefaultApi", new
                {
                    controller = "outcome",
                    id = outcomeResponse.Id
                });
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
            return response;
        }

        [Transaction]
        [HttpPut]
        public void AddSupportingOutcome(Guid supportingOutcomeId, Guid supportedOutcomeId)
        {
            _learningOutcomeService.AddSupportingOutcome(supportingOutcomeId, supportedOutcomeId);
        }

        [Transaction]
        [HttpDelete]
        public void RemoveSupportingOutcome(Guid supportingOutcomeId, Guid supportedOutcomeId)
        {
            _learningOutcomeService.RemoveSupportingOutcome(supportingOutcomeId, supportedOutcomeId);
        }
    }
}
