using System;
using System.Net.Http;
using BpeProducts.Services.Course.Contract;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Resources
{
    public class CourseResource
    {
        public Guid Id { get; set; }
        public Uri ResourseUri { get; set; }
        public SaveCourseRequest SaveRequest { get; set; }
        public CourseInfoResponse Dto { get; set; }
        public HttpResponseMessage Response { get; set; }
    }
}