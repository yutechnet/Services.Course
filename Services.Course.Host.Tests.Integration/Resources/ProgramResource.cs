using System;
using System.Net.Http;
using BpeProducts.Services.Course.Contract;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Resources
{
    public class ProgramResource : IResource
    {
        public HttpResponseMessage Response { get; set; }
        public Guid Id { get; set; }
        public Uri ResourceUri { get; set; }
        public SaveProgramRequest SaveRequest { get; set; }
        public ProgramResponse Dto { get; set; }
    }
}