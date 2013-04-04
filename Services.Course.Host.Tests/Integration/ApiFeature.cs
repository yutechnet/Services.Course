using System;
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
        // This token expires in 2033
        // private const string samlToken = "<Assertion ID=\"_f94db144-a300-47a5-8073-6d1dc1003b85\" IssueInstant=\"2013-04-04T18:35:44.048Z\" Version=\"2.0\" xmlns=\"urn:oasis:names:tc:SAML:2.0:assertion\"><Issuer>http://identityserver.v2.thinktecture.com/trust/changethis</Issuer><Signature xmlns=\"http://www.w3.org/2000/09/xmldsig#\"><SignedInfo><CanonicalizationMethod Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\" /><SignatureMethod Algorithm=\"http://www.w3.org/2001/04/xmldsig-more#rsa-sha256\" /><Reference URI=\"#_f94db144-a300-47a5-8073-6d1dc1003b85\"><Transforms><Transform Algorithm=\"http://www.w3.org/2000/09/xmldsig#enveloped-signature\" /><Transform Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\" /></Transforms><DigestMethod Algorithm=\"http://www.w3.org/2001/04/xmlenc#sha256\" /><DigestValue>GXr7vMv45KM+9pvJWGLNYINHHip0ebxBiGaROdTNlzM=</DigestValue></Reference></SignedInfo><SignatureValue>CRnFGaPI3sapCENWrZ78poqXQQnGnUrYOZWfyjb3pfrt+3/D/nCi479+TR056lEiZJWPkWsF/N0JGgGTbPXQ7fxeSl763TPyIez35u0Bswwc6n+ptAEOd6NHY60RsvVhLnw2OIeN/7DBVRWdQ3DAemp4B5v150tZ73Puy4buHxg=</SignatureValue><KeyInfo><X509Data><X509Certificate>MIIFFTCCBH6gAwIBAgIKQYEavAAAAAAACTANBgkqhkiG9w0BAQUFADBpMRUwEwYKCZImiZPyLGQBGRYFbG9jYWwxGzAZBgoJkiaJk/IsZAEZFgtCUEVTRVJWSUNFUzETMBEGCgmSJomT8ixkARkWA0FFUDEeMBwGA1UEAxMVQUVQLUJQU0FaLUFFUC1EQzAxLUNBMB4XDTEyMDkxMjIwNDM0MVoXDTE0MDkxMjIwNDM0MVowcjELMAkGA1UEBhMCVVMxCzAJBgNVBAgTAkNBMRIwEAYDVQQHEwlTYW4gRGllZ28xHjAcBgNVBAoTFUJyaWRnZXBvaW50IEVkdWNhdGlvbjEMMAoGA1UECxMDQlRTMRQwEgYDVQQDDAsqLnRodXplLmNvbTCBnzANBgkqhkiG9w0BAQEFAAOBjQAwgYkCgYEAkPv2H6VDil0W2SWx71wAW5ejgl3sKrmcxq7m/3VksuBrg2YTVtmtamweBOcfEa+BR9lJHqmZQGF9iDKITrGG13fyc2G/I8YSknHyHbHSfyZHFU47DKQG3yCLRqE+zhM3YwV6LFTnF2P7bCkfKXYv9DHBDcnvy4xxLr50k+QJCi0CAwEAAaOCArkwggK1MA4GA1UdDwEB/wQEAwIFoDATBgNVHSUEDDAKBggrBgEFBQcDATB4BgkqhkiG9w0BCQ8EazBpMA4GCCqGSIb3DQMCAgIAgDAOBggqhkiG9w0DBAICAIAwCwYJYIZIAWUDBAEqMAsGCWCGSAFlAwQBLTALBglghkgBZQMEAQIwCwYJYIZIAWUDBAEFMAcGBSsOAwIHMAoGCCqGSIb3DQMHMB0GA1UdDgQWBBQItTF+/7Mdq9DOU+qfQaBs+UDy5DAfBgNVHSMEGDAWgBQ+kmYIp63iCnKN5ynCHhHi/8RS+jCB4AYDVR0fBIHYMIHVMIHSoIHPoIHMhoHJbGRhcDovLy9DTj1BRVAtQlBTQVotQUVQLURDMDEtQ0EsQ049QlBTQVotQUVQLURDMDEsQ049Q0RQLENOPVB1YmxpYyUyMEtleSUyMFNlcnZpY2VzLENOPVNlcnZpY2VzLENOPUNvbmZpZ3VyYXRpb24sREM9QlBFU0VSVklDRVMsREM9bG9jYWw/Y2VydGlmaWNhdGVSZXZvY2F0aW9uTGlzdD9iYXNlP29iamVjdENsYXNzPWNSTERpc3RyaWJ1dGlvblBvaW50MIHNBggrBgEFBQcBAQSBwDCBvTCBugYIKwYBBQUHMAKGga1sZGFwOi8vL0NOPUFFUC1CUFNBWi1BRVAtREMwMS1DQSxDTj1BSUEsQ049UHVibGljJTIwS2V5JTIwU2VydmljZXMsQ049U2VydmljZXMsQ049Q29uZmlndXJhdGlvbixEQz1CUEVTRVJWSUNFUyxEQz1sb2NhbD9jQUNlcnRpZmljYXRlP2Jhc2U/b2JqZWN0Q2xhc3M9Y2VydGlmaWNhdGlvbkF1dGhvcml0eTAhBgkrBgEEAYI3FAIEFB4SAFcAZQBiAFMAZQByAHYAZQByMA0GCSqGSIb3DQEBBQUAA4GBAJg5skDrzvGP0VfPObLGmvnfezKRey2M1kS63HvgUZ+ZWuldu7/Hq9fJjYvwoX34dPzXJELpdSeUVj3Uz9PtwsvtytOqi/301l7WD1VthPcT7+s09a+kAFeH6N61bJXkd9hUqwGbie+bHEAnTsc84qpTZ/jJssIB0fczYsJ0lfmG</X509Certificate></X509Data></KeyInfo></Signature><Subject><SubjectConfirmation Method=\"urn:oasis:names:tc:SAML:2.0:cm:bearer\" /></Subject><Conditions NotBefore=\"2013-04-04T18:35:44.037Z\" NotOnOrAfter=\"2033-03-30T18:35:44.037Z\"><AudienceRestriction><Audience>urn:ExampleWeb</Audience></AudienceRestriction></Conditions><AttributeStatement><Attribute Name=\"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name\"><AttributeValue>admin</AttributeValue></Attribute><Attribute Name=\"http://identityserver.thinktecture.com/claims/profileclaims/tenants\"><AttributeValue>ashford</AttributeValue></Attribute><Attribute Name=\"http://identityserver.thinktecture.com/claims/profileclaims/firstname\"><AttributeValue>Super</AttributeValue></Attribute><Attribute Name=\"http://identityserver.thinktecture.com/claims/profileclaims/lastname\"><AttributeValue>Admin</AttributeValue></Attribute></AttributeStatement><AuthnStatement AuthnInstant=\"2013-04-04T18:35:44.019Z\"><AuthnContext><AuthnContextClassRef>urn:oasis:names:tc:SAML:2.0:ac:classes:Password</AuthnContextClassRef></AuthnContext></AuthnStatement></Assertion>";

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
            /*
            var samlToken = @"<Assertion ID=""_83173c94-dc35-4a80-9462-941d67acfa87"" IssueInstant=""2013-04-04T18:05:56.699Z"" Version=""2.0"" xmlns=""urn:oasis:names:tc:SAML:2.0:assertion"">
  <Issuer>http://identityserver.v2.thinktecture.com/trust/changethis</Issuer>
  <Signature xmlns=""http://www.w3.org/2000/09/xmldsig#"">
    <SignedInfo>
      <CanonicalizationMethod Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" />
      <SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"" />
      <Reference URI=""#_83173c94-dc35-4a80-9462-941d67acfa87"">
        <Transforms>
          <Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature"" />
          <Transform Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" />
        </Transforms>
        <DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256"" />
        <DigestValue>8S3bjcW4Iwjzdr0R1iUY4M1bJRgnDoZg+zwXZip4Ils=</DigestValue>
      </Reference>
    </SignedInfo>
    <SignatureValue>gZloxkJ3vBa/v4tBPlueLVKy9m8HeT5s/svPp8YIerW6Bqagvwa/Px2alycv2WFiUwnIRza+Z0pRN6Q9MUXI1ObmJiMfPQW24S8w4W1s7W2dz7/4ZLND0G+s8O5ZiEgd8frEyX//pEiDByYtKTC8om+2bdGiBR8Qnm4LCcW9Om8=</SignatureValue>
    <KeyInfo>
      <X509Data>
        <X509Certificate>MIIFFTCCBH6gAwIBAgIKQYEavAAAAAAACTANBgkqhkiG9w0BAQUFADBpMRUwEwYKCZImiZPyLGQBGRYFbG9jYWwxGzAZBgoJkiaJk/IsZAEZFgtCUEVTRVJWSUNFUzETMBEGCgmSJomT8ixkARkWA0FFUDEeMBwGA1UEAxMVQUVQLUJQU0FaLUFFUC1EQzAxLUNBMB4XDTEyMDkxMjIwNDM0MVoXDTE0MDkxMjIwNDM0MVowcjELMAkGA1UEBhMCVVMxCzAJBgNVBAgTAkNBMRIwEAYDVQQHEwlTYW4gRGllZ28xHjAcBgNVBAoTFUJyaWRnZXBvaW50IEVkdWNhdGlvbjEMMAoGA1UECxMDQlRTMRQwEgYDVQQDDAsqLnRodXplLmNvbTCBnzANBgkqhkiG9w0BAQEFAAOBjQAwgYkCgYEAkPv2H6VDil0W2SWx71wAW5ejgl3sKrmcxq7m/3VksuBrg2YTVtmtamweBOcfEa+BR9lJHqmZQGF9iDKITrGG13fyc2G/I8YSknHyHbHSfyZHFU47DKQG3yCLRqE+zhM3YwV6LFTnF2P7bCkfKXYv9DHBDcnvy4xxLr50k+QJCi0CAwEAAaOCArkwggK1MA4GA1UdDwEB/wQEAwIFoDATBgNVHSUEDDAKBggrBgEFBQcDATB4BgkqhkiG9w0BCQ8EazBpMA4GCCqGSIb3DQMCAgIAgDAOBggqhkiG9w0DBAICAIAwCwYJYIZIAWUDBAEqMAsGCWCGSAFlAwQBLTALBglghkgBZQMEAQIwCwYJYIZIAWUDBAEFMAcGBSsOAwIHMAoGCCqGSIb3DQMHMB0GA1UdDgQWBBQItTF+/7Mdq9DOU+qfQaBs+UDy5DAfBgNVHSMEGDAWgBQ+kmYIp63iCnKN5ynCHhHi/8RS+jCB4AYDVR0fBIHYMIHVMIHSoIHPoIHMhoHJbGRhcDovLy9DTj1BRVAtQlBTQVotQUVQLURDMDEtQ0EsQ049QlBTQVotQUVQLURDMDEsQ049Q0RQLENOPVB1YmxpYyUyMEtleSUyMFNlcnZpY2VzLENOPVNlcnZpY2VzLENOPUNvbmZpZ3VyYXRpb24sREM9QlBFU0VSVklDRVMsREM9bG9jYWw/Y2VydGlmaWNhdGVSZXZvY2F0aW9uTGlzdD9iYXNlP29iamVjdENsYXNzPWNSTERpc3RyaWJ1dGlvblBvaW50MIHNBggrBgEFBQcBAQSBwDCBvTCBugYIKwYBBQUHMAKGga1sZGFwOi8vL0NOPUFFUC1CUFNBWi1BRVAtREMwMS1DQSxDTj1BSUEsQ049UHVibGljJTIwS2V5JTIwU2VydmljZXMsQ049U2VydmljZXMsQ049Q29uZmlndXJhdGlvbixEQz1CUEVTRVJWSUNFUyxEQz1sb2NhbD9jQUNlcnRpZmljYXRlP2Jhc2U/b2JqZWN0Q2xhc3M9Y2VydGlmaWNhdGlvbkF1dGhvcml0eTAhBgkrBgEEAYI3FAIEFB4SAFcAZQBiAFMAZQByAHYAZQByMA0GCSqGSIb3DQEBBQUAA4GBAJg5skDrzvGP0VfPObLGmvnfezKRey2M1kS63HvgUZ+ZWuldu7/Hq9fJjYvwoX34dPzXJELpdSeUVj3Uz9PtwsvtytOqi/301l7WD1VthPcT7+s09a+kAFeH6N61bJXkd9hUqwGbie+bHEAnTsc84qpTZ/jJssIB0fczYsJ0lfmG</X509Certificate>
      </X509Data>
    </KeyInfo>
  </Signature>
  <Subject>
    <SubjectConfirmation Method=""urn:oasis:names:tc:SAML:2.0:cm:bearer"" />
  </Subject>
  <Conditions NotBefore=""2013-04-04T18:05:56.688Z"" NotOnOrAfter=""2033-03-30T18:05:56.688Z"">
    <AudienceRestriction>
      <Audience>urn:ExampleWeb</Audience>
    </AudienceRestriction>
  </Conditions>
  <AttributeStatement>
    <Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"">
      <AttributeValue>admin</AttributeValue>
    </Attribute>
    <Attribute Name=""http://identityserver.thinktecture.com/claims/profileclaims/tenants"">
      <AttributeValue>ashford</AttributeValue>
    </Attribute>
    <Attribute Name=""http://identityserver.thinktecture.com/claims/profileclaims/firstname"">
      <AttributeValue>Super</AttributeValue>
    </Attribute>
    <Attribute Name=""http://identityserver.thinktecture.com/claims/profileclaims/lastname"">
      <AttributeValue>Admin</AttributeValue>
    </Attribute>
    <Attribute Name=""http://identityserver.thinktecture.com/claims/profileclaims/tenant"">
      <AttributeValue>1</AttributeValue>
    </Attribute>
  </AttributeStatement>
  <AuthnStatement AuthnInstant=""2013-04-04T18:05:56.574Z"">
    <AuthnContext>
      <AuthnContextClassRef>urn:oasis:names:tc:SAML:2.0:ac:classes:Password</AuthnContextClassRef>
    </AuthnContext>
  </AuthnStatement>
</Assertion>";

            samlToken = samlToken.Replace(Environment.NewLine, String.Empty);
            samlToken = samlToken.Replace("  ", String.Empty);

            return samlToken;
            */

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
        }
    }
}