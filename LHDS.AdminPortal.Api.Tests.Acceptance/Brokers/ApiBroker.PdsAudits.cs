// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.OdataResponses;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.PdsAudits;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string PdsAuditsRelativeUrl = "api/pdsAudits";
        private const string PdsAuditsRelativeOdataUrl = "odata/pdsAudits";

        public async ValueTask<PdsAudit> PostPdsAuditAsync(PdsAudit ingestionTracking) =>
            await this.apiFactoryClient.PostContentAsync(PdsAuditsRelativeUrl, ingestionTracking);

        public async ValueTask<PdsAudit> GetPdsAuditByIdAsync(Guid ingestionTrackingId) =>
            await this.apiFactoryClient.GetContentAsync<PdsAudit>($"{PdsAuditsRelativeUrl}/{ingestionTrackingId}");

        public async ValueTask<List<PdsAudit>> GetAllPdsAuditsAsync()
        {
            OdataResponse<PdsAudit> response =
                await this.apiFactoryClient.GetContentAsync<OdataResponse<PdsAudit>>($"{PdsAuditsRelativeOdataUrl}/");

            return response.Items;
        }

        public async ValueTask<PdsAudit> PutPdsAuditAsync(PdsAudit ingestionTracking) =>
            await this.apiFactoryClient.PutContentAsync(PdsAuditsRelativeUrl, ingestionTracking);

        public async ValueTask<PdsAudit> DeletePdsAuditByIdAsync(Guid ingestionTrackingId) =>
            await this.apiFactoryClient.DeleteContentAsync<PdsAudit>($"{PdsAuditsRelativeUrl}/{ingestionTrackingId}");
    }
}
