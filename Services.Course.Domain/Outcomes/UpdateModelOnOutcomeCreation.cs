using System;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;

namespace BpeProducts.Services.Course.Domain.Outcomes
{
	public class UpdateModelOnOutcomeCreation : IHandle<OutcomeCreated>
	{
		private readonly IRepository _repository;

		public UpdateModelOnOutcomeCreation(IRepository repository)
		{
			_repository = repository;
		}

		public void Handle(IDomainEvent domainEvent)
		{
			var e = domainEvent as OutcomeCreated;
			if (e == null)
			{
				throw new InvalidOperationException("Invalid domain event.");
			}

			_repository.Save(e.Outcome);
		}
	}
}