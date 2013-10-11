using System;
using System.Net.Http;

namespace BpeProducts.Services.Course.Host.TempSectionContracts
{
    public interface ISectionClient
    {
        HttpResponseMessage CreateSection(CreateSectionRequest request);
        HttpResponseMessage CreateSection(Uri sectionServiceAddress, CreateSectionRequest request);
    }

    public class SectionClient : ISectionClient
    {
        public Uri BaseAddress { get; private set; }

        public SectionClient(Uri baseAddress)
        {
            BaseAddress = baseAddress;
        }

        public HttpResponseMessage CreateSection(CreateSectionRequest request)
        {
            return CreateSection(BaseAddress, request);
        }

        public HttpResponseMessage CreateSection(Uri sectionServiceAddress, CreateSectionRequest request)
        {
            var client = new HttpClient();
            var uri = new Uri(sectionServiceAddress, "/section");
            var response = client.PostAsJsonAsync(uri.ToString(), request);

            return response.Result;
        }
    }
}
