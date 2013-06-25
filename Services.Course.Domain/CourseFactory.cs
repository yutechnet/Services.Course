﻿using System;
using System.Collections.Generic;
using AutoMapper;
using Autofac.Features.Indexed;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate.Version;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Entities;
using BpeProducts.Services.Course.Domain.Repositories;
using EventStore;
using NHibernate;

namespace BpeProducts.Services.Course.Domain
{
	public class CourseFactory : VersionFactory<Entities.Course>, ICourseFactory
	{
	    private readonly ICourseRepository _courseRepository;

	    public CourseFactory(IStoreEvents store, IIndex<string, IPlayEvent> index, ICourseRepository courseRepository) : base(store, index)
	    {
	        _courseRepository = courseRepository;
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
	        var existing = _courseRepository.GetVersion(course.OriginalEntityId, version);

            if(existing != null)
                throw new BadRequestException(string.Format("Course version {0} already exists for CourseId {1}", version, course.OriginalEntityId));

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