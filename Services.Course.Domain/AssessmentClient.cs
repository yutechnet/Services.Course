using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using BpeProducts.Common.Authorization;
using BpeProducts.Common.Exceptions;
using Services.Assessment.Contract;
using log4net;

namespace BpeProducts.Services.Course.Domain
{
    public interface IAssessmentClient
    {
        RubricInfoResponse GetRubric(Guid id);
        AssessmentInfo GetAssessment(Guid id);
        void PublishAssessment(Guid id, string publishNote);
        FeedbackResponse GetFeedback(Guid rubricId, Guid criterionId, Guid feedbackId);
        ObservationInfoResponse GetObservation(Guid rubricId, Guid criterionId, Guid observationId);
        void CloneEntityOutcomes(string entityType, Guid entityId, CloneEntityOutcomeRequest request);
    }

    public class AssessmentClient : HttpClientBase, IAssessmentClient
    {
        public Uri BaseAddress { get; private set; }

        public AssessmentClient(ISamlTokenExtractor tokenExtractor, ILog log) : base(tokenExtractor, log)
        {
            BaseAddress = new Uri(ConfigurationManager.AppSettings["AssessmentServiceBaseUrl"]);
        }

        public AssessmentClient(ISamlTokenExtractor tokenExtractor, Uri baseAddress, ILog log) : base(tokenExtractor, log)
        {
            BaseAddress = baseAddress;
        }

        public RubricInfoResponse GetRubric(Guid id)
        {
            var uri = string.Format("{0}/rubric/{1}", BaseAddress, id);
            var result = HttpClient.GetAsync(uri).Result;

            CheckForErrors(result);
            return result.Content.ReadAsAsync<RubricInfoResponse>().Result;
        }

        public AssessmentInfo GetAssessment(Guid id)
        {
            var uri = string.Format("{0}/assessment/{1}", BaseAddress, id);
            var result = HttpClient.GetAsync(uri).Result;

            CheckForErrors(result);
            return result.Content.ReadAsAsync<AssessmentInfo>().Result;
        }

        public void PublishAssessment(Guid id, string publishNote)
        {
            var publishRequest = new PublishRequest {PublishNote = publishNote};
            var uri = string.Format("{0}/assessment/{1}/publish", BaseAddress, id);
            var result = HttpClient.PutAsJsonAsync(uri, publishRequest).Result;

            CheckForErrors(result);
        }

        public void CloneEntityOutcomes(string entityType, Guid entityId, CloneEntityOutcomeRequest request)
        {
            var requestUri = string.Format("{0}{1}/{2}/clone", BaseAddress, entityType, entityId);
            var result = HttpClient.PostAsJsonAsync(requestUri, request).Result;

            CheckForErrors(result);
        }

        private void CheckForErrors(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            var message = string.Format("Unexpected StatusCode {0} resluted from {1} call to {2}", response.StatusCode,
                                        response.RequestMessage.Method, response.RequestMessage.RequestUri);

            if ((int) response.StatusCode <= 199)
            {
                throw new InternalServerErrorException(message);
            }
            if ((int) response.StatusCode <= 399)
            {
                throw new InternalServerErrorException(message);
            }
            if ((int) response.StatusCode == 400)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                throw new BadRequestException(content);
            }
            if ((int) response.StatusCode == 401)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                throw new AuthorizationException(content);
            }
            if ((int) response.StatusCode == 403)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                throw new ForbiddenException(content);
            }
            if ((int) response.StatusCode == 404)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                throw new NotFoundException(content);
            }
            if ((int) response.StatusCode <= 499)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                throw new BadRequestException(content);
            }

            throw new InternalServerErrorException(message);
        }

        public FeedbackResponse GetFeedback(Guid rubricId, Guid criterionId, Guid feedbackId)
        {
            var uri = string.Format("{0}/rubric/{1}/criterion/{2}/feedback/{3}", BaseAddress, rubricId, criterionId, feedbackId);
            var result = HttpClient.GetAsync(uri).Result;

            CheckForErrors(result);
            return result.Content.ReadAsAsync<FeedbackResponse>().Result;
        }

        public ObservationInfoResponse GetObservation(Guid rubricId, Guid criterionId, Guid observationId)
        {
            var uri = string.Format("{0}/rubric/{1}/criterion/{2}/observation/{3}", BaseAddress, rubricId, criterionId, observationId);
            var result = HttpClient.GetAsync(uri).Result;

            CheckForErrors(result);
            return result.Content.ReadAsAsync<ObservationInfoResponse>().Result;
        }
    }

	public class HttpClientBase
	{
		private readonly ILog _log;
		private readonly ISamlTokenExtractor _samlTokenExtractor;

		public HttpClientBase(ISamlTokenExtractor samlTokenExtractor, ILog log)
		{
			_log = log;
			_samlTokenExtractor = samlTokenExtractor;
		}

		public HttpClient HttpClient
		{
			get
			{
				var samlToken = _samlTokenExtractor.GetSamlToken();
				var xApiKey = _samlTokenExtractor.GetApiKey();

				_log.Debug("Extracted X-ApiKey from SAML Token: " + xApiKey);

				var client = new HttpClient();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SAML", samlToken);
				client.DefaultRequestHeaders.Add("X-ApiKey", xApiKey);
				return client;
			}
		}
	}
}