using System;
using System.Collections.Generic;
using BpeProducts.Services.Course.Contract;
using EventStore;

namespace BpeProducts.Services.Course.Domain
{
	public interface ICourseFactory
	{
		Courses.Course Build(SaveCourseRequest request);
	    Courses.Course Build(CreateCourseFromTemplateRequest request);
    }
}