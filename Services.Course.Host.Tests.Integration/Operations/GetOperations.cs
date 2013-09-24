using BpeProducts.Services.Course.Contract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Operations
{
    public static class GetOperations
    {
        public static Dictionary<Guid, List<OutcomeInfo>> GetEntityLearningOutcomes(IList<Guid> entityIds)
        {
            var queryString = entityIds.Aggregate("?entityIds=", (current, entityId) => current + (entityId + ",")).TrimEnd(',');
            var requestUri = ApiFeature.LeadingPath + "/outcome/entityoutcomes" + queryString;
            return ApiFeature.ApiTestHost.Get<Dictionary<Guid, List<OutcomeInfo>>>(requestUri);
        }

        public static CourseInfoResponse GetCourse(Uri requestUri)
        {
            return ApiFeature.ApiTestHost.Get<CourseInfoResponse>(requestUri.ToString());
        }

        public static ProgramResponse GetProgram(Uri requestUri)
        {
            return ApiFeature.ApiTestHost.Get<ProgramResponse>(requestUri.ToString());
        }

        public static CourseLearningActivityResponse GetCourseLearningActivity(Uri resourseUri)
        {
            return ApiFeature.ApiTestHost.Get<CourseLearningActivityResponse>(resourseUri.ToString());            
        }

        public static CourseSegmentInfo GetSegment(Uri resourseUri)
        {
            return ApiFeature.ApiTestHost.Get<CourseSegmentInfo>(resourseUri.ToString());
        }

        public static OutcomeInfo GetLearningOutcome(Uri requestUri)
        {
            return ApiFeature.ApiTestHost.Get<OutcomeInfo>(requestUri.ToString());
        }

        public static List<OutcomeInfo> GetSupportedOutcomes(Uri requestUri)
        {
            var uri = string.Format("{0}/supports", requestUri);
            return ApiFeature.ApiTestHost.Get<List<OutcomeInfo>>(uri);
        }

        public static List<ProgramResponse> GetAllPrograms()
        {
            var uri = string.Format("{0}/program", ApiFeature.LeadingPath);
            return ApiFeature.ApiTestHost.Get<List<ProgramResponse>>(uri);
        }

        public static List<Guid> GetCourseTemplateIds(Guid organizationId)
        {
            //TODO: Replace with we implement the service.
            return new List<Guid> {Guid.NewGuid()};
        }
    }
}
