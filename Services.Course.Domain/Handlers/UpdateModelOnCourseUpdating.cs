using AutoMapper;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Handlers
{
	public class UpdateModelOnCourseUpdating : IHandle<CourseUpdated>
	{
		private readonly ICourseRepository _courseRepository;
		public UpdateModelOnCourseUpdating(ICourseRepository courseRepository)
		{
			_courseRepository = courseRepository;
		}
		public void Handle(IDomainEvent domainEvent)
		{
			var e = (CourseUpdated)domainEvent;
			//update model
			var courseInDb = _courseRepository.GetById(e.AggregateId);
			Mapper.Map(e.Request,courseInDb);
		
			courseInDb.Programs.Clear();
			foreach (var p in e.Request.ProgramIds)
			{
				var program = _courseRepository.Load<Program>(p);
				courseInDb.Programs.Add(program);
			}

			_courseRepository.Update(courseInDb);

		}
	}
}