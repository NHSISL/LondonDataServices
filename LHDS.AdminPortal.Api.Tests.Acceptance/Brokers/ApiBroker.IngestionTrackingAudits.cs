// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackingAudits;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.OdataResponses;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string auditsRelativeUrl = "api/IngestionTrackingAudits";
        private const string auditsRelativeOdataUrl = "odata/IngestionTrackingAudits";

        public async ValueTask<IngestionTrackingAudit> PostIngestionTrackingAuditAsync(
            IngestionTrackingAudit ingestionTrackingAudit) =>
                await this.apiFactoryClient.PostContentAsync(auditsRelativeUrl, ingestionTrackingAudit);

        public async ValueTask<IngestionTrackingAudit> GetIngestionTrackingAuditByIdAsync(
            Guid ingestionTrackingAuditId) =>
                await this.apiFactoryClient.GetContentAsync<IngestionTrackingAudit>(
                    $"{auditsRelativeUrl}/{ingestionTrackingAuditId}");

        public async ValueTask<List<IngestionTrackingAudit>> GetAllIngestionTrackingAuditsAsync()
        {
            OdataResponse<IngestionTrackingAudit> response =
                await this.apiFactoryClient.GetContentAsync<OdataResponse<IngestionTrackingAudit>>(
                    $"{auditsRelativeOdataUrl}/");

            return response.Items;
        }

        public async ValueTask<List<IngestionTrackingAudit>> FindIngestionTrackingAuditByIngestionTrackingIdAsync(
            Guid ingestionTrackingId) =>
                await this.apiFactoryClient.GetContentAsync<List<IngestionTrackingAudit>>(
                    $"{auditsRelativeUrl}/?$filter=IngestionTrackingId eq {ingestionTrackingId}");

        public async ValueTask<IngestionTrackingAudit> PutIngestionTrackingAuditAsync(
            IngestionTrackingAudit ingestionTrackingAudit) =>
                await this.apiFactoryClient.PutContentAsync(auditsRelativeUrl, ingestionTrackingAudit);

        public async ValueTask<IngestionTrackingAudit> DeleteIngestionTrackingAuditByIdAsync(
            Guid ingestionTrackingAuditId) =>
                await this.apiFactoryClient.DeleteContentAsync<IngestionTrackingAudit>(
                    $"{auditsRelativeUrl}/{ingestionTrackingAuditId}");
    }
}
