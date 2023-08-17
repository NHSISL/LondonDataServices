// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string IngestionTrackingsRelativeUrl = "api/ingestionTrackings";
        private const string IngestionTrackingsRelativeOdataUrl = "odata/ingestionTrackings";

        public async ValueTask<IngestionTracking> PostIngestionTrackingAsync(IngestionTracking ingestionTracking) =>
            await this.apiFactoryClient.PostContentAsync(IngestionTrackingsRelativeUrl, ingestionTracking);

        public async ValueTask<IngestionTracking> GetIngestionTrackingByIdAsync(Guid ingestionTrackingId) =>
            await this.apiFactoryClient.GetContentAsync<IngestionTracking>(
                $"{IngestionTrackingsRelativeUrl}/{ingestionTrackingId}");

        public async ValueTask<List<IngestionTracking>> GetAllIngestionTrackingsAsync() =>
          await this.apiFactoryClient.GetContentAsync<List<IngestionTracking>>($"{IngestionTrackingsRelativeUrl}/");

        public async ValueTask<IngestionTracking> PutIngestionTrackingAsync(IngestionTracking ingestionTracking) =>
            await this.apiFactoryClient.PutContentAsync(IngestionTrackingsRelativeUrl, ingestionTracking);

        public async ValueTask<IngestionTracking> DeleteIngestionTrackingByIdAsync(Guid ingestionTrackingId) =>
            await this.apiFactoryClient.DeleteContentAsync<IngestionTracking>(
                $"{IngestionTrackingsRelativeUrl}/{ingestionTrackingId}");
    }
}
