using System;
using System.Web.Http;
using BpeProducts.Common.WebApi.NHibernate;
using BpeProducts.Common.WebApi.Validation;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain;
using BpeProducts.Services.Course.Domain.Courses;

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
		[ArgumentsNotNull]
		[ValidateModelState]
        [Route("course/{courseId:guid}/prerequisite")]
		public void Put(Guid courseId, UpdateCoursePrerequisites prerequisites)
		{
            _courseService.UpdatePrerequisiteList(courseId, prerequisites.PrerequisiteIds);
		}
    }
}
