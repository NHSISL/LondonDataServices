using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Audits;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string AuditsRelativeUrl = "api/audits";

        public async ValueTask<Audit> PostAuditAsync(Audit audit) =>
            await this.apiFactoryClient.PostContentAsync(AuditsRelativeUrl, audit);

        public async ValueTask<Audit> GetAuditByIdAsync(Guid auditId) =>
            await this.apiFactoryClient.GetContentAsync<Audit>($"{AuditsRelativeUrl}/{auditId}");

        public async ValueTask<List<Audit>> GetAllAuditsAsync() =>
          await this.apiFactoryClient.GetContentAsync<List<Audit>>($"{AuditsRelativeUrl}/");

        public async ValueTask<Audit> PutAuditAsync(Audit audit) =>
            await this.apiFactoryClient.PutContentAsync(AuditsRelativeUrl, audit);

        public async ValueTask<Audit> DeleteAuditByIdAsync(Guid auditId) =>
            await this.apiFactoryClient.DeleteContentAsync<Audit>($"{AuditsRelativeUrl}/{auditId}");
    }
}
