// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using LHDS.Core.Models.Brokers.Assigns;
using LHDS.Core.Models.Foundations.AssignAddresses;
using Newtonsoft.Json;
using RESTFulSense.Clients;

namespace LHDS.Core.Brokers.Assigns
{
    public class AssignBroker : IAssignBroker
    {
        private readonly AssignConfiguration assignConfiguration;
        private readonly IRESTFulApiFactoryClient apiClient;
        private readonly HttpClient httpClient;

        public AssignBroker(AssignConfiguration assignConfiguration)
        {
            this.assignConfiguration = assignConfiguration;
            this.httpClient = SetupHttpClient();
            this.apiClient = SetupApiClient();
        }

        public async ValueTask<AssignAddress> MatchAddressAsync(string address)
        {
            //var returnedAddress =
            //    await this.apiClient.GetContentAsync<AssignAddress>($"api/getinfo?adrec={address}");

            var jsonResponse = await this.apiClient.GetContentStringAsync($"/api/getinfo?adrec={address}");

            var returnedAddress = JsonConvert.DeserializeObject<AssignAddress>(jsonResponse);
            //var content = await response.Content.ReadAsStringAsync();
            //Console.WriteLine(response);
            //AssignAddress assign = new AssignAddress();
            return returnedAddress;
        }

        private HttpClient SetupHttpClient()
        {
            var httpClient = new HttpClient()
            {
                BaseAddress =
                    new Uri(uriString: this.assignConfiguration.ApiUrl),
            };

            return httpClient;
        }

        private IRESTFulApiFactoryClient SetupApiClient() =>
            new RESTFulApiFactoryClient(this.httpClient);
    }
}
