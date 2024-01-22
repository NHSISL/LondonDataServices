// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.OdataResponses;
using LHDS.Core.Models.Foundations.OptOuts;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string optOutsRelativeUrl = "api/optouts";
        private const string optOutsRelativeOdataUrl = "odata/optouts";

        public async ValueTask<OptOut> PostOptOutAsync(OptOut optOut) =>
            await this.apiFactoryClient.PostContentAsync<OptOut, OptOut>(optOutsRelativeUrl, optOut);

        public async ValueTask<OptOut> DeleteOptOutByIdAsync(Guid optOutId) =>
            await this.apiFactoryClient.DeleteContentAsync<OptOut>($"{optOutsRelativeUrl}/{optOutId}");

        public async ValueTask<List<OptOut>> GetAllOptOutsAsync()
        {
            OdataResponse<OptOut> response =
                await this.apiFactoryClient.GetContentAsync<OdataResponse<OptOut>>($"{optOutsRelativeOdataUrl}/");

            return response.Items;
        }

        public async ValueTask<OptOut> GetOptOutByNhsNumberAsync(string nhsNumber) =>
            await this.apiFactoryClient.GetContentAsync<OptOut>($"{optOutsRelativeUrl}/{nhsNumber}");

        public async ValueTask<OptOut> PutOptOutAsync(OptOut optOut) =>
            await this.apiFactoryClient.PutContentAsync<OptOut>(optOutsRelativeUrl, optOut);
    }
}
