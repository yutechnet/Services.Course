using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate;
using BpeProducts.Common.WebApi.Attributes;
using BpeProducts.Common.WebApi.Authorization;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using BpeProducts.Services.Course.Domain.Outcomes;
using BpeProducts.Services.Course.Domain.Repositories;
using NHibernate;
using NHibernate.Linq;
using log4net;
using BpeProducts.Common.Log;

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
	    public OutcomeResponse Get(Guid id)
		{
		    return _learningOutcomeService.Get(id);
		}

	    [Transaction]
        public OutcomeResponse Get(string entityType, Guid entityId, Guid outcomeId)
	    {
	        return _learningOutcomeService.Get(entityType, entityId, outcomeId);
	    }

		[Transaction]
		public List<OutcomeResponse> Get(string entityType, Guid entityId)
		{
		    return _learningOutcomeService.Get(entityType, entityId).ToList();
		}

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [ClaimsAuthorize()]
        public HttpResponseMessage Post(OutcomeRequest request)
        {
            var outcomeResponse = _learningOutcomeService.Create(request);
            var response = base.Request.CreateResponse(HttpStatusCode.Created, outcomeResponse);

            string uri = Url.Link("DefaultApi", new { id = outcomeResponse.Id });
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
    }
}
