// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.AdminPortal.Web.Tests.Acceptance.Models.IngestionTrackings;
using Xunit;

namespace LHDS.AdminPortal.Web.Tests.Acceptance.Brokers
{
    public partial class WebServerBroker : IAsyncLifetime, IDisposable
    {
        private const string ingestionTrackingsRelativeUrl = "api/ingestionTrackings";

        public async ValueTask<IngestionTracking> PostIngestionTrackingAsync(IngestionTracking ingestionTracking) =>
            await this.apiFactoryClient.PostContentAsync(ingestionTrackingsRelativeUrl, ingestionTracking);

        public async ValueTask<IngestionTracking> GetIngestionTrackingByIdAsync(Guid ingestionTrackingId) =>
            await this.apiFactoryClient.GetContentAsync<IngestionTracking>(
                $"{ingestionTrackingsRelativeUrl}/{ingestionTrackingId}");

        public async ValueTask<IngestionTracking> PutIngestionTrackingAsync(IngestionTracking ingestionTracking) =>
            await this.apiFactoryClient.PutContentAsync(ingestionTrackingsRelativeUrl, ingestionTracking);

        public async ValueTask<IngestionTracking> DeleteIngestionTrackingByIdAsync(Guid ingestionTrackingId) =>
            await this.apiFactoryClient.DeleteContentAsync<IngestionTracking>(
                $"{ingestionTrackingsRelativeUrl}/{ingestionTrackingId}");
    }
}
