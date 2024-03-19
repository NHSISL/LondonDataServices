// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Documents;

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

        public async ValueTask<Document> DownloadDocumentByFileNameAsync(string fileName)
        {
            return await this.apiFactoryClient.GetContentAsync<Document>(
                $"{emisLandingsRelativeUrl}/download/{HttpUtility.UrlEncode(fileName)}");
        }

        public async ValueTask<List<string>> RetrieveListOfDocumentsToProcessAsync(Guid subscriberAgreementId)
        {
            return await this.apiFactoryClient.GetContentAsync<List<string>>(
                $"{emisLandingsRelativeUrl}/{subscriberAgreementId}");
        }
    }
}
