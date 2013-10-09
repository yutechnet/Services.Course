using System;
using System.Net.Http;

namespace BpeProducts.Services.Course.Host.TempSectionContracts
{
    public interface ISectionClient
    {
        HttpResponseMessage CreateSection(CreateSectionRequest request);
    }

    public class SectionClient : ISectionClient
    {
        private readonly Uri _sectionServiceBaseAddress;

        public SectionClient(Uri sectionServiceBaseAddress)
        {
            _sectionServiceBaseAddress = sectionServiceBaseAddress;
        }

        public HttpResponseMessage CreateSection(CreateSectionRequest request)
        {
            var client = new HttpClient();
            var uri = new Uri(_sectionServiceBaseAddress, "/section");
            var response = client.PostAsJsonAsync(uri.ToString(), request);

            return response.Result;
        }
    }
}
