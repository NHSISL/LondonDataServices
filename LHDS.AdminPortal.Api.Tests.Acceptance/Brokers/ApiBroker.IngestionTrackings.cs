// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.OdataResponses;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string ingestionTrackingsRelativeUrl = "api/ingestionTrackings";
        private const string ingestionTrackingsRelativeOdataUrl = "odata/ingestionTrackings";

        public async ValueTask<IngestionTracking> PostIngestionTrackingAsync(IngestionTracking ingestionTracking) =>
            await this.apiFactoryClient.PostContentAsync(ingestionTrackingsRelativeUrl, ingestionTracking);

        public async ValueTask<IngestionTracking> GetIngestionTrackingByIdAsync(Guid ingestionTrackingId) =>
            await this.apiFactoryClient.GetContentAsync<IngestionTracking>(
                $"{ingestionTrackingsRelativeUrl}/{ingestionTrackingId}");

        public async ValueTask<List<IngestionTracking>> GetAllIngestionTrackingsAsync()
        {
            OdataResponse<IngestionTracking> response =
                await this.apiFactoryClient.GetContentAsync<OdataResponse<IngestionTracking>>($"{ingestionTrackingsRelativeOdataUrl}/");

            return response.Items;
        }

        public async ValueTask<IngestionTracking> FindIngestionTrackingByFileNameAsync(
            string fileName)
        {
            string encodedFileName = HttpUtility.UrlEncode(fileName);

            return await this.apiFactoryClient.GetContentAsync<IngestionTracking>(
                $"{ingestionTrackingsRelativeUrl}/byfilename/{encodedFileName}");
        }

        public async ValueTask<IngestionTracking> PutIngestionTrackingAsync(IngestionTracking ingestionTracking) =>
            await this.apiFactoryClient.PutContentAsync(ingestionTrackingsRelativeUrl, ingestionTracking);

        public async ValueTask<IngestionTracking> DeleteIngestionTrackingByIdAsync(Guid ingestionTrackingId) =>
            await this.apiFactoryClient.DeleteContentAsync<IngestionTracking>(
                $"{ingestionTrackingsRelativeUrl}/{ingestionTrackingId}");
    }
}
