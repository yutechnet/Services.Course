﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using AutoMapper;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Outcomes;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain
{
    public class LearningOutcomeService : ILearningOutcomeService, IHandleVersioning<LearningOutcome>
    {
        private readonly ILearningOutcomeRepository _learningOutcomeRepository;
        private readonly IRepository _repository;
        private readonly IOutcomeFactory _outcomeFactory;
        private readonly IDomainEvents _domainEvents;

        public LearningOutcomeService(ILearningOutcomeRepository learningOutcomeRepository, IRepository repository, 
            IOutcomeFactory outcomeFactory, IDomainEvents domainEvents)
        {
            _learningOutcomeRepository = learningOutcomeRepository;
            _repository = repository;
            _outcomeFactory = outcomeFactory;
            _domainEvents = domainEvents;
        }

        public OutcomeResponse Get(Guid outcomeId)
        {
            var learningOutcome = _learningOutcomeRepository.Get(outcomeId);
            if (learningOutcome == null || !learningOutcome.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Learning outcome {0} not found.", outcomeId));
            }

            return Mapper.Map<OutcomeResponse>(learningOutcome);
        }

        public OutcomeResponse Get(string entityType, Guid entityId, Guid outcomeId)
        {
            var entity =
                _repository.Query<IHaveOutcomes>().SingleOrDefault(e => e.Id == entityId);

            if (entity == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var outcome = entity.Outcomes
                                            .SingleOrDefault(l => l.Id == outcomeId);

            if (outcome == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return Mapper.Map<OutcomeResponse>(outcome);
        }

        public IEnumerable<OutcomeResponse> Get(string entityType, Guid entityId)
        {
            var entity =
                _repository.Query<IHaveOutcomes>().SingleOrDefault(e => e.Id == entityId);

            if (entity == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var outcomes = entity.Outcomes ?? new List<LearningOutcome>();

            return Mapper.Map<List<OutcomeResponse>>(outcomes);
        }

        public OutcomeResponse Create(OutcomeRequest request)
        {
            var learningOutcome = _outcomeFactory.Build(request);
            _domainEvents.Raise<OutcomeCreated>(new OutcomeCreated
            {
                AggregateId = learningOutcome.Id,
                Description = learningOutcome.Description,
                ActiveFlag = learningOutcome.ActiveFlag,
                Outcome = learningOutcome
            });

            return Mapper.Map<OutcomeResponse>(learningOutcome);
        }

        public OutcomeResponse Create(string entityType, Guid entityId, OutcomeRequest request)
        {
            var entity = _repository.Query<IHaveOutcomes>().SingleOrDefault(x => x.Id == entityId);
            if (entity == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var learningOutcome = entity.Outcomes.SingleOrDefault(o => o.Description.ToLower() == request.Description.ToLower());
            if (learningOutcome == null)
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

            return Mapper.Map<OutcomeResponse>(learningOutcome);
        }

        public void Update(Guid outcomeId, OutcomeRequest request)
        {
            LearningOutcome learningOutcome = _outcomeFactory.Reconstitute(outcomeId);

            if (learningOutcome == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            if (learningOutcome.IsPublished)
            {
                throw new ForbiddenException(string.Format("Learning outcome {0} is published and cannot be modified.", outcomeId));
            }

            _domainEvents.Raise<OutcomeUpdated>(new OutcomeUpdated
            {
                AggregateId = outcomeId,
                Description = request.Description
            });
        }

        public void Update(string entityType, Guid entityId, Guid outcomeId, OutcomeRequest request)
        {
            var entity = _repository.Query<IHaveOutcomes>().SingleOrDefault(x => x.Id == entityId);

            if (entity == null)
                throw new NotFoundException(string.Format("Entity \"{0}\" with id {1} not found", entityId, entityId));

            var learningOutcome = entity.Outcomes.SingleOrDefault(o => o.Id == outcomeId);
            if (learningOutcome == null)
            {
                learningOutcome =
                    _learningOutcomeRepository.Get(outcomeId);
                if (learningOutcome == null || learningOutcome.ActiveFlag == false)
                {
                    throw new NotFoundException(string.Format("Outcome with id {0} not found.", outcomeId));
                }
                entity.Outcomes.Add(learningOutcome);

                _repository.Update(entity);
            }
        }

        public void Delete(Guid outcomeId)
        {
            var outcomeInDb = _outcomeFactory.Reconstitute(outcomeId);

            if (outcomeInDb == null || !outcomeInDb.ActiveFlag)
            {
                throw new NotFoundException(string.Format("Learning outcome {0} not found.", outcomeId));
            }

            if (outcomeInDb.IsPublished)
            {
                throw new ForbiddenException(string.Format("Learning outcome {0} is published and cannot be deleted.", outcomeId));
            }

            _domainEvents.Raise<OutcomeDeleted>(new OutcomeDeleted
            {
                AggregateId = outcomeId
            });
        }

        public void Delete(string entityType, Guid entityId, Guid outcomeId)
        {
            var entity = _repository.Query<IHaveOutcomes>().SingleOrDefault<IHaveOutcomes>(x => x.Id == entityId);
            if (entity == null)
            {
                throw new NotFoundException(string.Format("{0} {1} not found.", entityType, entityId));
            }
            var outcome = entity.Outcomes.SingleOrDefault(o => o.Id == outcomeId);
            if (outcome == null)
            {
                throw new NotFoundException(string.Format("Learing outcome {0} not found.", outcomeId));
            }
            entity.Outcomes.Remove(outcome);
            _repository.Update(entity);
        }

        public void PublishVersion(Guid entityId, string publishNote)
        {
            var courseInDb = _outcomeFactory.Reconstitute(entityId);

            if (courseInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            _domainEvents.Raise<OutcomeVersionPublished>(new OutcomeVersionPublished
            {
                AggregateId = entityId,
                PublishNote = publishNote
            });
        }

        public LearningOutcome CreateVersion(VersionRequest request)
        {
            var outcomeInDb = _learningOutcomeRepository.Get(request.ParentVersionId);
            if (outcomeInDb == null)
            {
                throw new NotFoundException(string.Format("Parent version {0} not found.", request.ParentVersionId));
            }

            var newVersion = _outcomeFactory.BuildNewVersion(outcomeInDb, request.VersionNumber);

            _domainEvents.Raise<OutcomeVersionCreated>(new OutcomeVersionCreated
            {
                AggregateId = newVersion.Id,
                NewVersion = newVersion
            });

            return newVersion;
        }
    }
}