using System;
using BpeProducts.Common.NHibernate;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Handlers;

namespace BpeProducts.Services.Course.Domain.Outcomes
{
	public class UpdateModelOnOutcomeVersionCreation : IHandle<OutcomeVersionCreated>
	{
		private readonly IRepository _repository;

		public UpdateModelOnOutcomeVersionCreation(IRepository repository)
		{
			_repository = repository;
		}

		public void Handle(IDomainEvent domainEvent)
		{
			var e = domainEvent as OutcomeVersionCreated;
			if (e == null)
			{
				throw new InvalidOperationException("Invalid domain event.");
			}

			_repository.Save(e.NewVersion);
		}
	}
}