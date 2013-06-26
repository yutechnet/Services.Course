using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
			//const string samlToken = @"<Assertion ID=""_6755b5e1-1534-4a38-91ac-02a76a259a98"" IssueInstant=""2013-05-29T23:13:17.461Z"" NewVersion=""2.0"" xmlns=""urn:oasis:names:tc:SAML:2.0:assertion""><Issuer>https://devaccounts.thuze.com/issue/hrd</Issuer><Signature xmlns=""http://www.w3.org/2000/09/xmldsig#""><SignedInfo><CanonicalizationMethod Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" /><SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"" /><Reference URI=""#_6755b5e1-1534-4a38-91ac-02a76a259a98""><Transforms><Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature"" /><Transform Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" /></Transforms><DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256"" /><DigestValue>NoPnciB9lHiUOCdKFF7UxhfNPsNszoO5zvkMVUHkk3Y=</DigestValue></Reference></SignedInfo><SignatureValue>kYhGPsRZGdFxctmqEwbB2KOlYlPtIhlx8vAD8qTaYBauVK/ZZWcu2ObRqo7/u+eXJsSZOAcWD2sShwfemM8UuyVQ0sxM5+Bz6r+X1kX5L6FqxuSDhqwriDLBZAspa95SS17AhMF+q6M8OWEw4fXsqN1XcumcZZftOXUdTCBIG+g=</SignatureValue><KeyInfo><X509Data><X509Certificate>MIICKjCCAZegAwIBAgIQrBhl92t1QpFNAzNKmmYIzTAJBgUrDgMCHQUAMCgxJjAkBgNVBAMTHWRldi5pZGVudGl0eXNpZ25pbmcudGh1emUuY29tMB4XDTEzMDUxNjIyMjAwOFoXDTM5MTIzMTIzNTk1OVowKDEmMCQGA1UEAxMdZGV2LmlkZW50aXR5c2lnbmluZy50aHV6ZS5jb20wgZ8wDQYJKoZIhvcNAQEBBQADgY0AMIGJAoGBALByEte04RHdSj/uWMHQpsu7ARqKviF8a0kfrgWqcN6jIf7CbfIC/wzzcz9DnyJf79mSMfW6dQPpWQ/XkxPDUxCCY0oYB0aFbEe4g50wrZOpMIvNGIsyTgj8Pq5C1RwMPcQXkfJ0jGSwq4BJAtOGOxeQ0D/61B4jJInezV9Iw8NnAgMBAAGjXTBbMFkGA1UdAQRSMFCAEGTnIx7EfxcqeHPi/Oo6UDihKjAoMSYwJAYDVQQDEx1kZXYuaWRlbnRpdHlzaWduaW5nLnRodXplLmNvbYIQrBhl92t1QpFNAzNKmmYIzTAJBgUrDgMCHQUAA4GBAKbSI8yA4NPvLzg4s5dA1+tDPk613TXrXJGNPXXXyDdJRp1pvFRdi+foJTThV6BKFOTnjLh30wOHTJIrzbNBJVCcRFY9hPwtrLG5TA/ofHVHnBJNDS+ZJYs0Us8BR4AUJ1OIYDTFtzjSBJWbXJLft1tELU33CjBdAbyljMzU4I3M</X509Certificate></X509Data></KeyInfo></Signature><Subject><SubjectConfirmation Method=""urn:oasis:names:tc:SAML:2.0:cm:bearer"" /></Subject><Conditions NotBefore=""2013-05-29T23:13:17.459Z"" NotOnOrAfter=""2033-05-24T23:13:17.459Z""><AudienceRestriction><Audience>urn:ExampleWeb</Audience></AudienceRestriction></Conditions><AttributeStatement><Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"" a:OriginalIssuer=""Parabotype"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><AttributeValue>admin</AttributeValue></Attribute><Attribute Name=""urn:BPEP/UserGuid"" a:OriginalIssuer=""Parabotype"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><AttributeValue>a163b0dd-4f30-422f-854b-a1cd010b39d8</AttributeValue></Attribute><Attribute Name=""http://identityserver.thinktecture.com/claims/profileclaims/userguid"" a:OriginalIssuer=""Parabotype"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><AttributeValue>a163b0dd-4f30-422f-854b-a1cd010b39d8</AttributeValue></Attribute><Attribute Name=""urn:BPEP/TenantId"" a:OriginalIssuer=""Parabotype"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><AttributeValue>1</AttributeValue></Attribute><Attribute Name=""urn:BPEP/FirstName"" a:OriginalIssuer=""Parabotype"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><AttributeValue>admin</AttributeValue></Attribute><Attribute Name=""urn:BPEP/LastName"" a:OriginalIssuer=""Parabotype"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><AttributeValue>admin</AttributeValue></Attribute><Attribute Name=""urn:BPEP/UserName"" a:OriginalIssuer=""Parabotype"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><AttributeValue>admin</AttributeValue></Attribute><Attribute Name=""http://identityserver.thinktecture.com/claims/profileclaims/tenant"" a:OriginalIssuer=""Parabotype"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><AttributeValue>1</AttributeValue></Attribute><Attribute Name=""http://identityserver.thinktecture.com/claims/identityprovider""><AttributeValue>Parabotype</AttributeValue></Attribute></AttributeStatement></Assertion>";
			
			//var base64SamlBytes = System.Text.Encoding.ASCII.GetBytes(samlToken);
			//var base64SamlToken = System.Convert.ToBase64String(base64SamlBytes);

            var samlToken = File.ReadAllText("SAML.xml");

            samlToken = samlToken.Replace(Environment.NewLine, String.Empty);
            samlToken = samlToken.Replace("\r", String.Empty);
            samlToken = samlToken.Replace("\n", String.Empty);
            samlToken = samlToken.Replace("  ", String.Empty);

            ApiFeature.ApiTestHost.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SAML", samlToken);  

        }

		[Given(@"That I am guest")]
		public void GivenThatIAmGuest()
		{
			const string samlToken = @"<Assertion ID=""_afb668d1-c648-4a41-abbd-9dd13a31e1ed"" IssueInstant=""2013-05-30T00:31:10.484Z"" NewVersion=""2.0"" xmlns=""urn:oasis:names:tc:SAML:2.0:assertion""><Issuer>https://devaccounts.thuze.com/issue/hrd</Issuer><Signature xmlns=""http://www.w3.org/2000/09/xmldsig#""><SignedInfo><CanonicalizationMethod Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" /><SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#rsa-sha256"" /><Reference URI=""#_afb668d1-c648-4a41-abbd-9dd13a31e1ed""><Transforms><Transform Algorithm=""http://www.w3.org/2000/09/xmldsig#enveloped-signature"" /><Transform Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#"" /></Transforms><DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256"" /><DigestValue>HLjmVh6Z6kmJQ6eZ1b82jhhOxZ8IbT2J8ZEd6HogNCQ=</DigestValue></Reference></SignedInfo><SignatureValue>K8RoPxsdxNekZkSdiHbc9ruAqG5lMnGsJjvVkhJcGMQcmP+0pDTZQQD2E8ndL0iKZSENIiQPcXHoAIcJSnLBNmChkXlhX3Bcqp733uEJCL9g32hoD22urQd0WqLIFOLw4jNGgZ+InTGzfOI61zP37eKBGSMjQPcyBcK84kJFEog=</SignatureValue><KeyInfo><X509Data><X509Certificate>MIICKjCCAZegAwIBAgIQrBhl92t1QpFNAzNKmmYIzTAJBgUrDgMCHQUAMCgxJjAkBgNVBAMTHWRldi5pZGVudGl0eXNpZ25pbmcudGh1emUuY29tMB4XDTEzMDUxNjIyMjAwOFoXDTM5MTIzMTIzNTk1OVowKDEmMCQGA1UEAxMdZGV2LmlkZW50aXR5c2lnbmluZy50aHV6ZS5jb20wgZ8wDQYJKoZIhvcNAQEBBQADgY0AMIGJAoGBALByEte04RHdSj/uWMHQpsu7ARqKviF8a0kfrgWqcN6jIf7CbfIC/wzzcz9DnyJf79mSMfW6dQPpWQ/XkxPDUxCCY0oYB0aFbEe4g50wrZOpMIvNGIsyTgj8Pq5C1RwMPcQXkfJ0jGSwq4BJAtOGOxeQ0D/61B4jJInezV9Iw8NnAgMBAAGjXTBbMFkGA1UdAQRSMFCAEGTnIx7EfxcqeHPi/Oo6UDihKjAoMSYwJAYDVQQDEx1kZXYuaWRlbnRpdHlzaWduaW5nLnRodXplLmNvbYIQrBhl92t1QpFNAzNKmmYIzTAJBgUrDgMCHQUAA4GBAKbSI8yA4NPvLzg4s5dA1+tDPk613TXrXJGNPXXXyDdJRp1pvFRdi+foJTThV6BKFOTnjLh30wOHTJIrzbNBJVCcRFY9hPwtrLG5TA/ofHVHnBJNDS+ZJYs0Us8BR4AUJ1OIYDTFtzjSBJWbXJLft1tELU33CjBdAbyljMzU4I3M</X509Certificate></X509Data></KeyInfo></Signature><Subject><SubjectConfirmation Method=""urn:oasis:names:tc:SAML:2.0:cm:bearer"" /></Subject><Conditions NotBefore=""2013-05-30T00:31:10.468Z"" NotOnOrAfter=""2033-05-25T00:31:10.468Z""><AudienceRestriction><Audience>urn:ExampleWeb</Audience></AudienceRestriction></Conditions><AttributeStatement><Attribute Name=""http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"" a:OriginalIssuer=""Parabotype"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><AttributeValue>guest</AttributeValue></Attribute><Attribute Name=""urn:BPEP/UserGuid"" a:OriginalIssuer=""Parabotype"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><AttributeValue>2bb31b70-bc5b-4c99-8300-a1cd00f04ba7</AttributeValue></Attribute><Attribute Name=""http://identityserver.thinktecture.com/claims/profileclaims/userguid"" a:OriginalIssuer=""Parabotype"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><AttributeValue>2bb31b70-bc5b-4c99-8300-a1cd00f04ba7</AttributeValue></Attribute><Attribute Name=""urn:BPEP/TenantId"" a:OriginalIssuer=""Parabotype"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><AttributeValue>1</AttributeValue></Attribute><Attribute Name=""urn:BPEP/FirstName"" a:OriginalIssuer=""Parabotype"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><AttributeValue>guest</AttributeValue></Attribute><Attribute Name=""urn:BPEP/LastName"" a:OriginalIssuer=""Parabotype"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><AttributeValue>guest</AttributeValue></Attribute><Attribute Name=""urn:BPEP/UserName"" a:OriginalIssuer=""Parabotype"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><AttributeValue>guest</AttributeValue></Attribute><Attribute Name=""http://identityserver.thinktecture.com/claims/profileclaims/tenant"" a:OriginalIssuer=""Parabotype"" xmlns:a=""http://schemas.xmlsoap.org/ws/2009/09/identity/claims""><AttributeValue>1</AttributeValue></Attribute><Attribute Name=""http://identityserver.thinktecture.com/claims/identityprovider""><AttributeValue>Parabotype</AttributeValue></Attribute></AttributeStatement></Assertion>";

            ApiFeature.ApiTestHost.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SAML", samlToken);           
		}

		[When(@"I submit an authorized creation request")]
		public void WhenISubmitAnAuthorizedCreationRequest()
		{
			var saveCourseRequest = new SaveCourseRequest()
				{
					Name = "Test",
					Code = "Test",
					Description = "Test",
					TenantId = 999999					              
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
