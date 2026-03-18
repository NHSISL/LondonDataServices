// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string emisLandingsRelativeUrl = "api/emislandings";

        public async ValueTask<string> ReLandDocumentByFileNameAsync(string fileName)
        {
            return await this.apiFactoryClient.GetContentStringAsync(
                $"{emisLandingsRelativeUrl}/filename/{HttpUtility.UrlEncode(fileName)}");
        }

        public async ValueTask<List<string>> RetrieveListOfDocumentsToProcessAsync(Guid subscriberAgreementId)
        {
            return await this.apiFactoryClient.GetContentAsync<List<string>>(
                $"{emisLandingsRelativeUrl}/{subscriberAgreementId}");
        }

        public async ValueTask RedecryptDocumentByIngestionTrackingIdAsync(Guid ingestionTrackiingId)
        {
            await this.apiFactoryClient.PutContentAsync<IngestionTracking>(
                $"{emisLandingsRelativeUrl}/decrypt/{ingestionTrackiingId}");
        }
    }
}
