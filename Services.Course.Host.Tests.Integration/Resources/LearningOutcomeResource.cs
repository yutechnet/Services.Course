using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Contract;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Resources
{
    public class LearningOutcomeResource
    {
        public HttpResponseMessage Response { get; set; }
        public Guid Id { get; set; }
        public Uri ResourceUri { get; set; }
        public OutcomeRequest SaveRequest { get; set; }
    }
}
