using System;
using System.Configuration;
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

            var samlToken = GetSamlToken();

            //var base64SamlBytes = System.Text.Encoding.ASCII.GetBytes(samlToken);
            //var base64SamlToken = System.Convert.ToBase64String(base64SamlBytes);

            apiTestHost.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SAML", samlToken);

            FeatureContext.Current.Add("ApiTestHost", apiTestHost);

			var targetUri = new Uri(ConfigurationManager.AppSettings["TestHostBaseAddress"]);
			if (!targetUri.Host.Equals("localhost"))
			{
				FeatureContext.Current.Add("CourseLeadingPath", targetUri.PathAndQuery + "/courses");
				FeatureContext.Current.Add("ProgramLeadingPath", targetUri.PathAndQuery + "/programs");
                FeatureContext.Current.Add("OutcomeLeadingPath", targetUri.PathAndQuery + "/outcome");
			}
			else
			{
				FeatureContext.Current.Add("CourseLeadingPath", "/courses");
				FeatureContext.Current.Add("ProgramLeadingPath", "/programs");
                FeatureContext.Current.Add("OutcomeLeadingPath", "/outcome");
            }
		
		}

        [AfterFeature("Api")]
        public static void AfterFeature()
        {
            ApiTestHost.Dispose();
        }

        private static string GetSamlToken()
        {
            const string samlToken = @"<Assertion ID=""_6755b5e1-1534-4a38-91ac-02a76a259a98"" IssueInstant=""2013-05-29T23:13:17.461Z"" Version=""2.0"" xmlns=""urn:oasis:names:tc:SAML:2.0:assertion""><Issuer>https://devaccounts.thuze.com/issue/hrd</Issuer><Signature xmlns=""http://www.w3.org/2000/09/xmldsig#""><SignedInfo><CanonicalizationMethod Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" /><SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"" /><Reference URI=""#_6755b5e1-1534-4a38-91ac-02a76a259a98""><Transforms><Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature"" /><Transform Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" /></Transforms><DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256"" /><DigestValue>NoPnciB9lHiUOCdKFF7UxhfNPsNszoO5zvkMVUHkk3Y=</DigestValue></Reference></SignedInfo><SignatureValue>kYhGPsRZGdFxctmqEwbB2KOlYlPtIhlx8vAD8qTaYBauVK/ZZWcu2ObRqo7/u+eXJsSZOAcWD2sShwfemM8UuyVQ0sxM5+Bz6r+X1kX5L6FqxuSDhqwriDLBZAspa95SS17AhMF+q6M8OWEw4fXsqN1XcumcZZftOXUdTCBIG+g=</SignatureValue><KeyInfo><X509Data><X509Certificate>MIICKjCCAZegAwIBAgIQrBhl92t1QpFNAzNKmmYIzTAJBgUrDgMCHQUAMCgxJjAkBgNVBAMTHWRldi5pZGVudGl0eXNpZ25pbmcudGh1emUuY29tMB4XDTEzMDUxNjIyMjAwOFoXDTM5MTIzMTIzNTk1OVowKDEmMCQGA1UEAxMdZGV2LmlkZW50aXR5c2lnbmluZy50aHV6ZS5jb20wgZ8wDQYJKoZIhvcNAQEBBQADgY0AMIGJAoGBALByEte04RHdSj/uWMHQpsu7ARqKviF8a0kfrgWqcN6jIf7CbfIC/wzzcz9DnyJf79mSMfW6dQPpWQ/XkxPDUxCCY0oYB0aFbEe4g50wrZOpMIvNGIsyTgj8Pq5C1RwMPcQXkfJ0jGSwq4BJAtOGOxeQ0D/61B4jJInezV9Iw8NnAgMBAAGjXTBbMFkGA1UdAQRSMFCAEGTnIx7EfxcqeHPi/Oo6UDihKjAoMSYwJAYDVQQDEx1kZXYuaWRlbnRpdHlzaWduaW5nLnRodXplLmNvbYIQrBhl92t1QpFNAzNKmmYIzTAJBgUrDgMCHQUAA4GBAKbSI8yA4NPvLzg4s5dA1+tDPk613TXrXJGNPXXXyDdJRp1pvFRdi+foJTThV6BKFOTnjLh30wOHTJIrzbNBJVCcRFY9hPwtrLG5TA/ofHVHnBJNDS+ZJYs0Us8BR4AUJ1OIYDTFtzjSBJWbXJLft1tELU33CjBdAbyljMzU4I3M</X509Certificate></X509Data></KeyInfo></Signature><Subject><SubjectConfirmation Method=""urn:oasis:names:tc:SAML:2.0:cm:bearer"" /></Subject><Conditions NotBefore=""2013-05-29T23:13:17.459Z"" NotOnOrAfter=""2033-05-24T23:13:17.459Z""><AudienceRestriction><Audience>urn:ExampleWeb</Audience></AudienceRestriction></Conditions><AttributeStatement><Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"" a:OriginalIssuer=""Parabotype"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><AttributeValue>admin</AttributeValue></Attribute><Attribute Name=""urn:BPEP/UserGuid"" a:OriginalIssuer=""Parabotype"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><AttributeValue>a163b0dd-4f30-422f-854b-a1cd010b39d8</AttributeValue></Attribute><Attribute Name=""http://identityserver.thinktecture.com/claims/profileclaims/userguid"" a:OriginalIssuer=""Parabotype"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><AttributeValue>a163b0dd-4f30-422f-854b-a1cd010b39d8</AttributeValue></Attribute><Attribute Name=""urn:BPEP/TenantId"" a:OriginalIssuer=""Parabotype"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><AttributeValue>1</AttributeValue></Attribute><Attribute Name=""urn:BPEP/FirstName"" a:OriginalIssuer=""Parabotype"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><AttributeValue>admin</AttributeValue></Attribute><Attribute Name=""urn:BPEP/LastName"" a:OriginalIssuer=""Parabotype"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><AttributeValue>admin</AttributeValue></Attribute><Attribute Name=""urn:BPEP/UserName"" a:OriginalIssuer=""Parabotype"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><AttributeValue>admin</AttributeValue></Attribute><Attribute Name=""http://identityserver.thinktecture.com/claims/profileclaims/tenant"" a:OriginalIssuer=""Parabotype"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><AttributeValue>1</AttributeValue></Attribute><Attribute Name=""http://identityserver.thinktecture.com/claims/identityprovider""><AttributeValue>Parabotype</AttributeValue></Attribute></AttributeStatement></Assertion>";

            return samlToken;
            /*

            const string WsTrustAddress = "https://devaccounts.thuze.com/issue/wstrust/mixed/username";
            const string UserId = "admin";
            const string Password = "Password1!";
            const string Realm = "urn:ExampleWeb";

            var wsTrustChannelFactory = new WSTrustChannelFactory(
                new UserNameWSTrustBinding(SecurityMode.TransportWithMessageCredential),
                WsTrustAddress)
            {
                TrustVersion = TrustVersion.WSTrust13
            };

            if (wsTrustChannelFactory.Credentials == null)
            {
                throw new Exception("Really was hoping WSTrustChannelFactory.Credentials was not null");
            }

            wsTrustChannelFactory.Credentials.UserName.UserName = UserId;
            wsTrustChannelFactory.Credentials.UserName.Password = Password;

            var wsTrustChannelContract = wsTrustChannelFactory.CreateChannel();

            var requestSecurityToken = new RequestSecurityToken
            {
                RequestType = RequestTypes.Issue,
                KeyType = KeyTypes.Bearer,
                TokenType = TokenTypes.Saml2TokenProfile11,
                AppliesTo = new EndpointReference(Realm)
            };

            RequestSecurityTokenResponse requestSecurityTokenResponse;
            var securityToken = wsTrustChannelContract.Issue(requestSecurityToken, out requestSecurityTokenResponse) as GenericXmlSecurityToken;
            if (securityToken == null)
            {
                throw new Exception("Really was hoping WsTrustChannelFactory.Issue gave me a token of type GenericXmlSecurityToken");
            }

            return securityToken.TokenXml.OuterXml;
            */
        }
    }
}