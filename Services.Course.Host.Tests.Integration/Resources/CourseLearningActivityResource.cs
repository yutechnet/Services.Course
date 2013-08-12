using System;
using System.Net.Http;
using BpeProducts.Services.Course.Contract;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Resources
{
    public class CourseLearningActivityResource : IResource
    {
        public HttpResponseMessage Response { get; set; }
        public Guid Id { get; set; }
        public Uri ResourceUri { get; set; }
        public SaveCourseLearningActivityRequest SaveRequest { get; set; }
        public CourseLearningActivityResponse Dto { get; set; }
    }
}