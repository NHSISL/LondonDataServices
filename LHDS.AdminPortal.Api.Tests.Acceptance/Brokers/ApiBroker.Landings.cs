// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using System.Web;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string LandingsRelativeUrl = "api/landings";

        public async ValueTask<string> GetLandingDocumentByFileNameAsync(string fileName)
        {
            try
            {
                var response = await this.apiFactoryClient.GetContentAsync<object>(
                            $"{LandingsRelativeUrl}/{HttpUtility.UrlEncode(fileName)}");

                return "test";
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }
    }
}
