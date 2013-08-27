using System;
using System.Collections.Generic;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Events;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain.Courses.Events
{
    public class PlayCoursePrerequisiteAdded:IPlayEvent<CoursePrerequisiteAdded, Course>
    {
	    private readonly ICourseRepository _repository;

	    public PlayCoursePrerequisiteAdded(ICourseRepository repository)
	    {
		    _repository = repository;
	    }

	    public Course Apply(Domain.Events.CoursePrerequisiteAdded msg, Course course)
	    {
		    var prereq = _repository.Get(msg.PrerequisiteCourseId);
			course.AddPrerequisite(prereq);           
            return course;
        }

        public TE Apply<T, TE>(T msg, TE entity) where T : IDomainEvent where TE : class
        {
            return Apply(msg as CoursePrerequisiteAdded, entity as Course) as TE;
        }
    }
}