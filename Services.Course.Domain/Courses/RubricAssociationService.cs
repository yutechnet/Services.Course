using System;
using AutoMapper;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Repositories;
using Services.Assessment.Contract;

namespace BpeProducts.Services.Course.Domain.Courses
{
	public interface IRubricAssociationService
	{
		RubricAssociationInfo Get(Guid courseId, Guid segmentId, Guid learningActivityId, Guid rubricassociationId);
		RubricAssociationInfo AddRubric(Guid courseId, Guid segmentId, Guid learningActivityId, RubricAssociationRequest request);
		void DeleteRubric(Guid courseId, Guid segmentId, Guid learningActivityId, Guid rubricId);
	}

	public class RubricAssociationService : IRubricAssociationService
	{
		private readonly ICourseRepository _courseRepository;
		private readonly IAssessmentClient _assessmentClient;

		public RubricAssociationService(ICourseRepository courseRepository, IAssessmentClient assessmentClient)
		{
			_courseRepository = courseRepository;
			_assessmentClient = assessmentClient;
		}

		public RubricAssociationInfo Get(Guid courseId, Guid segmentId, Guid learningActivityId, Guid rubricassociationId)
		{
			var rubricAssociation = _courseRepository.GetRubricAssociation(rubricassociationId);
			return Mapper.Map<RubricAssociationInfo>(rubricAssociation);
		}

		public RubricAssociationInfo AddRubric(Guid courseId, Guid segmentId, Guid learningActivityId, RubricAssociationRequest request)
		{
			var course = _courseRepository.GetOrThrow(courseId);

			var rubric = _assessmentClient.GetRubric(new Uri("http://google.com"), request.RubricId);

			//TODO: Enable once rubric versioning/publishing is enabled
			//if (!rubric.IsPublished)
			//{
			//	throw new BadRequestException(string.Format("Rubric {0} is not published, and thus cannot be associated with learning activities.", rubric.Id));
			//}

			var learningMaterial = course.AddRubricAssociation(segmentId, learningActivityId, request);
			return Mapper.Map<RubricAssociationInfo>(learningMaterial);
		}

		public void DeleteRubric(Guid courseId, Guid segmentId, Guid learningActivityId, Guid rubricId)
		{
			var course = _courseRepository.GetOrThrow(courseId);
			course.DeleteRubricAssociation(segmentId, learningActivityId, rubricId);
		}
	}
}
