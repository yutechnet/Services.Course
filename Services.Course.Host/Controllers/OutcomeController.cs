using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.WebApi.NHibernate;
using BpeProducts.Common.WebApi.Validation;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace BpeProducts.Services.Course.Host.Controllers
{
    public class OutcomeController : ApiController
    {
        private readonly ILearningOutcomeService _learningOutcomeService;
        private IVersionHandler _versionHandler;

        public OutcomeController(ILearningOutcomeService learningOutcomeService,IVersionHandler versionHandler)
        {
            _learningOutcomeService = learningOutcomeService;
            _versionHandler = versionHandler;
        }

        [Transaction]
        [Route("{entityType}/{entityId:guid}/supports", Name = "GetEntityOutcomes")]
        public List<OutcomeInfo> Get(string entityType, Guid entityId)
        {
            return _learningOutcomeService.GetEntityOutcomes(entityId).ToList();
        }

        [HttpGet]
        [Route("outcome/entityoutcomes")]
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
        [Route("outcome/{outcomeId:guid}", Name = "GetOutcome")]
        public OutcomeInfo Get(Guid outcomeId)
        {
            return _learningOutcomeService.Get(outcomeId);
        }

        [Transaction]
        [ArgumentsNotNull]
        [ValidateModelState]
        [Route("outcome")]
        public HttpResponseMessage Post(OutcomeRequest request)
        {
            var outcomeResponse = _learningOutcomeService.Create(request);
            var response = Request.CreateResponse(HttpStatusCode.Created, outcomeResponse);

            var uri = Url.Link("GetOutcome", new { outcomeId = outcomeResponse.Id });
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
            return response;

        }

        [Transaction]
        [Route("{entityType}/{entityId:guid}/supports/{outcomeId:guid}", Name = "GetEntityOutcome")]
        public OutcomeInfo Get(string entityType, Guid entityId, Guid outcomeId)
        {
            return _learningOutcomeService.Get(entityType, entityId, outcomeId);
        }

        [Transaction]
        [ArgumentsNotNull]
        [ValidateModelState]
        [Route("{entityType}/{entityId:guid}/supports")]
        public HttpResponseMessage Post(string entityType, Guid entityId, OutcomeRequest request)
        {
            var outcomeResponse = _learningOutcomeService.Create(entityType, entityId, request);

            var response = Request.CreateResponse(HttpStatusCode.Created, outcomeResponse);

            var uri = Url.Link("GetEntityOutcome", new {entityType, entityId, outcomeId = outcomeResponse.Id});
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
            return response;
        }

        [Transaction]
        [ArgumentsNotNull]
        [ValidateModelState]
        [Route("outcome/{outcomeId:guid}")]
        public void Put(Guid outcomeId, OutcomeRequest request)
        {
            _learningOutcomeService.Update(outcomeId, request);
        }

        [Transaction]
        [ArgumentsNotNull]
        [ValidateModelState]
        [Route("{entityType}/{entityId:guid}/supports/{outcomeId:guid}", Order = 9)]
        public void Put(string entityType, Guid entityId, Guid outcomeId, OutcomeRequest request)
        {
            _learningOutcomeService.Update(entityType, entityId, outcomeId, request);
        }

        [Transaction]
        [Route("outcome/{outcomeId:guid}")]
        public void Delete(Guid outcomeId)
        {
            _learningOutcomeService.Delete(outcomeId);
        }

        [Transaction]
        [Route("{entityType}/{entityId:guid}/supports/{outcomeId:guid}")]
        public void Delete(string entityType, Guid entityId, Guid outcomeId)
        {
            _learningOutcomeService.Delete(entityType, entityId, outcomeId);
        }

        [HttpGet]
        [Route("course/{courseId:guid}/segments/{segmentId:guid}/supports", Name = "GetSegmentOutcomes")]
        public List<OutcomeInfo> CourseSegmentOutcome(Guid courseId, Guid segmentId)
        {
            return _learningOutcomeService.GetEntityOutcomes(segmentId).ToList();
        }

        [Transaction]
        [HttpPost]
        [Route("course/{courseId:guid}/segments/{segmentId:guid}/supports")]
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
        [Route("outcome/{supportingOutcomeId:guid}/supports", Name = "GetSupportingOutcomes", Order = -3)]
        public List<OutcomeInfo> GetSupportingOutcomes(Guid supportingOutcomeId)
        {
            return _learningOutcomeService.GetSupportingOutcomes(supportingOutcomeId);
        }

        [Transaction]
        [HttpPut]
        [Route("outcome/{supportingOutcomeId:guid}/supports/{supportedOutcomeId:guid}", Order = -2)]
        public void AddSupportingOutcome(Guid supportingOutcomeId, Guid supportedOutcomeId)
        {
            _learningOutcomeService.AddSupportingOutcome(supportingOutcomeId, supportedOutcomeId);
        }

        [Transaction]
        [HttpDelete]
        [Route("outcome/{supportingOutcomeId:guid}/supports/{supportedOutcomeId:guid}", Order = -1)]
        public void RemoveSupportingOutcome(Guid supportingOutcomeId, Guid supportedOutcomeId)
        {
            _learningOutcomeService.RemoveSupportingOutcome(supportingOutcomeId, supportedOutcomeId);
        }

        [Transaction]
        [ArgumentsNotNull]
        [ValidateModelState]
        [HttpPost]
        [Route("outcome/version", Name = "CreateOutcomeVersion")]
        public HttpResponseMessage CreateVersion(VersionRequest request)
        {
           
            var entity = _versionHandler.CreateVersion("learningoutcome", request.ParentVersionId, request.VersionNumber);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

            var uri = Url.Link("GetOutcome", new { outcomeId = entity.Id });
            if (uri != null)
            {
                response.Headers.Location = new Uri(uri);
            }
           
            return response;
        }

        [Transaction]
        [ArgumentsNotNull]
        [ValidateModelState]
        [HttpPut]
        [Route("outcome/{entityId:guid}/publish")]
        public void PublishVersion( Guid entityId, PublishRequest request)
        {
            _versionHandler.PublishVersion("learningoutcome", entityId, request.PublishNote);
        }
    }
}
