// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Decisions;
using LHDS.Core.Models.Foundations.Decisions;
using RESTFulSense.Clients;

namespace LHDS.Core.Brokers.Decisions
{
    public class DecisionBroker : IDecisionBroker
    {
        private readonly DecisionConfiguration decisionConfiguration;
        private IRESTFulApiFactoryClient? apiClient = null;

        public DecisionBroker(DecisionConfiguration decisionConfiguration)
        {
            this.decisionConfiguration = decisionConfiguration;
            SetupApiClient();
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

        private async ValueTask<T> GetAsync<T>(string relativeUrl) =>
            await this.apiClient.GetContentAsync<T>(relativeUrl);

        private async ValueTask PostAsync(string relativeUrl, object body) =>
            await this.apiClient.PostContentAsync(relativeUrl, body);

        private void SetupApiClient()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(uriString: $"{this.decisionConfiguration.IDecideBaseUrl}"),
                Timeout = TimeSpan.FromSeconds(this.decisionConfiguration.TimeoutInSeconds),

                MaxResponseContentBufferSize =
                    this.decisionConfiguration.MaxResponseContentBufferSizeInMegaBytes * 1024 * 1024
            };

            this.apiClient = new RESTFulApiFactoryClient(httpClient);
        }
    }
}
