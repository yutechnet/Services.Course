using System;
using BpeProducts.Common.NHibernate;

namespace BpeProducts.Services.Course.Domain
{
	public abstract class VersionFactory<T> where T : class
	{
		private readonly IRepository _repository;

		protected VersionFactory(IRepository repository)
		{
			_repository = repository;
		}

		public T Reconstitute(Guid aggregateId)
		{
			var entity = _repository.Get<T>(aggregateId);

			return entity;
		}
	}
}