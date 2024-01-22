// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Documents;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string downloadsRelativeUrl = "api/downloads";

        public async ValueTask<List<Document>> RetrieveListOfDocumentsToProcessAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<Document>>(downloadsRelativeUrl);
    }
}
