// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Audits;
using LHDS.Core.Services.Foundations.Audits;

namespace LHDS.Core.Services.Processings.IngestionTrackings
{
    public partial class IngestionTrackingAuditProcessingService : IIngestionTrackingAuditProcessingService
    {
        private readonly IAuditService ingestionTrackingAuditService;
        private readonly ILoggingBroker loggingBroker;

        public IngestionTrackingAuditProcessingService(
            IAuditService ingestionTrackingAuditService,
            ILoggingBroker loggingBroker)
        {
            this.ingestionTrackingAuditService = ingestionTrackingAuditService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Audit> AddIngestionTrackingAuditAsync(Audit audit) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingAudit(audit);

                return await this.ingestionTrackingAuditService.AddAuditAsync(audit);
            });

        public IQueryable<Audit> RetrieveAllIngestionTrackingAudits() =>
            TryCatch(() => this.ingestionTrackingAuditService.RetrieveAllAudits());

        public ValueTask<Audit> RetrieveIngestionTrackingAuditByIdAsync(Guid auditId) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingAuditId(auditId);

                return await this.ingestionTrackingAuditService.RetrieveAuditByIdAsync(auditId);
            });

        public ValueTask<Audit> RetrieveOrAddIngestionTrackingAuditAsync(Audit audit) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingAudit(audit);
                ValidateIngestionTrackingAuditId(audit.Id);

                return await this.ingestionTrackingAuditService.RetrieveAuditByIdAsync(audit.Id) ??
                    await this.ingestionTrackingAuditService.AddAuditAsync(audit);
            });

        public ValueTask<Audit> ModifyOrAddIngestionTrackingAuditAsync(Audit audit) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingAudit(audit);
                ValidateIngestionTrackingAuditId(audit.Id);
                var maybeIngestionTracking = await this.ingestionTrackingAuditService.RetrieveAuditByIdAsync(audit.Id);

                if (maybeIngestionTracking != null)
                {
                    return await this.ingestionTrackingAuditService.ModifyAuditAsync(audit);
                }
                else
                {
                    return await this.ingestionTrackingAuditService.AddAuditAsync(audit);
                }
            });

        public ValueTask<Audit> ModifyIngestionTrackingAuditAsync(Audit audit) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingAudit(audit);

                return await this.ingestionTrackingAuditService.ModifyAuditAsync(audit);
            });

        public ValueTask<Audit> RemoveIngestionTrackingAuditByIdAsync(Guid auditId) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingAuditId(auditId);

                return await this.ingestionTrackingAuditService.RemoveAuditByIdAsync(auditId);
            });
    }
}
