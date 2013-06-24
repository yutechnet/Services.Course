using System;
using AutoMapper;
using Autofac.Features.Indexed;
using BpeProducts.Common.NHibernate.Version;
using BpeProducts.Services.Course.Contract;
using EventStore;

namespace BpeProducts.Services.Course.Domain
{
	public class CourseFactory : VersionFactory<Entities.Course>, ICourseFactory
	{
	    public CourseFactory(IStoreEvents store, IIndex<string, IPlayEvent> index) : base(store, index)
	    {
	    }

	    public Entities.Course Create(SaveCourseRequest request)
		{
		    var courseId = Guid.NewGuid();
			var course = new Entities.Course
			    {
                    Id = courseId,
                    ActiveFlag = true,
                    OriginalEntityId = courseId,
                    ParentEntityId = null,
                    VersionNumber = new Version(1, 0, 0, 0).ToString()
                };

			Mapper.Map(request, course);
			return course;
		}
	}
}