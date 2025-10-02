// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Decisions;
using LHDS.Core.Models.Foundations.Decisions;
using Newtonsoft.Json;
using RESTFulSense.Clients;

namespace LHDS.Core.Brokers.Decisions
{
    public class DecisionBroker : IDecisionBroker
    {
        private readonly DecisionConfiguration decisionConfiguration;
        private DecisionAccessToken decisionAccessToken = null;
        private IRESTFulApiFactoryClient? apiClient = null;

        public DecisionBroker(DecisionConfiguration decisionConfiguration)
        {
            this.decisionConfiguration = decisionConfiguration;
            this.decisionAccessToken = null;
            this.apiClient = null;
        }

        public async ValueTask<List<Decision>> GetPatientDecisions(DateTimeOffset? lastPollDate)
        {
            throw new NotImplementedException();
        }

        public ValueTask RecordAdoption(List<Decision> decisionsAdopted)
        {
            throw new NotImplementedException();
        }

        private async ValueTask GetAccessTokenAsync()
        {
            string tenantId = this.decisionConfiguration.TenantId;
            string tokenEndpoint = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";

            using (HttpClient httpClient = new HttpClient())
            {
                var requestContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_id", decisionConfiguration.ClientId),
                    new KeyValuePair<string, string>("client_secret", decisionConfiguration.ClientSecret),
                    new KeyValuePair<string, string>("scope", decisionConfiguration.Scope)
                });

                HttpResponseMessage response = await httpClient
                    .PostAsync(
                        requestUri: tokenEndpoint,
                        content: requestContent);

                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException("Failed to obtain access token");
                }

                string responseContent = await response.Content.ReadAsStringAsync();
                DecisionAccessToken? token = JsonConvert.DeserializeObject<DecisionAccessToken>(responseContent);

                if (token is null)
                {
                    throw new InvalidOperationException("Failed to deserialize access token");
                }

                token.ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(token.ExpiresIn);
                this.decisionAccessToken = token;
            }
        }

        private async ValueTask SetupApiClient()
        {
            await GetAccessTokenAsync();

            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(uriString: $"{this.decisionConfiguration.IDecideBaseUrl}"),
                Timeout = TimeSpan.FromSeconds(this.decisionConfiguration.TimeoutInSeconds),

                MaxResponseContentBufferSize =
                    this.decisionConfiguration.MaxResponseContentBufferSizeInMegaBytes * 1024 * 1024
            };

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    scheme: "Bearer",
                    parameter: this.decisionAccessToken?.AccessToken ?? "");

            this.apiClient = new RESTFulApiFactoryClient(httpClient);
        }
    }
}
