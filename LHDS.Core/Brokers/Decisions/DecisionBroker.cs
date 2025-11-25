// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using LHDS.Core.Models.Brokers.Decisions;
using LHDS.Core.Models.Foundations.Decisions;
using RESTFulSense.Clients;

namespace LHDS.Core.Brokers.Decisions
{
    public class DecisionBroker : IDecisionBroker
    {
        private readonly DecisionConfiguration decisionConfiguration;
        private readonly TokenCredential credential;
        private IRESTFulApiFactoryClient? apiClient = null;
        private AccessToken? accessToken = null;

        public DecisionBroker(DecisionConfiguration decisionConfiguration, TokenCredential credential)
        {
            this.decisionConfiguration = decisionConfiguration;
            this.credential = credential;
            this.accessToken = null;
            this.apiClient = null;
        }

        public async ValueTask<List<Decision>> GetPatientDecisions()
        {
            string relativeUrl = this.decisionConfiguration.IDecidePatientDecisionsRelativeUrl;
            List<Decision> decisions = await GetAsync<List<Decision>>(relativeUrl);

            return decisions;
        }

        public async ValueTask RecordAdoption(List<Decision> decisionsAdopted)
        {
            string relativeUrl = this.decisionConfiguration.IDecideRecordAdoptionRelativeUrl;
            await PostAsync(relativeUrl, decisionsAdopted);
        }

        private async ValueTask<T> GetAsync<T>(string relativeUrl)
        {
            if (apiClient is null || IsTokenExpired())
            {
                await SetupApiClient();
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)await this.apiClient.GetContentStringAsync(relativeUrl);
            }
            else
            {
                return await this.apiClient.GetContentAsync<T>(relativeUrl);
            }
        }

        private async ValueTask PostAsync(string relativeUrl, object body)
        {
            if (apiClient is null || IsTokenExpired())
            {
                await SetupApiClient();
            }

            await this.apiClient.PostContentAsync(relativeUrl, body);
        }

        private bool IsTokenExpired()
        {
            if (this.accessToken is null)
            {
                return true;
            }

            return DateTimeOffset.UtcNow >= this.accessToken.Value.ExpiresOn.AddMinutes(-5);
        }

        private async ValueTask GetAccessTokenAsync()
        {
            var tokenRequestContext =
                new TokenRequestContext(
                new[] { this.decisionConfiguration.IDecideScope }
                );

            AccessToken token =
                await this.credential.GetTokenAsync(
                    tokenRequestContext,
                    default);

            this.accessToken = token;
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
                    parameter: this.accessToken?.Token ?? "");

            this.apiClient = new RESTFulApiFactoryClient(httpClient);
        }
    }
}
