using System;
using System.Configuration;
using System.Net.Http;
using BpeProducts.Common.Authorization;
using BpeProducts.Common.Contract;
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
        public Lazy<HttpClient> HttpClient { get; set; }

        public AssetServiceClient(RequestContext requestContext)
        {
            HttpClient = new Lazy<HttpClient>(() =>
                {
                    var httpClient = HttpClientFactory.Create(
                        new RequestIdMessageHandler(requestContext.RequestId),
                        new ApiVersionMessageHandler(ApiVersions.Version1Aug292014Release),
                        new SignatureMessageHandler(requestContext.ApiKey.Value, requestContext.UserId.Value,
                                                    requestContext.SharedSecret));
                    httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["AssetServiceBaseUrl"]);
                    return httpClient;
                });
        }

        public LibraryInfo AddAssetToLibrary(string ownerType, Guid ownerId, Guid assetId)
        {
            var uri = string.Format("library/{0}/{1}/asset/{2}", ownerType, ownerId, assetId);
            var result = HttpClient.Value.PutAsJsonAsync(uri, new { }).Result;

            CheckForErrors(result);

            return result.Content.ReadAsAsync<LibraryInfo>().Result;
        }

        public AssetInfo GetAsset(Guid assetId)
        {
            var uri = string.Format("asset/{0}", assetId);
            var result = HttpClient.Value.GetAsync(uri).Result;

            CheckForErrors(result);

            return result.Content.ReadAsAsync<AssetInfo>().Result;
        }

        public void PublishAsset(Guid assetId, string note)
        {
            var uri = string.Format("asset/{0}/publish", assetId);
            var result = HttpClient.Value.PutAsJsonAsync(uri, new { }).Result;

            CheckForErrors(result);
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
            if ((int)response.StatusCode <= 399)
            {
                throw new InternalServerErrorException(message);
            }
            if ((int)response.StatusCode <= 499)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                throw new BadRequestException(content);
            }

            throw new InternalServerErrorException(message);
        }
    }
}