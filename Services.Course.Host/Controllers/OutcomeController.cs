using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting.Web.Http;
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
        [HttpGet]
        [GET("{entityType}/{entityId:guid}/supports", RouteName = "GetEntityOutcomes")]
        public List<OutcomeInfo> Get(string entityType, Guid entityId)
        {
            return _learningOutcomeService.GetEntityOutcomes(entityId).ToList();
        }

        [HttpGet]
        [GET("outcome/entityoutcomes?{entityIds}")]
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

        [Transaction]
        [HttpGet]
        [GET("outcome/{outcomeId:guid}", RouteName = "GetOutcome")]
        public OutcomeInfo Get(Guid outcomeId)
        {
            return _learningOutcomeService.Get(outcomeId);
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [ClaimsAuthorize]
        [HttpPost]
        [POST("outcome")]
        public HttpResponseMessage Post(OutcomeRequest request)
        {
            var outcomeResponse = _learningOutcomeService.Create(request);
            var response = base.Request.CreateResponse(HttpStatusCode.Created, outcomeResponse);

            var uri = Url.Link("GetOutcome", new { outcomeId = outcomeResponse.Id });
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
            return response;

        }

        [Transaction]
        [HttpGet]
        [GET("{entityType}/{entityId:guid}/supports/{outcomeId:guid}", RouteName = "GetEntityOutcome")]
        public OutcomeInfo Get(string entityType, Guid entityId, Guid outcomeId)
        {
            return _learningOutcomeService.Get(entityType, entityId, outcomeId);
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [ClaimsAuthorize]
        [HttpPost]
        [POST("{entityType}/{entityId:guid}/supports")]
        public HttpResponseMessage Post(string entityType, Guid entityId, OutcomeRequest request)
        {
            var outcomeResponse = _learningOutcomeService.Create(entityType, entityId, request);

            var response = base.Request.CreateResponse(HttpStatusCode.Created, outcomeResponse);

            var uri = Url.Link("GetEntityOutcome", new {entityType = entityType, entityId = entityId, outcomeId = outcomeResponse.Id});
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
            return response;
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [HttpPut]
        [PUT("outcome/{outcomeId:guid}")]
        public void Put(Guid outcomeId, OutcomeRequest request)
        {
            _learningOutcomeService.Update(outcomeId, request);
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [HttpPut]
        [PUT("{entityType}/{entityId:guid}/supports/{outcomeId:guid}")]
        public void Put(string entityType, Guid entityId, Guid outcomeId, OutcomeRequest request)
        {
            _learningOutcomeService.Update(entityType, entityId, outcomeId, request);
        }

        [Transaction]
        [HttpDelete]
        [DELETE("outcome/{outcomeId:guid}")]
        public void Delete(Guid outcomeId)
        {
            _learningOutcomeService.Delete(outcomeId);
        }

        [Transaction]
        [HttpDelete]
        [DELETE("{entityType}/{entityId:guid}/supports/{outcomeId:guid}")]
        public void Delete(string entityType, Guid entityId, Guid outcomeId)
        {
            _learningOutcomeService.Delete(entityType, entityId, outcomeId);
        }

        [HttpGet]
        [GET("course/{courseId:guid}/segments/{segmentId:guid}/supports", RouteName = "GetSegmentOutcomes")]
        public List<OutcomeInfo> CourseSegmentOutcome(Guid courseId, Guid segmentId)
        {
            return _learningOutcomeService.GetEntityOutcomes(segmentId).ToList();
        }

        [Transaction]
        [HttpPost]
        [POST("course/{courseId:guid}/segments/{segmentId:guid}/supports")]
        public HttpResponseMessage CourseSegmentOutcome(Guid courseId, Guid segmentId, OutcomeRequest request)
        {
            var outcomeResponse = _learningOutcomeService.Create("segments", segmentId, request);

            var response = Request.CreateResponse(HttpStatusCode.Created, outcomeResponse);

            var uri = Url.Link("GetOutcome", new {outcomeId = outcomeResponse.Id});
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
            return response;
        }

        [HttpGet]
        [GET("outcome/{supportingOutcomeId:guid}/supports", RouteName = "GetSupportingOutcomes", ControllerPrecedence = 1)]
        public List<OutcomeInfo> GetSupportingOutcomes(Guid supportingOutcomeId)
        {
            return _learningOutcomeService.GetSupportingOutcomes(supportingOutcomeId);
        }

        [Transaction]
        [HttpPut]
        [PUT("outcome/{supportingOutcomeId:guid}/supports/{supportedOutcomeId:guid}", ControllerPrecedence = 2)]
        public void AddSupportingOutcome(Guid supportingOutcomeId, Guid supportedOutcomeId)
        {
            _learningOutcomeService.AddSupportingOutcome(supportingOutcomeId, supportedOutcomeId);
        }

        [Transaction]
        [HttpDelete]
        [DELETE("outcome/{supportingOutcomeId:guid}/supports/{supportedOutcomeId:guid}", ControllerPrecedence = 3)]
        public void RemoveSupportingOutcome(Guid supportingOutcomeId, Guid supportedOutcomeId)
        {
            _learningOutcomeService.RemoveSupportingOutcome(supportingOutcomeId, supportedOutcomeId);
        }
    }
}
