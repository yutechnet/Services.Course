using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BpeProducts.Services.Course.Host.Tests.Integration.Resources;
using BpeProducts.Services.Course.Host.Tests.Integration.StepSetups;

namespace BpeProducts.Services.Course.Host.Tests.Integration.Operations
{
    public static class DeleteOperations
    {
        public static HttpResponseMessage DeleteResource(IResource resource)
        {
            var response = ApiFeature.ApiTestHost.Client.DeleteAsync(resource.ResourceUri).Result;
            Whens.ResponseMessages.Add(response);
            
            return response;
        }
    }
}
