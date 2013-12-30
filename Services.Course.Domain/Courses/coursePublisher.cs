﻿using System.Collections.Generic;
using BpeProducts.Services.Asset.Contracts;
using Services.Assessment.Contract;

namespace BpeProducts.Services.Course.Domain.Courses
{
    public interface ICoursePublisher
    {
        void Publish(Course course, string publishNote);
        void PublishAssesments(IList<CourseSegment> segments, string publishNote);
        void PublishLearningMaterialAsset(IList<CourseSegment> segments, string publishNote);
    }

    public class CoursePublisher : ICoursePublisher
    {
        private readonly IAssessmentClient _assessmentClient;
        private readonly IAssetServiceClient _assetServiceClient;
        //the linked resources are 
        //1. assesmments
        //2. learning materials

        public CoursePublisher(IAssessmentClient assessmentClient, IAssetServiceClient assetServiceClient)
        {
            _assessmentClient = assessmentClient;
            _assetServiceClient = assetServiceClient;
        }

        public void Publish(Course course, string publishNote)
        {
            PublishAssesments(course.Segments, publishNote);
            PublishLearningMaterialAsset(course.Segments, publishNote);
        }

        public void PublishAssesments(IList<CourseSegment> segments, string publishNote)
        {
            foreach (var segment in segments)
            {
                foreach (var learningActivity in segment.CourseLearningActivities)
                {
                    if (learningActivity.AssessmentType != AssessmentType.Custom)
                    {
                        _assessmentClient.PublishAssessment(learningActivity.AssessmentId, publishNote);
                    }
                }
                if (segment.ChildSegments.Count > 0) PublishAssesments(segment.ChildSegments, publishNote);
            }
        }
        public void PublishLearningMaterialAsset(IList<CourseSegment> segments, string publishNote)
        {
            foreach (var segment in segments)
            {
                foreach (var learningMaterials in segment.LearningMaterials)
                {
                    _assetServiceClient.PublishAsset(learningMaterials.AssetId, publishNote);
                }
                if (segment.ChildSegments.Count > 0) PublishLearningMaterialAsset(segment.ChildSegments, publishNote);
            }
        }
    }
}