using System;
using System.Collections.Generic;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Course.Domain.Validation;
using Services.Assessment.Contract;

namespace BpeProducts.Services.Course.Domain.CourseAggregates
{
    public interface ICoursePublisher
    {
        void Publish(Course course, string publishNote);
        void PublishAssesments(IList<CourseSegment> segments, string publishNote);
        void PublishCourseLearningMaterialAsset(Course course, string publishNote);
    }

	public class CoursePublisher : ICoursePublisher
	{
		private readonly IAssessmentClient _assessmentClient;
		private readonly IAssetServiceClient _assetServiceClient;
		private readonly IValidator<Course> _coursePublishValidator;
		//the linked resources are 
		//1. assesmments
		//2. learning materials

		public CoursePublisher(IAssessmentClient assessmentClient, IAssetServiceClient assetServiceClient, IValidator<Course> coursePublishValidator)
		{
			_assessmentClient = assessmentClient;
			_assetServiceClient = assetServiceClient;
			_coursePublishValidator = coursePublishValidator;
		}

	    public void Publish(Course course, string publishNote)
	    {
	        IEnumerable<string> brokenRules;
	        var isValid = course.Validate(_coursePublishValidator, out brokenRules);

	        if (!isValid)
	            throw new BadRequestException(string.Join("\n", brokenRules));

            PublishAssesments(course.Segments, publishNote);
            PublishCourseLearningMaterialAsset(course, publishNote);
        }

	    public void PublishAssesments(IList<CourseSegment> segments, string publishNote)
		{
			foreach (var segment in segments)
			{
				foreach (var learningActivity in segment.CourseLearningActivities)
				{
					if (learningActivity.AssessmentId.HasValue && learningActivity.AssessmentId != Guid.Empty && AssessmentNotPublished(learningActivity.AssessmentId.Value))
					{
						_assessmentClient.PublishAssessment(learningActivity.AssessmentId.Value, publishNote);
					}
				}
				if (segment.ChildSegments.Count > 0) PublishAssesments(segment.ChildSegments, publishNote);
			}
		}

		private bool AssessmentNotPublished(Guid assessmentId)
		{
			AssessmentInfo assessment = _assessmentClient.GetAssessment(assessmentId);
			return assessment.IsPublished == false;
		}

        public void PublishCourseLearningMaterialAsset(Course course, string publishNote)
        {
            foreach (var lm in course.LearningMaterials)
            {
                PublishAsset(lm.AssetId, publishNote);
            }
            PublishSegmentLearningMaterialAsset(course.Segments, publishNote);
        }

        private void PublishSegmentLearningMaterialAsset(IList<CourseSegment> segments, string publishNote)
        {
            foreach (var segment in segments)
            {
                foreach (var lm in segment.LearningMaterials)
                {
                    PublishAsset(lm.AssetId, publishNote);
                }

                if (segment.ChildSegments.Count > 0) PublishSegmentLearningMaterialAsset(segment.ChildSegments, publishNote);
            }

        }

		public  void PublishAsset(Guid assetId,string publishNote)
		{
			if (!CheckAssetIsPublished(assetId))
				_assetServiceClient.PublishAsset(assetId,publishNote);
		}

		private bool CheckAssetIsPublished(Guid assetId)
		{
			var asset = _assetServiceClient.GetAsset(assetId);
			return asset.IsPublished;
		}

	}
}