using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
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
        private readonly ISamlTokenExtractor _tokenExtractor;
        public Uri BaseAddress { get; private set; }

        public SectionClient(ISamlTokenExtractor tokenExtractor)
        {
            _tokenExtractor = tokenExtractor;
            BaseAddress = new Uri(ConfigurationManager.AppSettings["SectionServiceBaseUrl"]);

        }

        public HttpResponseMessage CreateSection(CreateSectionRequest request)
        {
            var samlToken = _tokenExtractor.GetSamlToken();
			var xApiKey = _tokenExtractor.GetApiKey();
            var client = HttpClientFactory.Create(new ApiVersionMessageHandler(ApiVersions.Version1Aug292014Release));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SAML", samlToken);
			client.DefaultRequestHeaders.Add("X-ApiKey", xApiKey);
            var uri = new Uri(BaseAddress + "section");
            var response = client.PostAsJsonAsync(uri.ToString(), request);

            return response.Result;
        }
    }
}