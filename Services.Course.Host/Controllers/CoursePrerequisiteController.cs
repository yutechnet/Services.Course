using System;
using System.Collections.Generic;
using System.Web.Http;
using AttributeRouting.Web.Http;
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
        [PUT("course/{courseId:guid}/prerequisites")]
		public void Put(Guid courseId, UpdateCoursePrerequisites prerequisites)
		{
            _courseService.UpdatePrerequisiteList(courseId, prerequisites.PrerequisiteIds);
		}
    }
}
