using System;
using System.Linq;
using AutoMapper;
using System.Collections.Generic;
using BpeProducts.Common.Authorization;
using BpeProducts.Services.Authorization.Contract;
using BpeProducts.Services.Course.Contract;
using BpeProducts.Services.Course.Domain.CourseAggregates;
using BpeProducts.Services.Course.Domain.Repositories;

namespace BpeProducts.Services.Course.Domain
{
    public class CourseLearningActivityService : ICourseLearningActivityService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IAssessmentClient _assessmentClient;

        public CourseLearningActivityService(ICourseRepository courseRepository, IAssessmentClient assessmentClient)
        {
            _courseRepository = courseRepository;
            _assessmentClient = assessmentClient;
        }

        [AuthByAcl(Capability = Capability.CourseView, ObjectId = "courseId", ObjectType = typeof(CourseAggregates.Course))]
        public CourseLearningActivityResponse Get(Guid courseId, Guid segmentId, Guid learningActivityId)
        {
            var course = _courseRepository.Get(courseId);

            var learningActivity = course.GetLearningActivity(segmentId, learningActivityId);
     
            return Mapper.Map<CourseLearningActivityResponse>(learningActivity);
        }

        [AuthByAcl(Capability = Capability.CourseView, ObjectId = "courseId", ObjectType = typeof(CourseAggregates.Course))]
        public IEnumerable<CourseLearningActivityResponse> Get(Guid courseId, Guid segmentId)
        {
            var course = _courseRepository.Get(courseId);

            var learningActivity = course.GetLearningActivity(segmentId);

            return Mapper.Map<IList<CourseLearningActivityResponse>>(learningActivity);
        }

        [AuthByAcl(Capability = Capability.EditCourse, ObjectId = "courseId", ObjectType = typeof(CourseAggregates.Course))]
        public void Update(Guid courseId, Guid segmentId, Guid learningActivityId, SaveCourseLearningActivityRequest request)
        {
            var course = _courseRepository.Get(courseId);

            if (request.AssessmentId.HasValue && request.AssessmentId != Guid.Empty)
            {
               var assessmentResponse = _assessmentClient.GetAssessment(request.AssessmentId.Value);
               request.AssessmentType = assessmentResponse.AssessmentType.ToString();
            }

            course.UpdateLearningActivity(segmentId, learningActivityId, request);
            _courseRepository.Save(course);
        }

        [AuthByAcl(Capability = Capability.EditCourse, ObjectId = "courseId", ObjectType = typeof(CourseAggregates.Course))]
        public void Delete(Guid courseId, Guid segmentId, Guid learningActivityId)
        {
            var course = _courseRepository.Get(courseId);
            course.DeleteLearningActivity(segmentId, learningActivityId);
            _courseRepository.Save(course);
        }

        [AuthByAcl(Capability = Capability.EditCourse, ObjectId = "courseId", ObjectType = typeof(CourseAggregates.Course))]
        public CourseLearningActivityResponse Create(Guid courseId, Guid segmentId, SaveCourseLearningActivityRequest request)
        {
            var course = _courseRepository.Get(courseId);

            if (request.AssessmentId.HasValue && request.AssessmentId != Guid.Empty)
            {
                var assessmentResponse = _assessmentClient.GetAssessment(request.AssessmentId.Value);
                if (assessmentResponse != null)
                {
                    // TODO: What if the assessmentType from Assessment does not match with the assessmentType in the request?
                    // Throw and exception or override?
                    request.AssessmentType = assessmentResponse.AssessmentType.ToString();
                }
            }

            var learningActivity = course.AddLearningActivity(segmentId, request);
            _courseRepository.Save(course);

            return Mapper.Map<CourseLearningActivityResponse>(learningActivity);
        }
    }
}