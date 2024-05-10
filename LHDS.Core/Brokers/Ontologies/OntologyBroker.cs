// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using LHDS.Core.Models.Brokers.Ontologies;
using Newtonsoft.Json;
using RESTFulSense.Clients;

namespace LHDS.Core.Brokers.Ontologies
{
    public partial class OntologyBroker : IOntologyBroker
    {
        private readonly OntologyConfiguration ontologyConfiguration;
        private IRESTFulApiFactoryClient? apiClient = null;
        private OntologyAccessToken? ontologyAccessToken = null;

        public OntologyBroker(OntologyConfiguration ontologyConfiguration)
        {
            this.ontologyConfiguration = ontologyConfiguration;
            this.ontologyAccessToken = null;
            this.apiClient = null;
        }

        public async ValueTask<Bundle> GetAllAsync(string relativeUrl)
        {
            string jsonResponse = await GetAsync<string>(relativeUrl);
            var parser = new FhirJsonParser();
            Bundle bundle = parser.Parse<Bundle>(jsonResponse);

            return bundle;
        }

        public async ValueTask<string> GetArtifactDetailsAsync(string relativeUrl) =>
            await GetAsync<string>(relativeUrl);

        private async ValueTask<T> GetAsync<T>(string relativeUrl)
        {
            if (apiClient is null || DateTimeOffset.UtcNow <= ontologyAccessToken?.ExpiresAt)
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
            else
            {
                return await this.apiClient.GetContentAsync<T>(relativeUrl);
            }
        }

        private async ValueTask GetAccessTokenAsync()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var requestContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_id", ontologyConfiguration.ClientId),
                    new KeyValuePair<string, string>("client_secret", ontologyConfiguration.ClientSecret)
                });

                HttpResponseMessage response = await httpClient
                    .PostAsync(
                        requestUri: $"{ontologyConfiguration.TerminologyServerBaseUrl}" +
                            $"{ontologyConfiguration.TerminologyServerAuthenticationRelativeUrl}",
                        content: requestContent);

                if (response.IsSuccessStatusCode)
                {
                    DateTimeOffset now = DateTimeOffset.UtcNow;
                    string responseContent = await response.Content.ReadAsStringAsync() ?? string.Empty;
                    OntologyAccessToken? token = JsonConvert.DeserializeObject<OntologyAccessToken>(responseContent);

                    if (token is null)
                    {
                        throw new InvalidOperationException("Failed to deserialize access token");
                    }

                    token.ExpiresAt = now.AddSeconds(token.ExpiresIn);
                    this.ontologyAccessToken = token;
                }
                else
                {
                    throw new InvalidOperationException("Failed to obtain access token");
                }
            }
        }

        private async ValueTask SetupApiClient()
        {
            await GetAccessTokenAsync();

            var httpClient = new HttpClient()
            {
                BaseAddress =
                    new Uri(uriString: $"{this.ontologyConfiguration.TerminologyServerBaseUrl}"),
            };

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    scheme: "Bearer",
                    parameter: this.ontologyAccessToken?.AccessToken ?? "");

            this.apiClient = new RESTFulApiFactoryClient(httpClient);
        }
    }
}
