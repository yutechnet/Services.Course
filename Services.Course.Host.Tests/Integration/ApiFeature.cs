using BpeProducts.Common.WebApi.Test;
using TechTalk.SpecFlow;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
    [Binding]
    public static class ApiFeature
    {
        public static WebApiTestHost ApiTestHost
        {
            get { return (WebApiTestHost)FeatureContext.Current["ApiTestHost"]; }
        }

        [BeforeFeature("Api")]
        public static void BeforeFeature()
        {
            var apiTestHost = new WebApiTestHost(WebApiApplication.ConfigureWebApi);
            FeatureContext.Current.Add("ApiTestHost", apiTestHost);
        }

        [AfterFeature("Api")]
        public static void AfterFeature()
        {
            ApiTestHost.Dispose();
        }
    }
}