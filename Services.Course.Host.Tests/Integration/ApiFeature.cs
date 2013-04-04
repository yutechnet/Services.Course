﻿using System;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.ServiceModel.Security;
using BpeProducts.Common.WebApi.Test;
using TechTalk.SpecFlow;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityModel.WSTrust;

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

            var base64SamlBytes = System.Text.Encoding.ASCII.GetBytes(samlToken);
            var base64SamlToken = System.Convert.ToBase64String(base64SamlBytes);

            apiTestHost.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SAML", base64SamlToken);

            FeatureContext.Current.Add("ApiTestHost", apiTestHost);
        }

        [AfterFeature("Api")]
        public static void AfterFeature()
        {
            ApiTestHost.Dispose();
        }

        private static string GetSamlToken()
        {
            const string samlToken = "<Assertion ID=\"_403cf4a6-ca26-4232-9971-a0785f0ac436\" IssueInstant=\"2013-04-04T21:37:36.937Z\" Version=\"2.0\" xmlns=\"urn:oasis:names:tc:SAML:2.0:assertion\"><Issuer>http://identityserver.v2.thinktecture.com/trust/changethis</Issuer><Signature xmlns=\"http://www.w3.org/2000/09/xmldsig#\"><SignedInfo><CanonicalizationMethod Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\" /><SignatureMethod Algorithm=\"http://www.w3.org/2001/04/xmldsig-more#rsa-sha256\" /><Reference URI=\"#_403cf4a6-ca26-4232-9971-a0785f0ac436\"><Transforms><Transform Algorithm=\"http://www.w3.org/2000/09/xmldsig#enveloped-signature\" /><Transform Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\" /></Transforms><DigestMethod Algorithm=\"http://www.w3.org/2001/04/xmlenc#sha256\" /><DigestValue>iEyrsVevt8aTSSQ8OqqWhWQzuLLEcFnJDcHLPgT8+SE=</DigestValue></Reference></SignedInfo><SignatureValue>GgtyDcnaIs0iFAual4Ibkyv8uv2vChVQZtYFhljevgQmwOKBDRzIpAqxBWP4EGf2isy9hSSG+b9O2goqjv6Ur8aFmJWuZI5qhA4s4EwOi9N92AV/Oz6izVL4yRk4bmXIoHyalBjhf82xs+slwYAuC/tAYhq/UtrjczHrDlhtrIg=</SignatureValue><KeyInfo><X509Data><X509Certificate>MIIFFTCCBH6gAwIBAgIKQYEavAAAAAAACTANBgkqhkiG9w0BAQUFADBpMRUwEwYKCZImiZPyLGQBGRYFbG9jYWwxGzAZBgoJkiaJk/IsZAEZFgtCUEVTRVJWSUNFUzETMBEGCgmSJomT8ixkARkWA0FFUDEeMBwGA1UEAxMVQUVQLUJQU0FaLUFFUC1EQzAxLUNBMB4XDTEyMDkxMjIwNDM0MVoXDTE0MDkxMjIwNDM0MVowcjELMAkGA1UEBhMCVVMxCzAJBgNVBAgTAkNBMRIwEAYDVQQHEwlTYW4gRGllZ28xHjAcBgNVBAoTFUJyaWRnZXBvaW50IEVkdWNhdGlvbjEMMAoGA1UECxMDQlRTMRQwEgYDVQQDDAsqLnRodXplLmNvbTCBnzANBgkqhkiG9w0BAQEFAAOBjQAwgYkCgYEAkPv2H6VDil0W2SWx71wAW5ejgl3sKrmcxq7m/3VksuBrg2YTVtmtamweBOcfEa+BR9lJHqmZQGF9iDKITrGG13fyc2G/I8YSknHyHbHSfyZHFU47DKQG3yCLRqE+zhM3YwV6LFTnF2P7bCkfKXYv9DHBDcnvy4xxLr50k+QJCi0CAwEAAaOCArkwggK1MA4GA1UdDwEB/wQEAwIFoDATBgNVHSUEDDAKBggrBgEFBQcDATB4BgkqhkiG9w0BCQ8EazBpMA4GCCqGSIb3DQMCAgIAgDAOBggqhkiG9w0DBAICAIAwCwYJYIZIAWUDBAEqMAsGCWCGSAFlAwQBLTALBglghkgBZQMEAQIwCwYJYIZIAWUDBAEFMAcGBSsOAwIHMAoGCCqGSIb3DQMHMB0GA1UdDgQWBBQItTF+/7Mdq9DOU+qfQaBs+UDy5DAfBgNVHSMEGDAWgBQ+kmYIp63iCnKN5ynCHhHi/8RS+jCB4AYDVR0fBIHYMIHVMIHSoIHPoIHMhoHJbGRhcDovLy9DTj1BRVAtQlBTQVotQUVQLURDMDEtQ0EsQ049QlBTQVotQUVQLURDMDEsQ049Q0RQLENOPVB1YmxpYyUyMEtleSUyMFNlcnZpY2VzLENOPVNlcnZpY2VzLENOPUNvbmZpZ3VyYXRpb24sREM9QlBFU0VSVklDRVMsREM9bG9jYWw/Y2VydGlmaWNhdGVSZXZvY2F0aW9uTGlzdD9iYXNlP29iamVjdENsYXNzPWNSTERpc3RyaWJ1dGlvblBvaW50MIHNBggrBgEFBQcBAQSBwDCBvTCBugYIKwYBBQUHMAKGga1sZGFwOi8vL0NOPUFFUC1CUFNBWi1BRVAtREMwMS1DQSxDTj1BSUEsQ049UHVibGljJTIwS2V5JTIwU2VydmljZXMsQ049U2VydmljZXMsQ049Q29uZmlndXJhdGlvbixEQz1CUEVTRVJWSUNFUyxEQz1sb2NhbD9jQUNlcnRpZmljYXRlP2Jhc2U/b2JqZWN0Q2xhc3M9Y2VydGlmaWNhdGlvbkF1dGhvcml0eTAhBgkrBgEEAYI3FAIEFB4SAFcAZQBiAFMAZQByAHYAZQByMA0GCSqGSIb3DQEBBQUAA4GBAJg5skDrzvGP0VfPObLGmvnfezKRey2M1kS63HvgUZ+ZWuldu7/Hq9fJjYvwoX34dPzXJELpdSeUVj3Uz9PtwsvtytOqi/301l7WD1VthPcT7+s09a+kAFeH6N61bJXkd9hUqwGbie+bHEAnTsc84qpTZ/jJssIB0fczYsJ0lfmG</X509Certificate></X509Data></KeyInfo></Signature><Subject><SubjectConfirmation Method=\"urn:oasis:names:tc:SAML:2.0:cm:bearer\" /></Subject><Conditions NotBefore=\"2013-04-04T21:37:36.925Z\" NotOnOrAfter=\"2033-03-30T21:37:36.925Z\"><AudienceRestriction><Audience>urn:ExampleWeb</Audience></AudienceRestriction></Conditions><AttributeStatement><Attribute Name=\"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name\"><AttributeValue>admin</AttributeValue></Attribute><Attribute Name=\"http://identityserver.thinktecture.com/claims/profileclaims/tenants\"><AttributeValue>ashford</AttributeValue></Attribute><Attribute Name=\"http://identityserver.thinktecture.com/claims/profileclaims/firstname\"><AttributeValue>Super</AttributeValue></Attribute><Attribute Name=\"http://identityserver.thinktecture.com/claims/profileclaims/lastname\"><AttributeValue>Admin</AttributeValue></Attribute><Attribute Name=\"http://identityserver.thinktecture.com/claims/profileclaims/tenant\"><AttributeValue>1</AttributeValue></Attribute></AttributeStatement><AuthnStatement AuthnInstant=\"2013-04-04T21:37:36.906Z\"><AuthnContext><AuthnContextClassRef>urn:oasis:names:tc:SAML:2.0:ac:classes:Password</AuthnContextClassRef></AuthnContext></AuthnStatement></Assertion>";

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