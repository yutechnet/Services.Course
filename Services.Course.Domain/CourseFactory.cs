using System;
using System.Collections.Generic;
using AutoMapper;
using Autofac.Features.Indexed;
using BpeProducts.Common.NHibernate.Version;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
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

	    public Entities.Course BuildNewVersion(Entities.Course course, string version)
	    {
	        var newVersion = new Entities.Course
	            {
                    Id = Guid.NewGuid(),
                    OriginalEntityId = course.OriginalEntityId,
                    ParentEntityId = course.Id,
	                Name = course.Name,
	                Code = course.Code,
	                Description = course.Description,

	                Programs = new List<Program>(course.Programs),
	                Segments = new List<CourseSegment>(course.Segments),
	                Outcomes = new List<LearningOutcome>(course.Outcomes),

	                CourseSegmentJson = course.CourseSegmentJson,
	                TenantId = course.TenantId,
                    VersionNumber = version
	            };

	        return newVersion;
	    }
	}
}