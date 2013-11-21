using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting.Web.Http;
using BpeProducts.Common.WebApi.Attributes;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Courses;

namespace BpeProducts.Services.Course.Host.Controllers
{
	[Authorize]
	public class RubricAssociationController : ApiController
	{
		private readonly IRubricAssociationService _rubricAssociationService;

		public RubricAssociationController(IRubricAssociationService rubricAssociationService)
		{
			_rubricAssociationService = rubricAssociationService;
		}

		[Transaction]
		[CheckModelForNull]
		[ValidateModelState]
		[HttpPost]
		[POST("course/{courseId:guid}/segments/{segmentId:guid}/learningactivity/{learningActivityId:guid}/rubricassociation")]
		public HttpResponseMessage Post(Guid courseId, Guid segmentId, Guid learningActivityId, RubricAssociationRequest request)
		{
			var rubricAssociation = _rubricAssociationService.AddRubric(courseId, segmentId, learningActivityId, request);

			var response = Request.CreateResponse(HttpStatusCode.Created, rubricAssociation);

			var uri = Url.Link("GetRubricAssociation", new { courseId, segmentId, learningActivityId, rubricAssociationId = rubricAssociation.Id });
			if (uri != null)
			{
				response.Headers.Location = new Uri(uri);
			}
			return response;
		}

		[HttpGet]
		[GET("course/{courseId:guid}/segments/{segmentId:guid}/learningactivity/{learningActivityId:guid}/rubricassociation/{rubricassociationId:guid}", RouteName = "GetRubricAssociation")]
		public RubricAssociationInfo Get(Guid courseId, Guid segmentId, Guid learningActivityId, Guid rubricassociationId)
		{
			return _rubricAssociationService.Get(courseId, segmentId, learningActivityId, rubricassociationId);
		}

		[HttpGet]
		[GET("course/{courseId:guid}/segments/{segmentId:guid}/learningactivity/{learningActivityId:guid}/rubricassociation/{rubricId:guid}", RouteName = "GetRubricAssociation")]
		public void Delete(Guid courseId, Guid segmentId, Guid learningActivityId, Guid rubricId)
		{
			_rubricAssociationService.DeleteRubric(courseId, segmentId, learningActivityId, rubricId);
		}
	}
}