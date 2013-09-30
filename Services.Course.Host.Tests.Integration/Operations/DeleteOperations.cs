using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Common.WebApiTest.Framework;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Operations
{
    public static class DeleteOperations
    {
        public static HttpResponseMessage DeleteResource(IResource resource)
        {
            var response = ApiFeature.CourseTestHost.Client.DeleteAsync(resource.ResourceUri).Result;
            Responses.Add(response);
            
            return response;
        }
    }
}
