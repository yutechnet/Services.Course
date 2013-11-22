using System;
using AutoMapper;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Repositories;

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

		public RubricAssociationService(ICourseRepository courseRepository)
		{
			_courseRepository = courseRepository;
		}

		public RubricAssociationInfo Get(Guid courseId, Guid segmentId, Guid learningActivityId, Guid rubricassociationId)
		{
			var rubricAssociation = _courseRepository.GetRubricAssociation(rubricassociationId);
			return Mapper.Map<RubricAssociationInfo>(rubricAssociation);
		}

		public RubricAssociationInfo AddRubric(Guid courseId, Guid segmentId, Guid learningActivityId, RubricAssociationRequest request)
		{
			var course = _courseRepository.GetOrThrow(courseId);

			// TODO: Get rubric and ensure viability

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
