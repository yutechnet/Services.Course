using System;
using AutoMapper;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.Repositories;
using Services.Assessment.Contract;

namespace BpeProducts.Services.Course.Domain.Courses
{
	public interface ICourseRubricService
	{
		CourseRubricInfo Get(Guid courseId, Guid segmentId, Guid learningActivityId, Guid courseRubricId);
		CourseRubricInfo AddRubric(Guid courseId, Guid segmentId, Guid learningActivityId, CourseRubricRequest request);
		void DeleteRubric(Guid courseId, Guid segmentId, Guid learningActivityId, Guid rubricId);
	}

	public class CourseRubricService : ICourseRubricService
	{
		private readonly ICourseRepository _courseRepository;
		private readonly IAssessmentClient _assessmentClient;

		public CourseRubricService(ICourseRepository courseRepository, IAssessmentClient assessmentClient)
		{
			_courseRepository = courseRepository;
			_assessmentClient = assessmentClient;
		}

		public CourseRubricInfo Get(Guid courseId, Guid segmentId, Guid learningActivityId, Guid courseRubricId)
		{
			var courseRubric = _courseRepository.GetCourseRubric(courseRubricId);
			return Mapper.Map<CourseRubricInfo>(courseRubric);
		}

		public CourseRubricInfo AddRubric(Guid courseId, Guid segmentId, Guid learningActivityId, CourseRubricRequest request)
		{
			var course = _courseRepository.GetOrThrow(courseId);

			var rubric = _assessmentClient.GetRubric(request.RubricId);

			if (!rubric.IsPublished)
			{
				throw new BadRequestException(string.Format("Rubric {0} is not published, and thus cannot be associated with learning activities.", rubric.Id));
			}

			var learningMaterial = course.AddCourseRubric(segmentId, learningActivityId, request);
			return Mapper.Map<CourseRubricInfo>(learningMaterial);
		}

		public void DeleteRubric(Guid courseId, Guid segmentId, Guid learningActivityId, Guid rubricId)
		{
			var course = _courseRepository.GetOrThrow(courseId);
			course.DeleteCourseRubric(segmentId, learningActivityId, rubricId);
		}
	}
}
