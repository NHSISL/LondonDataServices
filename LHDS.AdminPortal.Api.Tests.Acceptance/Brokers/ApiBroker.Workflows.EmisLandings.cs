// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string emisLandingsRelativeUrl = "api/workflows/emislandings";

        public async ValueTask<List<string>> PostProcessDocumentsAsync() =>
            await this.apiFactoryClient.PostContentAsync(emisLandingsRelativeUrl, new List<string>());

        public async ValueTask<string> PostProcessDocumentByFileNameAsyncAsync(string filenName) =>
            await this.apiFactoryClient.PostContentAsync(emisLandingsRelativeUrl, filenName);
    }
}
