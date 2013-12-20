using BpeProducts.Common.WebApiTest.Framework;
using BpeProducts.Services.Course.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources.Account;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Operations
{
    public static class GetOperations
    {
        public static Dictionary<Guid, List<OutcomeInfo>> GetEntityLearningOutcomes(IList<Guid> entityIds)
        {
            var queryString = entityIds.Aggregate("?entityIds=", (current, entityId) => current + (entityId + ",")).TrimEnd(',');
            var requestUri = ApiFeature.LeadingPath + "/outcome/entityoutcomes" + queryString;
            return ApiFeature.CourseTestHost.Get<Dictionary<Guid, List<OutcomeInfo>>>(requestUri);
        }

        public static CourseInfoResponse GetCourse(IResource resource)
        {
            return ApiFeature.CourseTestHost.Get<CourseInfoResponse>(resource.ResourceUri.ToString());
        }

        public static ProgramResponse GetProgram(IResource resource)
        {
            return ApiFeature.CourseTestHost.Get<ProgramResponse>(resource.ResourceUri.ToString());
        }

        public static CourseLearningActivityResponse GetCourseLearningActivity(IResource resource)
        {
            return ApiFeature.CourseTestHost.Get<CourseLearningActivityResponse>(resource.ResourceUri.ToString());            
        }

        public static CourseSegmentInfo GetSegment(IResource resource)
        {
            return ApiFeature.CourseTestHost.Get<CourseSegmentInfo>(resource.ResourceUri.ToString());
        }

        public static OutcomeInfo GetLearningOutcome(IResource resource)
        {
            return ApiFeature.CourseTestHost.Get<OutcomeInfo>(resource.ResourceUri.ToString());
        }

        public static List<OutcomeInfo> GetSupportedOutcomes(IResource resource)
        {
            var uri = string.Format("{0}/supports", resource.ResourceUri);
            return ApiFeature.CourseTestHost.Get<List<OutcomeInfo>>(uri);
        }

        public static List<ProgramResponse> GetAllPrograms()
        {
            var uri = string.Format("{0}/program", ApiFeature.LeadingPath);
            return ApiFeature.CourseTestHost.Get<List<ProgramResponse>>(uri);
        }

        public static IEnumerable<CourseInfoResponse> SearchCourses(string queryString)
        {
            var uri = string.Format("{0}/course{1}", ApiFeature.LeadingPath, queryString);
            return ApiFeature.CourseTestHost.Get<IEnumerable<CourseInfoResponse>>(uri);
        }

        public static IEnumerable<CourseInfoResponse> GetPublishedCourses(OrganizationResource orgResource)
        {
            var uri = string.Format("{0}/course/published?organizationId={1}", ApiFeature.LeadingPath, orgResource.Id);
            return ApiFeature.CourseTestHost.Get<IEnumerable<CourseInfoResponse>>(uri);
        }

        public static LearningMaterialInfo GetCourseLearningMaterial(IResource resource)
        {
            return ApiFeature.CourseTestHost.Get<LearningMaterialInfo>(resource.ResourceUri.ToString());
        }
    }
}
