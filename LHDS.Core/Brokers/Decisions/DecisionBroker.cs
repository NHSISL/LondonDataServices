// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Decisions;
using LHDS.Core.Models.Foundations.Decisions;
using Newtonsoft.Json;

namespace LHDS.Core.Brokers.Decisions
{
    public class DecisionBroker : IDecisionBroker
    {
        private readonly DecisionConfiguration decisionConfiguration;
        private DecisionToken decisionToken = null;

        public DecisionBroker(DecisionConfiguration decisionConfiguration)
        {
            this.decisionConfiguration = decisionConfiguration;
            this.decisionToken = null;
        }

        public async ValueTask<List<Decision>> GetPatientDecisions(DateTimeOffset? lastPollDate)
        {
            throw new NotImplementedException();
        }

        public ValueTask RecordAdoption(List<Decision> decisionsAdopted)
        {
            throw new NotImplementedException();
        }

        private async ValueTask<string> GetTokenAsync()
        {
            if (this.decisionToken is not null && this.decisionToken.ExpiresAt > DateTimeOffset.UtcNow)
            {
                return this.decisionToken.AccessToken;
            }

            using (HttpClient httpClient = new HttpClient())
            {
                var requestContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_id", decisionConfiguration.ClientId),
                    new KeyValuePair<string, string>("client_secret", decisionConfiguration.ClientSecret)
                });

                HttpResponseMessage response = await httpClient
                    .PostAsync(
                        requestUri:
                            $"{decisionConfiguration.IDecideBaseUrl}" +
                            $"{decisionConfiguration.IDecideAuthenticationRelativeUrl}",
                        content: requestContent);

                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException("Failed to obtain access token");
                }

                string responseContent = await response.Content.ReadAsStringAsync();
                DecisionToken? token = JsonConvert.DeserializeObject<DecisionToken>(responseContent);

                if (token is null)
                {
                    throw new InvalidOperationException("Failed to deserialize access token");
                }

                token.ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(token.ExpiresIn);
                this.decisionToken = token;

                return token.AccessToken;
            }
        }
    }
}
