// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string LandingsRelativeUrl = "api/landings";

        public async ValueTask<string> GetLandingDocumentByFileNameAsync(string fileName)
        {
            var jsonResponse = await this.apiFactoryClient.GetContentAsync<string>(
                        $"{LandingsRelativeUrl}/{HttpUtility.UrlEncode(fileName)}");

            var responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
            return responseObject.path;
        }
    }
}
