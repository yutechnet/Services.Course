using System;
using System.Collections.Generic;
using System.Transactions;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;
using EventStore;
using EventStore.Persistence;
using EventStore.Persistence.SqlPersistence.SqlDialects;
using Newtonsoft.Json;

namespace BpeProducts.Services.Course.Domain.Repositories
{
    public class CourseEventStore : IStoreCourseEvents
    {
        private readonly IStoreEvents _eventStore;

        public CourseEventStore()
        {
            _eventStore = Wireup.Init()
                                .LogToOutputWindow()
                                //.UsingInMemoryPersistence()
                                .UsingSqlPersistence("DefaultConnection") // Connection string is in app.config
                                .WithDialect(new MsSqlDialect())
                                .EnlistInAmbientTransaction() // two-phase commit
                                .InitializeStorageEngine()
                                //.TrackPerformanceInstance("example")
                                //.UsingJsonSerialization()
                                // Use the custom JsonSerializer because the built-in one does not support self-reference loop
                                .UsingCustomSerialization(new CustomJsonSerializer())
                                .Compress()
                                //.EncryptWith(EncryptionKey)
                                //.HookIntoPipelineUsing(new[] { new AuthorizationPipelineHook() })
                                .UsingSynchronousDispatchScheduler()
                                //.DispatchTo(new DelegateMessageDispatcher(DispatchCommit))
                                .Build();
        }

        public void Dispose()
        {
            _eventStore.Dispose();
        }

        public IEventStream CreateStream(Guid streamId)
        {
            return _eventStore.CreateStream(streamId);
        }

        public IEventStream OpenStream(Guid streamId, int minRevision, int maxRevision)
        {
            return _eventStore.OpenStream(streamId, minRevision, maxRevision);
        }

        public IEventStream OpenStream(Snapshot snapshot, int maxRevision)
        {
            return _eventStore.OpenStream(snapshot, maxRevision);
        }

        public IPersistStreams Advanced
        {
            get { return _eventStore.Advanced; }
        }

        public void Store<T>(T domainEvent) where T : IDomainEvent
        {

            using (var scope = new TransactionScope())
            using (_eventStore)
            {
                using (IEventStream stream = _eventStore.OpenStream(domainEvent.AggregateId, 0, int.MaxValue))
                {

                    stream.Add(new EventMessage { Body = domainEvent });
                    stream.CommitChanges(Guid.NewGuid());
                }
                scope.Complete();
            }
        }

        public void Store<T>(Guid aggregateId, List<T> domainEvents) where T : IDomainEvent
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            using (_eventStore)
            {
                using (IEventStream stream = _eventStore.OpenStream(aggregateId, 0, int.MaxValue))
                {
                    foreach (T domainEvent in domainEvents)
                    {
                        stream.Add(new EventMessage { Body = domainEvent });
                        stream.CommitChanges(Guid.NewGuid());
                    }
                }
                scope.Complete();
            }
        }

    }
}