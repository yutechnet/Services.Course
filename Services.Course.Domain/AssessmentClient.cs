using System;
using System.Configuration;
using System.Net.Http;
using BpeProducts.Common.Authorization;
using BpeProducts.Common.Contract;
using BpeProducts.Common.Exceptions;
using Services.Assessment.Contract;

namespace BpeProducts.Services.Course.Domain
{
    public interface IAssessmentClient
    {
        RubricInfoResponse GetRubric(Guid id);
        AssessmentInfo GetAssessment(Guid id);
        void PublishAssessment(Guid id, string publishNote);
        FeedbackResponse GetFeedback(Guid rubricId, Guid criterionId, Guid feedbackId);
        ObservationInfoResponse GetObservation(Guid rubricId, Guid criterionId, Guid observationId);
        void CloneEntityOutcomes(SupportingEntityType entityType, Guid entityId, CloneEntityOutcomeRequest request);
    }

    public class AssessmentClient : IAssessmentClient
    {
        public Lazy<HttpClient> HttpClient { get; set; }

        public AssessmentClient(RequestContext requestContext)
        {
            HttpClient = new Lazy<HttpClient>(() =>
                {
                    var httpClient = HttpClientFactory.Create(
                        new RequestIdMessageHandler(requestContext.RequestId),
                        new ApiVersionMessageHandler(ApiVersions.Version1Aug292014Release),
                        new SignatureMessageHandler(requestContext.ApiKey.Value, requestContext.UserId.Value,
                                                    requestContext.SharedSecret));
                    httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["AssessmentServiceBaseUrl"]);
                    return httpClient;
                });
        }

        public RubricInfoResponse GetRubric(Guid id)
        {
            var uri = string.Format("rubric/{0}", id);
            var result = HttpClient.Value.GetAsync(uri).Result;

            CheckForErrors(result);
            return result.Content.ReadAsAsync<RubricInfoResponse>().Result;
        }

        public AssessmentInfo GetAssessment(Guid id)
        {
            var uri = string.Format("assessment/{0}", id);
            var result = HttpClient.Value.GetAsync(uri).Result;

            CheckForErrors(result);
            return result.Content.ReadAsAsync<AssessmentInfo>().Result;
        }

        public void PublishAssessment(Guid id, string publishNote)
        {
            var publishRequest = new PublishRequest {PublishNote = publishNote};
            var uri = string.Format("assessment/{0}/publish", id);
            var result = HttpClient.Value.PutAsJsonAsync(uri, publishRequest).Result;

            CheckForErrors(result);
        }

        public void CloneEntityOutcomes(SupportingEntityType entityType, Guid entityId, CloneEntityOutcomeRequest request)
        {
            var requestUri = string.Format("{0}/{1}/clone", entityType, entityId);
            var result = HttpClient.Value.PostAsJsonAsync(requestUri, request).Result;

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
                throw new ForbiddenException(content);
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
            var uri = string.Format("rubric/{0}/criterion/{1}/feedback/{2}", rubricId, criterionId, feedbackId);
            var result = HttpClient.Value.GetAsync(uri).Result;

            CheckForErrors(result);
            return result.Content.ReadAsAsync<FeedbackResponse>().Result;
        }

        public ObservationInfoResponse GetObservation(Guid rubricId, Guid criterionId, Guid observationId)
        {
            var uri = string.Format("rubric/{0}/criterion/{1}/observation/{2}", rubricId, criterionId, observationId);
            var result = HttpClient.Value.GetAsync(uri).Result;

            CheckForErrors(result);
            return result.Content.ReadAsAsync<ObservationInfoResponse>().Result;
        }
    }
}