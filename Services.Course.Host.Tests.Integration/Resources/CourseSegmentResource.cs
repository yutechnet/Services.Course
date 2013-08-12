using System;
using System.Net.Http;
using BpeProducts.Services.Course.Contract;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Resources
{
    public class CourseSegmentResource : IResource
    {
        public HttpResponseMessage Response { get; set; }
        public Guid Id { get; set; }
        public Uri ResourseUri { get; set; }
        public SaveCourseSegmentRequest SaveRequest { get; set; }
    }
}