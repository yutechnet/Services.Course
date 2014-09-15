using System;
using System.Configuration;
using System.Net.Http;
using BpeProducts.Common.Authorization;
using BpeProducts.Common.Contract;
using BpeProducts.Services.Section.Contracts;

namespace BpeProducts.Services.Course.Domain
{
    public interface ISectionClient
    {
        HttpResponseMessage CreateSection(CreateSectionRequest request);
    }

    public class SectionClient : ISectionClient
    {
        public Lazy<HttpClient> HttpClient { get; set; }

        public SectionClient(RequestContext requestContext)
        {
            HttpClient = new Lazy<HttpClient>(() =>
                {
                    var httpClient = HttpClientFactory.Create(
                        new RequestIdMessageHandler(requestContext.RequestId),
                        new ApiVersionMessageHandler(ApiVersions.Version1Aug292014Release),
                        new SignatureMessageHandler(requestContext.ApiKey.Value, requestContext.UserId.Value,
                                                    requestContext.SharedSecret));
                    httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["SectionServiceBaseUrl"]);
                    return httpClient;
                });
        }

        public HttpResponseMessage CreateSection(CreateSectionRequest request)
        {
            var response = HttpClient.Value.PostAsJsonAsync("section", request);
            return response.Result;
        }
    }
}