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
using Newtonsoft.Json;
using RESTFulSense.Clients;

namespace LHDS.Core.Brokers.Decisions
{
    public class DecisionBroker : IDecisionBroker
    {
        private readonly DecisionConfiguration decisionConfiguration;
        private string accessToken = null;
        private IRESTFulApiFactoryClient? apiClient = null;

        public DecisionBroker(DecisionConfiguration decisionConfiguration)
        {
            this.decisionConfiguration = decisionConfiguration;
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
            if (apiClient is null)
            {
                await SetupApiClient();
            }

            if (apiClient is null)
            {
                throw new InvalidOperationException("Failed to setup API client");
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)await this.apiClient.GetContentStringAsync(relativeUrl);
            }

            return await this.apiClient.GetContentAsync<T>(relativeUrl);
        }

        private async ValueTask PostAsync(string relativeUrl, object body)
        {
            if (apiClient is null)
            {
                await SetupApiClient();
            }

            if (apiClient is null)
            {
                throw new InvalidOperationException("Failed to setup API client");
            }

            await this.apiClient.PostContentAsync(relativeUrl, body);
        }

        private async ValueTask GetAccessTokenAsync()
        {
            var credential = new DefaultAzureCredential();
            var tokenRequestContext = new TokenRequestContext(
                new[] { "api://a72f1411-698b-4efc-914b-46279c2d5aae/manage" }
            );

            AccessToken accessToken = await credential.GetTokenAsync(
                tokenRequestContext,
                default
            );

            this.accessToken = accessToken.Token;
        }

        private async ValueTask SetupApiClient()
        {
            await GetAccessTokenAsync();

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri($"{this.decisionConfiguration.IDecideBaseUrl}"),
                Timeout = TimeSpan.FromSeconds(this.decisionConfiguration.TimeoutInSeconds),
                MaxResponseContentBufferSize =
                    this.decisionConfiguration.MaxResponseContentBufferSizeInMegaBytes * 1024 * 1024
            };

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    this.accessToken ?? "");

            this.apiClient = new RESTFulApiFactoryClient(httpClient);
        }
    }
}
