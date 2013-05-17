using BpeProducts.Services.Course.Domain.Events;

namespace BpeProducts.Services.Course.Domain
{
	public interface IPlayEvent<in T>:IPlayEvent
	{
		Entities.Course Apply(T msg, Entities.Course course);
	}

	public interface IPlayEvent
	{
		Entities.Course Apply<T>(T msg, Entities.Course course) where T:IDomainEvent;
	}


}