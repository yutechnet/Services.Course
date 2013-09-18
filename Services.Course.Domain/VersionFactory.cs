﻿using System;
using System.Collections.Generic;
using Autofac.Features.Indexed;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Domain.Events;
using EventStore;
using Newtonsoft.Json;

namespace BpeProducts.Services.Course.Domain
{
    public abstract class VersionFactory<T> where T : class
    {
        private readonly IStoreEvents _store;
        public readonly IIndex<string, IPlayEvent> Index;
        private readonly IRepository _repository;

        protected VersionFactory(IStoreEvents store, IIndex<string, IPlayEvent> index,IRepository repository)
        {
            _store = store;
            Index = index;
            _repository = repository;
        }

        public T Reconstitute(Guid aggregateId)
        {
            T entity = _repository.Get<T>(aggregateId);
            
            //TODO complete refactor not to use Reconstitute at all?
            // Get the latest snapshot
            //Snapshot latestSnapshot = _store.Advanced.GetSnapshot(aggregateId, int.MaxValue);
            //if (latestSnapshot == null)
            //{
            //    using (IEventStream stream = _store.OpenStream(aggregateId, 0, int.MaxValue))
            //    {
            //        //throw if it is a new
            //        entity = Reconstitute(stream.CommittedEvents);
            //    }
            //}
            //else
            //{
            //    using (IEventStream stream = _store.OpenStream(latestSnapshot, int.MaxValue))
            //    {
            //        //throw if it is a new
            //        entity = Reconstitute(stream.CommittedEvents, latestSnapshot.Payload as T);
            //    }
            //}

            return entity;
        }

        public T Reconstitute(ICollection<EventMessage> events, T entity = null)
        {
            if (events.Count == 0)
                return null;

            if (entity == null)
                entity = Activator.CreateInstance<T>();

            foreach (EventMessage eventMessage in events)
            {
                object eventBody = eventMessage.Body;
                var eventPlayer = Index[eventBody.GetType().Name];
                entity = eventPlayer.Apply(eventBody as IDomainEvent, entity);
            }

            return entity;
        }
    }
}