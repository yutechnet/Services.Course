using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.WebApi.Attributes;
using BpeProducts.Common.WebApi.Authorization;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Repositories;
using NHibernate;

namespace BpeProducts.Services.Course.Host.Controllers
{
    public class OutcomeController : ApiController
    {
        private readonly ILearningOutcomeRepository _learningOutcomeRepository;
	    private readonly ISession _session;

	    public OutcomeController(ILearningOutcomeRepository learningOutcomeRepository,ISession session)
        {
	        _learningOutcomeRepository = learningOutcomeRepository;
	        _session = session;
        }

		[Transaction]
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
	    public object Get(string entityType, Guid entityId, Guid outcomeId)
	    {
		    IHaveOutcomes entity =
			    _session.QueryOver<IHaveOutcomes>()
			            .Where(e => e.Id == entityId)
			            .SingleOrDefault();
		    
			if (entity == null)
		    {
			    throw new HttpResponseException(HttpStatusCode.NotFound);
		    }

			LearningOutcome outcome = entity.Outcomes
		                                    .SingleOrDefault(l => l.Id == outcomeId);

		    if (outcome == null)
		    {
			    throw new HttpResponseException(HttpStatusCode.NotFound);
		    }
		    
			return Mapper.Map<OutcomeResponse>(outcome);
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
		[ClaimsAuthorize()]
		public object Post(string entityType,Guid entityId, OutcomeRequest request)
		{
			//check if the entity exists
			//apply strategy pattern here?
			var entities = _session.QueryOver<IHaveOutcomes>().Where(x => x.Id == entityId);
			var entity = entities.SingleOrDefault<IHaveOutcomes>();
			//create an outcome and associate to an entity
			var learningOutcome = Mapper.Map<LearningOutcome>(request);
			//learningOutcome.Id = Guid.NewGuid();
			_session.Save(learningOutcome);
			
			entity.Outcomes.Add(learningOutcome);

			_session.Save(entity);
			var outcomeResponse = Mapper.Map<OutcomeResponse>(learningOutcome);
			
			var response = base.Request.CreateResponse(HttpStatusCode.Created, outcomeResponse);

			string uri = Url.Link("OutcomeApi", new {controller="outcome",entityType=entityType,entityId=entityId, outcomeId = learningOutcome.Id });
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
