using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using BpeProducts.Services.Course.Contract;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace BpeProducts.Services.Course.Host.Tests.Integration
{
	[Binding]
	public class CourseAuthorizationSteps
	{
		private string _leadingPath;

		private AuthenticationHeaderValue _originalToken;

		// For additional details on SpecFlow step definitions see http://go.specflow.org/doc-stepdef
		public CourseAuthorizationSteps()
        {
            var targetUri = new Uri(ConfigurationManager.AppSettings["TestHostBaseAddress"]);
            if (!targetUri.Host.Equals("localhost"))
            {
                _leadingPath = targetUri.PathAndQuery + "/courses";
            }
            else
            {
                _leadingPath = "/courses";
            }
        }

		[BeforeScenario]
		public void BeforeScenario()
		{
			_originalToken = ApiFeature.ApiTestHost.Client.DefaultRequestHeaders.Authorization;
		}

		[AfterScenario]
		public void AfterScenario()
		{
			ApiFeature.ApiTestHost.Client.DefaultRequestHeaders.Authorization = _originalToken;
		}

		[Given(@"That I am admin")]
        public void GivenThatIAmAdmin()
        {
			const string samlToken = "<Assertion ID=\"_293ca6a8-54b4-4ecc-9c47-e3416d9efd3b\" IssueInstant=\"2013-04-12T18:04:42.447Z\" Version=\"2.0\" xmlns=\"urn:oasis:names:tc:SAML:2.0:assertion\"><Issuer>http://identityserver.v2.thinktecture.com/trust/changethis</Issuer><Signature xmlns=\"http://www.w3.org/2000/09/xmldsig#\"><SignedInfo><CanonicalizationMethod Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\" /><SignatureMethod Algorithm=\"http://www.w3.org/2001/04/xmldsig-more#rsa-sha256\" /><Reference URI=\"#_293ca6a8-54b4-4ecc-9c47-e3416d9efd3b\"><Transforms><Transform Algorithm=\"http://www.w3.org/2000/09/xmldsig#enveloped-signature\" /><Transform Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\" /></Transforms><DigestMethod Algorithm=\"http://www.w3.org/2001/04/xmlenc#sha256\" /><DigestValue>xE+3e4rKn/dxPq7XdKBW44+2VJPZ3Nje/b6JoLmKtSQ=</DigestValue></Reference></SignedInfo><SignatureValue>CA9oRkYJStO6UHidQNryTtAFG/u9r3AZUOZtG4AS7U0KCcHqZNgfH/aPZUeZPlJUBKT4ztVjNRJPjxPkIvM1ccL1dj54CXfi1itoepuhGhfxo62OPj8SnR4Xd7sbUwF24uM8YpdJck2WEcV1K1ds1u+iBnOMOE8/yW7KqQYuJsk=</SignatureValue><KeyInfo><X509Data><X509Certificate>MIIFFTCCBH6gAwIBAgIKQYEavAAAAAAACTANBgkqhkiG9w0BAQUFADBpMRUwEwYKCZImiZPyLGQBGRYFbG9jYWwxGzAZBgoJkiaJk/IsZAEZFgtCUEVTRVJWSUNFUzETMBEGCgmSJomT8ixkARkWA0FFUDEeMBwGA1UEAxMVQUVQLUJQU0FaLUFFUC1EQzAxLUNBMB4XDTEyMDkxMjIwNDM0MVoXDTE0MDkxMjIwNDM0MVowcjELMAkGA1UEBhMCVVMxCzAJBgNVBAgTAkNBMRIwEAYDVQQHEwlTYW4gRGllZ28xHjAcBgNVBAoTFUJyaWRnZXBvaW50IEVkdWNhdGlvbjEMMAoGA1UECxMDQlRTMRQwEgYDVQQDDAsqLnRodXplLmNvbTCBnzANBgkqhkiG9w0BAQEFAAOBjQAwgYkCgYEAkPv2H6VDil0W2SWx71wAW5ejgl3sKrmcxq7m/3VksuBrg2YTVtmtamweBOcfEa+BR9lJHqmZQGF9iDKITrGG13fyc2G/I8YSknHyHbHSfyZHFU47DKQG3yCLRqE+zhM3YwV6LFTnF2P7bCkfKXYv9DHBDcnvy4xxLr50k+QJCi0CAwEAAaOCArkwggK1MA4GA1UdDwEB/wQEAwIFoDATBgNVHSUEDDAKBggrBgEFBQcDATB4BgkqhkiG9w0BCQ8EazBpMA4GCCqGSIb3DQMCAgIAgDAOBggqhkiG9w0DBAICAIAwCwYJYIZIAWUDBAEqMAsGCWCGSAFlAwQBLTALBglghkgBZQMEAQIwCwYJYIZIAWUDBAEFMAcGBSsOAwIHMAoGCCqGSIb3DQMHMB0GA1UdDgQWBBQItTF+/7Mdq9DOU+qfQaBs+UDy5DAfBgNVHSMEGDAWgBQ+kmYIp63iCnKN5ynCHhHi/8RS+jCB4AYDVR0fBIHYMIHVMIHSoIHPoIHMhoHJbGRhcDovLy9DTj1BRVAtQlBTQVotQUVQLURDMDEtQ0EsQ049QlBTQVotQUVQLURDMDEsQ049Q0RQLENOPVB1YmxpYyUyMEtleSUyMFNlcnZpY2VzLENOPVNlcnZpY2VzLENOPUNvbmZpZ3VyYXRpb24sREM9QlBFU0VSVklDRVMsREM9bG9jYWw/Y2VydGlmaWNhdGVSZXZvY2F0aW9uTGlzdD9iYXNlP29iamVjdENsYXNzPWNSTERpc3RyaWJ1dGlvblBvaW50MIHNBggrBgEFBQcBAQSBwDCBvTCBugYIKwYBBQUHMAKGga1sZGFwOi8vL0NOPUFFUC1CUFNBWi1BRVAtREMwMS1DQSxDTj1BSUEsQ049UHVibGljJTIwS2V5JTIwU2VydmljZXMsQ049U2VydmljZXMsQ049Q29uZmlndXJhdGlvbixEQz1CUEVTRVJWSUNFUyxEQz1sb2NhbD9jQUNlcnRpZmljYXRlP2Jhc2U/b2JqZWN0Q2xhc3M9Y2VydGlmaWNhdGlvbkF1dGhvcml0eTAhBgkrBgEEAYI3FAIEFB4SAFcAZQBiAFMAZQByAHYAZQByMA0GCSqGSIb3DQEBBQUAA4GBAJg5skDrzvGP0VfPObLGmvnfezKRey2M1kS63HvgUZ+ZWuldu7/Hq9fJjYvwoX34dPzXJELpdSeUVj3Uz9PtwsvtytOqi/301l7WD1VthPcT7+s09a+kAFeH6N61bJXkd9hUqwGbie+bHEAnTsc84qpTZ/jJssIB0fczYsJ0lfmG</X509Certificate></X509Data></KeyInfo></Signature><Subject><SubjectConfirmation Method=\"urn:oasis:names:tc:SAML:2.0:cm:bearer\" /></Subject><Conditions NotBefore=\"2013-04-12T18:04:42.367Z\" NotOnOrAfter=\"2033-04-07T18:04:42.367Z\"><AudienceRestriction><Audience>urn:ExampleWeb</Audience></AudienceRestriction></Conditions><AttributeStatement><Attribute Name=\"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name\"><AttributeValue>admin</AttributeValue></Attribute><Attribute Name=\"http://identityserver.thinktecture.com/claims/profileclaims/tenants\"><AttributeValue>ashford</AttributeValue></Attribute><Attribute Name=\"http://identityserver.thinktecture.com/claims/profileclaims/firstname\"><AttributeValue>Super</AttributeValue></Attribute><Attribute Name=\"http://identityserver.thinktecture.com/claims/profileclaims/lastname\"><AttributeValue>Admin</AttributeValue></Attribute><Attribute Name=\"http://identityserver.thinktecture.com/claims/profileclaims/userguid\"><AttributeValue>cbac8764-b642-45e4-925e-2b155d1915e2</AttributeValue></Attribute><Attribute Name=\"http://identityserver.thinktecture.com/claims/profileclaims/tenant\"><AttributeValue>1</AttributeValue></Attribute></AttributeStatement><AuthnStatement AuthnInstant=\"2013-04-12T18:04:42.216Z\"><AuthnContext><AuthnContextClassRef>urn:oasis:names:tc:SAML:2.0:ac:classes:Password</AuthnContextClassRef></AuthnContext></AuthnStatement></Assertion>";
			
			var base64SamlBytes = System.Text.Encoding.ASCII.GetBytes(samlToken);
			var base64SamlToken = System.Convert.ToBase64String(base64SamlBytes);

			ApiFeature.ApiTestHost.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SAML", base64SamlToken);           
        }

		[Given(@"That I am guest")]
		public void GivenThatIAmGuest()
		{
			var base64SamlToken =
				"PEFzc2VydGlvbiBJRD0iX2FmZGVjYzJiLWQzZWQtNDIxMi1iMGVjLWRjZDE3ODdiYzdiNiIgSXNzdWVJbnN0YW50PSIyMDEzLTA0LTI1VDIzOjAwOjMyLjYxN1oiIFZlcnNpb249IjIuMCIgeG1sbnM9InVybjpvYXNpczpuYW1lczp0YzpTQU1MOjIuMDphc3NlcnRpb24iPjxJc3N1ZXI+aHR0cDovL2lkZW50aXR5c2VydmVyLnYyLnRoaW5rdGVjdHVyZS5jb20vdHJ1c3QvY2hhbmdldGhpczwvSXNzdWVyPjxTaWduYXR1cmUgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyMiPjxTaWduZWRJbmZvPjxDYW5vbmljYWxpemF0aW9uTWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS8xMC94bWwtZXhjLWMxNG4jIiAvPjxTaWduYXR1cmVNZXRob2QgQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNyc2Etc2hhMjU2IiAvPjxSZWZlcmVuY2UgVVJJPSIjX2FmZGVjYzJiLWQzZWQtNDIxMi1iMGVjLWRjZDE3ODdiYzdiNiI+PFRyYW5zZm9ybXM+PFRyYW5zZm9ybSBBbGdvcml0aG09Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyNlbnZlbG9wZWQtc2lnbmF0dXJlIiAvPjxUcmFuc2Zvcm0gQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzEwL3htbC1leGMtYzE0biMiIC8+PC9UcmFuc2Zvcm1zPjxEaWdlc3RNZXRob2QgQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGVuYyNzaGEyNTYiIC8+PERpZ2VzdFZhbHVlPnFYKzRPNE0wcnBmWGVEQlQybXRWZ2ZtcktFNy9mMTBXd0xaQi84SmkyUE09PC9EaWdlc3RWYWx1ZT48L1JlZmVyZW5jZT48L1NpZ25lZEluZm8+PFNpZ25hdHVyZVZhbHVlPkhLZW5Jc1FBUmNNdjdUMWM3ZFV3VFp3WGQ4OCtlV054L1I3T0NIT2dRZXBhUEtzWW5WZUtRSjhURlIzSzF5YXArNTJaaXN4aElhRTJaaDlRZ1FNd1BWdll2MGtyYVpWOHFGUzFMSVo1dzBGWWVCWWQvOW14N01GLzM2Qk4yZjBZcnQ5OW5sRzJUUFBIZmZPZE4wVTc1djQ3NEJ2aTJXY3N4T0h0TldtVmE3UT08L1NpZ25hdHVyZVZhbHVlPjxLZXlJbmZvPjxYNTA5RGF0YT48WDUwOUNlcnRpZmljYXRlPk1JSUZGVENDQkg2Z0F3SUJBZ0lLUVlFYXZBQUFBQUFBQ1RBTkJna3Foa2lHOXcwQkFRVUZBREJwTVJVd0V3WUtDWkltaVpQeUxHUUJHUllGYkc5allXd3hHekFaQmdvSmtpYUprL0lzWkFFWkZndENVRVZUUlZKV1NVTkZVekVUTUJFR0NnbVNKb21UOGl4a0FSa1dBMEZGVURFZU1Cd0dBMVVFQXhNVlFVVlFMVUpRVTBGYUxVRkZVQzFFUXpBeExVTkJNQjRYRFRFeU1Ea3hNakl3TkRNME1Wb1hEVEUwTURreE1qSXdORE0wTVZvd2NqRUxNQWtHQTFVRUJoTUNWVk14Q3pBSkJnTlZCQWdUQWtOQk1SSXdFQVlEVlFRSEV3bFRZVzRnUkdsbFoyOHhIakFjQmdOVkJBb1RGVUp5YVdSblpYQnZhVzUwSUVWa2RXTmhkR2x2YmpFTU1Bb0dBMVVFQ3hNRFFsUlRNUlF3RWdZRFZRUUREQXNxTG5Sb2RYcGxMbU52YlRDQm56QU5CZ2txaGtpRzl3MEJBUUVGQUFPQmpRQXdnWWtDZ1lFQWtQdjJINlZEaWwwVzJTV3g3MXdBVzVlamdsM3NLcm1jeHE3bS8zVmtzdUJyZzJZVFZ0bXRhbXdlQk9jZkVhK0JSOWxKSHFtWlFHRjlpREtJVHJHRzEzZnljMkcvSThZU2tuSHlIYkhTZnlaSEZVNDdES1FHM3lDTFJxRSt6aE0zWXdWNkxGVG5GMlA3YkNrZktYWXY5REhCRGNudnk0eHhMcjUwaytRSkNpMENBd0VBQWFPQ0Fya3dnZ0sxTUE0R0ExVWREd0VCL3dRRUF3SUZvREFUQmdOVkhTVUVEREFLQmdnckJnRUZCUWNEQVRCNEJna3Foa2lHOXcwQkNROEVhekJwTUE0R0NDcUdTSWIzRFFNQ0FnSUFnREFPQmdncWhraUc5dzBEQkFJQ0FJQXdDd1lKWUlaSUFXVURCQUVxTUFzR0NXQ0dTQUZsQXdRQkxUQUxCZ2xnaGtnQlpRTUVBUUl3Q3dZSllJWklBV1VEQkFFRk1BY0dCU3NPQXdJSE1Bb0dDQ3FHU0liM0RRTUhNQjBHQTFVZERnUVdCQlFJdFRGKy83TWRxOURPVStxZlFhQnMrVUR5NURBZkJnTlZIU01FR0RBV2dCUStrbVlJcDYzaUNuS041eW5DSGhIaS84UlMrakNCNEFZRFZSMGZCSUhZTUlIVk1JSFNvSUhQb0lITWhvSEpiR1JoY0Rvdkx5OURUajFCUlZBdFFsQlRRVm90UVVWUUxVUkRNREV0UTBFc1EwNDlRbEJUUVZvdFFVVlFMVVJETURFc1EwNDlRMFJRTEVOT1BWQjFZbXhwWXlVeU1FdGxlU1V5TUZObGNuWnBZMlZ6TEVOT1BWTmxjblpwWTJWekxFTk9QVU52Ym1acFozVnlZWFJwYjI0c1JFTTlRbEJGVTBWU1ZrbERSVk1zUkVNOWJHOWpZV3cvWTJWeWRHbG1hV05oZEdWU1pYWnZZMkYwYVc5dVRHbHpkRDlpWVhObFAyOWlhbVZqZEVOc1lYTnpQV05TVEVScGMzUnlhV0oxZEdsdmJsQnZhVzUwTUlITkJnZ3JCZ0VGQlFjQkFRU0J3RENCdlRDQnVnWUlLd1lCQlFVSE1BS0dnYTFzWkdGd09pOHZMME5PUFVGRlVDMUNVRk5CV2kxQlJWQXRSRU13TVMxRFFTeERUajFCU1VFc1EwNDlVSFZpYkdsakpUSXdTMlY1SlRJd1UyVnlkbWxqWlhNc1EwNDlVMlZ5ZG1salpYTXNRMDQ5UTI5dVptbG5kWEpoZEdsdmJpeEVRejFDVUVWVFJWSldTVU5GVXl4RVF6MXNiMk5oYkQ5alFVTmxjblJwWm1sallYUmxQMkpoYzJVL2IySnFaV04wUTJ4aGMzTTlZMlZ5ZEdsbWFXTmhkR2x2YmtGMWRHaHZjbWwwZVRBaEJna3JCZ0VFQVlJM0ZBSUVGQjRTQUZjQVpRQmlBRk1BWlFCeUFIWUFaUUJ5TUEwR0NTcUdTSWIzRFFFQkJRVUFBNEdCQUpnNXNrRHJ6dkdQMFZmUE9iTEdtdm5mZXpLUmV5Mk0xa1M2M0h2Z1VaK1pXdWxkdTcvSHE5ZkpqWXZ3b1gzNGRQelhKRUxwZFNlVVZqM1V6OVB0d3N2dHl0T3FpLzMwMWw3V0QxVnRoUGNUNytzMDlhK2tBRmVINk42MWJKWGtkOWhVcXdHYmllK2JIRUFuVHNjODRxcFRaL2pKc3NJQjBmY3pZc0owbGZtRzwvWDUwOUNlcnRpZmljYXRlPjwvWDUwOURhdGE+PC9LZXlJbmZvPjwvU2lnbmF0dXJlPjxTdWJqZWN0PjxTdWJqZWN0Q29uZmlybWF0aW9uIE1ldGhvZD0idXJuOm9hc2lzOm5hbWVzOnRjOlNBTUw6Mi4wOmNtOmJlYXJlciIgLz48L1N1YmplY3Q+PENvbmRpdGlvbnMgTm90QmVmb3JlPSIyMDEzLTA0LTI1VDIzOjAwOjMyLjYwMloiIE5vdE9uT3JBZnRlcj0iMjAzMy0wNC0yMFQyMzowMDozMi42MDJaIj48QXVkaWVuY2VSZXN0cmljdGlvbj48QXVkaWVuY2U+dXJuOkV4YW1wbGVXZWI8L0F1ZGllbmNlPjwvQXVkaWVuY2VSZXN0cmljdGlvbj48L0NvbmRpdGlvbnM+PEF0dHJpYnV0ZVN0YXRlbWVudD48QXR0cmlidXRlIE5hbWU9Imh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiPjxBdHRyaWJ1dGVWYWx1ZT5ndWVzdDwvQXR0cmlidXRlVmFsdWU+PC9BdHRyaWJ1dGU+PEF0dHJpYnV0ZSBOYW1lPSJodHRwOi8vaWRlbnRpdHlzZXJ2ZXIudGhpbmt0ZWN0dXJlLmNvbS9jbGFpbXMvcHJvZmlsZWNsYWltcy9maXJzdG5hbWUiPjxBdHRyaWJ1dGVWYWx1ZT5HdWVzdDwvQXR0cmlidXRlVmFsdWU+PC9BdHRyaWJ1dGU+PEF0dHJpYnV0ZSBOYW1lPSJodHRwOi8vaWRlbnRpdHlzZXJ2ZXIudGhpbmt0ZWN0dXJlLmNvbS9jbGFpbXMvcHJvZmlsZWNsYWltcy9sYXN0bmFtZSI+PEF0dHJpYnV0ZVZhbHVlPlVzZXI8L0F0dHJpYnV0ZVZhbHVlPjwvQXR0cmlidXRlPjxBdHRyaWJ1dGUgTmFtZT0iaHR0cDovL2lkZW50aXR5c2VydmVyLnRoaW5rdGVjdHVyZS5jb20vY2xhaW1zL3Byb2ZpbGVjbGFpbXMvdXNlcmd1aWQiPjxBdHRyaWJ1dGVWYWx1ZT43MTg5NWM5My1hOWExLTQ1YWQtOWJmMi1jNzM2YzIzNjViNzQ8L0F0dHJpYnV0ZVZhbHVlPjwvQXR0cmlidXRlPjxBdHRyaWJ1dGUgTmFtZT0iaHR0cDovL2lkZW50aXR5c2VydmVyLnRoaW5rdGVjdHVyZS5jb20vY2xhaW1zL3Byb2ZpbGVjbGFpbXMvdGVuYW50Ij48QXR0cmlidXRlVmFsdWU+MTwvQXR0cmlidXRlVmFsdWU+PC9BdHRyaWJ1dGU+PC9BdHRyaWJ1dGVTdGF0ZW1lbnQ+PEF1dGhuU3RhdGVtZW50IEF1dGhuSW5zdGFudD0iMjAxMy0wNC0yNVQyMzowMDozMi4zODRaIj48QXV0aG5Db250ZXh0PjxBdXRobkNvbnRleHRDbGFzc1JlZj51cm46b2FzaXM6bmFtZXM6dGM6U0FNTDoyLjA6YWM6Y2xhc3NlczpQYXNzd29yZDwvQXV0aG5Db250ZXh0Q2xhc3NSZWY+PC9BdXRobkNvbnRleHQ+PC9BdXRoblN0YXRlbWVudD48L0Fzc2VydGlvbj4=";
			
			ApiFeature.ApiTestHost.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SAML", base64SamlToken);           
		}

		[When(@"I submit an authorized creation request")]
		public void WhenISubmitAnAuthorizedCreationRequest()
		{
			var saveCourseRequest = new SaveCourseRequest()
				{
					Name = "Test",
					Code = "Test",
					Description = "Test",
					TenantId = 1					              
				};
			var response = ApiFeature.ApiTestHost.Client.PostAsync(_leadingPath, saveCourseRequest, new JsonMediaTypeFormatter()).Result;

			if (ScenarioContext.Current.ContainsKey("createCourseResponse"))
			{
				ScenarioContext.Current.Remove("createCourseResponse");
			}
			ScenarioContext.Current.Add("createCourseResponse", response);

			// this is the response to ensure the success code
			if (ScenarioContext.Current.ContainsKey("responseToValidate"))
			{
				ScenarioContext.Current.Remove("responseToValidate");
			}
			ScenarioContext.Current.Add("responseToValidate", response);
		}

		[Then(@"I should get a success response")]
		public void ThenIShouldGetASuccessResponse()
		{
			var response = ScenarioContext.Current.Get<HttpResponseMessage>("responseToValidate");
			response.EnsureSuccessStatusCode();
		}

		[Then(@"I should get a failure response")]
		public void ThenIShouldGetAFailureResponse()
		{
			var response = ScenarioContext.Current.Get<HttpResponseMessage>("responseToValidate");
			Assert.AreEqual(response.StatusCode,HttpStatusCode.Unauthorized);
		}
	}
}
