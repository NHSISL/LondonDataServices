// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Audits;
using LHDS.Core.Models.Foundations.OptOuts;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string OptOutsRelativeUrl = "api/optouts";

        public async ValueTask<OptOut> PostOptOutAsync(OptOut optOut) =>
            await this.apiFactoryClient.PostContentAsync<OptOut, OptOut>(OptOutsRelativeUrl, optOut);

        public async ValueTask<OptOut> DeleteOptOutByIdAsync(Guid optOutId) =>
            await this.apiFactoryClient.DeleteContentAsync<OptOut>($"{OptOutsRelativeUrl}/{optOutId}");

        public async ValueTask<OptOut> GetOptOutByNhsNumberAsync(string nhsNumber) =>
            await this.apiFactoryClient.GetContentAsync<OptOut>($"{OptOutsRelativeUrl}/{nhsNumber}");

        public async ValueTask<OptOut> PutOptOutAsync(OptOut optOut) =>
            await this.apiFactoryClient.PutContentAsync<OptOut>(OptOutsRelativeUrl, optOut);
    }
}
