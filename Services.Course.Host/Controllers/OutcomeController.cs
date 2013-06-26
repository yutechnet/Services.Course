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
        private readonly ILearningOutcomeRepository _learningOutcomeRepository;
	    private readonly IRepository _repository;
        private readonly IOutcomeFactory _outcomeFactory;
        private readonly IDomainEvents _domainEvents;

        public OutcomeController(ILearningOutcomeRepository learningOutcomeRepository, IRepository repository, IOutcomeFactory outcomeFactory, IDomainEvents domainEvents)
        {
	        _learningOutcomeRepository = learningOutcomeRepository;
		    _repository = repository;
		    _outcomeFactory = outcomeFactory;
            _domainEvents = domainEvents;
        }

		[Transaction]
	    public OutcomeResponse Get(Guid id)
		{
		    var learningOutcome =
		        _learningOutcomeRepository.Load(id);
            if (learningOutcome == null || learningOutcome.ActiveFlag == false)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return Mapper.Map<OutcomeResponse>(learningOutcome);
        }

	    [Transaction]
        public OutcomeResponse Get(string entityType, Guid entityId, Guid outcomeId)
	    {
			//apply strategy based on entitytype

		    IHaveOutcomes entity =
			    _repository.Query<IHaveOutcomes>().SingleOrDefault(e => e.Id == entityId);
		    
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
		public List<OutcomeResponse> Get(string entityType, Guid entityId)
		{
			IHaveOutcomes entity =
				_repository.Query<IHaveOutcomes>().SingleOrDefault(e => e.Id == entityId);

			if (entity == null)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound);
			}

			var outcomes = entity.Outcomes;
			if (outcomes == null)
			{
				outcomes=new List<LearningOutcome>();
			}

			return Mapper.Map<List<OutcomeResponse>>(outcomes);
		}

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        [ClaimsAuthorize()]
        public HttpResponseMessage Post(OutcomeRequest request)
        {
            var learningOutcome = _outcomeFactory.Build(request);
            _domainEvents.Raise<OutcomeCreated>(new OutcomeCreated
            {
                AggregateId = learningOutcome.Id,
                Description = learningOutcome.Description,
                ActiveFlag = learningOutcome.ActiveFlag,
                Outcome = learningOutcome
            });

            //var learningOutcome = Mapper.Map<LearningOutcome>(request);
            //_learningOutcomeRepository.Add(learningOutcome);

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
        public HttpResponseMessage Post(string entityType, Guid entityId, OutcomeRequest request)
		{
			//check if the entity exists
			//apply strategy pattern here?
			var entity = _repository.Query<IHaveOutcomes>().SingleOrDefault(x => x.Id == entityId);
			if (entity==null) throw new HttpResponseException(HttpStatusCode.NotFound);

			var learningOutcome=entity.Outcomes.SingleOrDefault(o => o.Description.ToLower() == request.Description.ToLower());
			if (learningOutcome==null)
			{
                learningOutcome = _outcomeFactory.Build(request);
                _domainEvents.Raise<OutcomeCreated>(new OutcomeCreated
                {
                    AggregateId = learningOutcome.Id,
                    Description = learningOutcome.Description,
                    ActiveFlag = learningOutcome.ActiveFlag,
                    Outcome = learningOutcome
                });
                
                ////create an outcome and associate to an entity
                //learningOutcome = Mapper.Map<LearningOutcome>(request);
                ////learningOutcome.Id = Guid.NewGuid();
                //_repository.Save(learningOutcome);

				entity.Outcomes.Add(learningOutcome);

				_repository.Save(entity);
				
			}
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
            LearningOutcome learningOutcome = _outcomeFactory.Reconstitute(id);

            if (learningOutcome == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            if (learningOutcome.IsPublished)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    ReasonPhrase = string.Format("Learning outcome {0} is published and cannot be modified.", id)
                });
            }

            _domainEvents.Raise<OutcomeUpdated>(new OutcomeUpdated
            {
                AggregateId = id,
                Description = request.Description
            });
        }

        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        public void Put(string entityType, Guid entityId, Guid outcomeId, OutcomeRequest request)
        {
            //check if the entity exists
            //apply strategy pattern here?
            var entity = _repository.Query<IHaveOutcomes>().SingleOrDefault(x => x.Id == entityId);
            if (entity == null) throw new HttpResponseException(new HttpResponseMessage
                {
                    ReasonPhrase = string.Format("Entity \"{0}\" with id {1} not found", entityId, entityId),
                    StatusCode = HttpStatusCode.NotFound
                });

            var learningOutcome = entity.Outcomes.SingleOrDefault(o => o.Id == outcomeId);
            if (learningOutcome == null)
            {
                learningOutcome =
                    _learningOutcomeRepository.Load(outcomeId);
                if (learningOutcome == null || learningOutcome.ActiveFlag == false)
                {
                    throw new HttpResponseException(new HttpResponseMessage
                        {
                            ReasonPhrase = string.Format("Outcome with id {0} not found.", outcomeId),
                            StatusCode = HttpStatusCode.NotFound
                        });                    
                }
                entity.Outcomes.Add(learningOutcome);

                _repository.Update(entity);

            }
        }

        [Transaction]
        public void Delete(Guid id)
        {
            var outcomeInDb = _outcomeFactory.Reconstitute(id);

            if (outcomeInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            if (outcomeInDb.IsPublished)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    ReasonPhrase = string.Format("Learning outcome {0} is published and cannot be deleted.", id)
                });
            }

            _domainEvents.Raise<OutcomeDeleted>(new OutcomeDeleted
            {
                AggregateId = id,
            });
        }

		[Transaction]
		public void Delete(string entityType, Guid entityId, Guid outcomeId)
		{
			var entity = _repository.Query<IHaveOutcomes>().SingleOrDefault<IHaveOutcomes>(x => x.Id == entityId);
			if (entity == null)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound);
			}
			var outcome = entity.Outcomes.SingleOrDefault(o => o.Id == outcomeId);
			if (outcome==null) throw new HttpResponseException(HttpStatusCode.NotFound);
			entity.Outcomes.Remove(outcome);
			_repository.Update(entity);

		}

	  
    }
}
