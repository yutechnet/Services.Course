using System;
using System.Net.Http;
using System.Net.Http.Headers;
using BpeProducts.Common.Authorization;
using BpeProducts.Services.Section.Contracts;

namespace BpeProducts.Services.Course.Domain
{
    public interface ISectionClient
    {
        HttpResponseMessage CreateSection(CreateSectionRequest request);
        HttpResponseMessage CreateSection(Uri sectionServiceAddress, CreateSectionRequest request);
    }

    public class SectionClient : ISectionClient
    {
        private readonly ISamlTokenExtractor _tokenExtractor;
        public Uri BaseAddress { get; private set; }

        public SectionClient(ISamlTokenExtractor tokenExtractor)
        {
            _tokenExtractor = tokenExtractor;
        }

        public SectionClient(ISamlTokenExtractor tokenExtractor, Uri baseAddress)
        {
            _tokenExtractor = tokenExtractor;
            BaseAddress = baseAddress;
        }

        public HttpResponseMessage CreateSection(CreateSectionRequest request)
        {
            return CreateSection(BaseAddress, request);
        }

        public HttpResponseMessage CreateSection(Uri sectionServiceAddress, CreateSectionRequest request)
        {
            var samlToken = _tokenExtractor.GetSamlToken();
			var xApiKey = _tokenExtractor.GetApiKey();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SAML", samlToken);
			client.DefaultRequestHeaders.Add("X-ApiKey", xApiKey);
            var uri = new Uri(sectionServiceAddress.ToString().TrimEnd('/') + "/section");
            var response = client.PostAsJsonAsync(uri.ToString(), request);

            return response.Result;
        }
    }
}