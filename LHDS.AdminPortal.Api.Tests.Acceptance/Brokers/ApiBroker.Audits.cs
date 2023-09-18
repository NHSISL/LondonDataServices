// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Audits;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.OdataResponses;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string AuditsRelativeUrl = "api/audits";
        private const string AuditsRelativeOdataUrl = "odata/audits";

        public async ValueTask<Audit> PostAuditAsync(Audit audit) =>
            await this.apiFactoryClient.PostContentAsync(AuditsRelativeUrl, audit);

        public async ValueTask<Audit> GetAuditByIdAsync(Guid auditId) =>
            await this.apiFactoryClient.GetContentAsync<Audit>($"{AuditsRelativeUrl}/{auditId}");

        public async ValueTask<List<Audit>> GetAllAuditsAsync()
        {
            OdataResponse<Audit> response =
                await this.apiFactoryClient.GetContentAsync<OdataResponse<Audit>>($"{AuditsRelativeOdataUrl}/");

            return response.Items;
        }

        public async ValueTask<Audit> PutAuditAsync(Audit audit) =>
            await this.apiFactoryClient.PutContentAsync(AuditsRelativeUrl, audit);

        public async ValueTask<Audit> DeleteAuditByIdAsync(Guid auditId) =>
            await this.apiFactoryClient.DeleteContentAsync<Audit>($"{AuditsRelativeUrl}/{auditId}");
    }
}
