// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Downloads;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string downloadsRelativeUrl = "api/downloads";

        public async ValueTask<List<Download>> RetrieveListOfDocumentsToProcessAsync(Download download) =>
            await this.apiFactoryClient.GetContentAsync<List<Download>>(downloadsRelativeUrl);
    }
}
