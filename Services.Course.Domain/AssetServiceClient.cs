using System;
using System.Configuration;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using BpeProducts.Common.Authorization;
using BpeProducts.Common.Exceptions;
using BpeProducts.Services.Asset.Contracts;

namespace BpeProducts.Services.Course.Domain
{
    public interface IAssetServiceClient
    {
        LibraryInfo AddAssetToLibrary(string ownerType, Guid ownerId, Guid assetId);
        AssetInfo GetAsset(Guid assetId);
        void PublishAsset(Guid assetId, string note);
    }

    public class AssetServiceClient : IAssetServiceClient
    {
        private readonly ISamlTokenExtractor _tokenExtractor;
        public Uri BaseAddress { get; private set; }

        public AssetServiceClient(ISamlTokenExtractor tokenExtractor)
        {
            _tokenExtractor = tokenExtractor;
            BaseAddress = new Uri(ConfigurationManager.AppSettings["AssetServiceBaseUrl"]);
        }

        public AssetServiceClient(ISamlTokenExtractor tokenExtractor, Uri baseAddress)
        {
            _tokenExtractor = tokenExtractor;
            BaseAddress = baseAddress;
        }

        public LibraryInfo AddAssetToLibrary(string ownerType, Guid ownerId, Guid assetId)
        {
            var client = GetHttpClient();

            var uri = string.Format("{0}/library/{1}/{2}/asset/{3}", BaseAddress, ownerType, ownerId, assetId);
            var result = client.PutAsJsonAsync(uri, new { }).Result;
            
            CheckForErrors(result);

            return result.Content.ReadAsAsync<LibraryInfo>().Result;
        }

        public AssetInfo GetAsset(Guid assetId)
        {
            var client = GetHttpClient();

            var uri = string.Format("{0}/asset/{1}", BaseAddress, assetId);
            var result = client.GetAsync(uri).Result;

            CheckForErrors(result);

            return result.Content.ReadAsAsync<AssetInfo>().Result;
        }

        public void PublishAsset(Guid assetId, string note)
        {
            var client = GetHttpClient();

            var uri = string.Format("{0}/asset/{1}/publish", BaseAddress, assetId);
            var result = client.PutAsJsonAsync(uri, new { }).Result;

            CheckForErrors(result);
        }

        private HttpClient GetHttpClient()
        {
            var samlToken = _tokenExtractor.GetSamlToken();
	        var xApiKey = _tokenExtractor.GetApiKey();


            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SAML", samlToken);
			client.DefaultRequestHeaders.Add("X-ApiKey", xApiKey);

            return client;
        }

        private void CheckForErrors(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            var message = string.Format("Unexpected StatusCode {0} resluted from {1} call to {2}", response.StatusCode,
                                        response.RequestMessage.Method, response.RequestMessage.RequestUri);

            if ((int)response.StatusCode <= 199)
            {
                throw new InternalServerErrorException(message);
            }
            if ((int) response.StatusCode <= 399)
            {
                throw new InternalServerErrorException(message);
            }
            if ((int) response.StatusCode <= 499)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                throw new BadRequestException(content);
            }
            
            throw new InternalServerErrorException(message);
        }
    }
}