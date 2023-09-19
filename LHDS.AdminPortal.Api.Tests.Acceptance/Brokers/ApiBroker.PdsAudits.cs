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
        private const string pdsAuditsRelativeUrl = "api/pdsAudits";
        private const string pdsAuditsRelativeOdataUrl = "odata/pdsAudits";

        public async ValueTask<PdsAudit> PostPdsAuditAsync(PdsAudit ingestionTracking) =>
            await this.apiFactoryClient.PostContentAsync(pdsAuditsRelativeUrl, ingestionTracking);

        public async ValueTask<PdsAudit> GetPdsAuditByIdAsync(Guid ingestionTrackingId) =>
            await this.apiFactoryClient.GetContentAsync<PdsAudit>($"{pdsAuditsRelativeUrl}/{ingestionTrackingId}");

        public async ValueTask<List<PdsAudit>> GetAllPdsAuditsAsync()
        {
            OdataResponse<PdsAudit> response =
                await this.apiFactoryClient.GetContentAsync<OdataResponse<PdsAudit>>($"{pdsAuditsRelativeOdataUrl}/");

            return response.Items;
        }

        public async ValueTask<PdsAudit> PutPdsAuditAsync(PdsAudit ingestionTracking) =>
            await this.apiFactoryClient.PutContentAsync(pdsAuditsRelativeUrl, ingestionTracking);

        public async ValueTask<PdsAudit> DeletePdsAuditByIdAsync(Guid ingestionTrackingId) =>
            await this.apiFactoryClient.DeleteContentAsync<PdsAudit>($"{pdsAuditsRelativeUrl}/{ingestionTrackingId}");
    }
}
