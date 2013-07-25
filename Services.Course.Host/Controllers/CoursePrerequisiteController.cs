using System;
using System.Collections.Generic;
using System.Web.Http;
using BpeProducts.Common.WebApi.Attributes;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;

namespace BpeProducts.Services.Course.Host.Controllers
{
	[Authorize]
	public class CoursePrerequisiteController : ApiController
    {
		private readonly ICourseService _courseService;

		public CoursePrerequisiteController(ICourseService courseService)
        {
            _courseService = courseService;
        }

		[Transaction]
		[CheckModelForNull]
		[ValidateModelState]
        [HttpPut]
		// PUT api/courses/5/prerequisites
		public void Put(Guid id, UpdateCoursePrerequisites prerequisites)
		{
			_courseService.UpdatePrerequisiteList(id, prerequisites.PrerequisiteIds);
		}
    }
}
