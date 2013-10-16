using System;
using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Host.Tests.Unit.Handlers
{
	public class FakeDomainEvent : IDomainEvent
	{
		public Guid AggregateId { get; set; }
	}
}

