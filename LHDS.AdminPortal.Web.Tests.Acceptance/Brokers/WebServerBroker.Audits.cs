// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using LHDS.AdminPortal.Web.Tests.Acceptance.Models.Audits;
using Xunit;

namespace LHDS.AdminPortal.Web.Tests.Acceptance.Brokers
{
    public partial class WebServerBroker : IAsyncLifetime, IDisposable
    {
        private const string auditsRelativeUrl = "api/audits";

        public async ValueTask<Audit> PostAuditAsync(Audit audit) =>
            await this.apiFactoryClient.PostContentAsync(auditsRelativeUrl, audit);

        public async ValueTask<Audit> GetAuditByIdAsync(Guid auditId) =>
            await this.apiFactoryClient.GetContentAsync<Audit>($"{auditsRelativeUrl}/{auditId}");

        public async ValueTask<Audit> PutAuditAsync(Audit audit) =>
            await this.apiFactoryClient.PutContentAsync(auditsRelativeUrl, audit);

        public async ValueTask<Audit> DeleteAuditByIdAsync(Guid auditId) =>
            await this.apiFactoryClient.DeleteContentAsync<Audit>($"{auditsRelativeUrl}/{auditId}");
    }
}
