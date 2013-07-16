using System;
using System.Configuration;
using System.IO;
using System.Net.Http.Headers;
using BpeProducts.Common.WebApiTest;
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

            AddSamlToken(apiTestHost);

            //var base64SamlBytes = System.Text.Encoding.ASCII.GetBytes(samlToken);
            //var base64SamlToken = System.Convert.ToBase64String(base64SamlBytes);

            FeatureContext.Current.Add("ApiTestHost", apiTestHost);

			var targetUri = new Uri(ConfigurationManager.AppSettings["TestHostBaseAddress"]);
			if (!targetUri.Host.Equals("localhost"))
			{
				FeatureContext.Current.Add("CourseLeadingPath", targetUri.PathAndQuery + "/course");
				FeatureContext.Current.Add("ProgramLeadingPath", targetUri.PathAndQuery + "/program");
                FeatureContext.Current.Add("OutcomeLeadingPath", targetUri.PathAndQuery + "/outcome");
			}
			else
			{
				FeatureContext.Current.Add("CourseLeadingPath", "/course");
				FeatureContext.Current.Add("ProgramLeadingPath", "/program");
                FeatureContext.Current.Add("OutcomeLeadingPath", "/outcome");
            }
		
		}

        [AfterFeature("Api")]
        public static void AfterFeature()
        {
            ApiTestHost.Dispose();
        }

        static void AddSamlToken(WebApiTestHost apiTestHost)
        {
            var samlToken = File.ReadAllText("SAML.xml");

            Console.Out.WriteLine("SAML: " + samlToken);

            samlToken = samlToken.Replace(Environment.NewLine, String.Empty);
            samlToken = samlToken.Replace("\r", String.Empty);
            samlToken = samlToken.Replace("\n", String.Empty);
            samlToken = samlToken.Replace("  ", String.Empty);

            apiTestHost.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SAML", samlToken);
        }
    }
}