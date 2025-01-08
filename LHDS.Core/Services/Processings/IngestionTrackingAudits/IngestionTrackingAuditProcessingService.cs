// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using LHDS.Core.Services.Processings.IngestionTrackingAudits;

namespace LHDS.Core.Services.Processings.IngestionTrackings
{
    public partial class IngestionTrackingAuditProcessingService : IIngestionTrackingAuditProcessingService
    {
        private readonly IIngestionTrackingAuditService ingestionTrackingAuditService;
        private readonly ILoggingBroker loggingBroker;

        public IngestionTrackingAuditProcessingService(
            IIngestionTrackingAuditService ingestionTrackingAuditService,
            ILoggingBroker loggingBroker)
        {
            this.ingestionTrackingAuditService = ingestionTrackingAuditService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<IngestionTrackingAudit> AddIngestionTrackingAuditAsync(IngestionTrackingAudit audit) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingAudit(audit);

                return await this.ingestionTrackingAuditService.AddIngestionTrackingAuditAsync(audit);
            });

        public ValueTask<IQueryable<IngestionTrackingAudit>> RetrieveAllIngestionTrackingAuditsAsync() =>
            TryCatch(async() => await this.ingestionTrackingAuditService.RetrieveAllIngestionTrackingAuditsAsync());

        public ValueTask<IngestionTrackingAudit> RetrieveIngestionTrackingAuditByIdAsync(Guid auditId) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingAuditId(auditId);

                return await this.ingestionTrackingAuditService.RetrieveIngestionTrackingAuditByIdAsync(auditId);
            });

        public ValueTask<IngestionTrackingAudit> RetrieveOrAddIngestionTrackingAuditAsync(IngestionTrackingAudit audit) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingAudit(audit);
                ValidateIngestionTrackingAuditId(audit.Id);

                return await this.ingestionTrackingAuditService.RetrieveIngestionTrackingAuditByIdAsync(audit.Id) ??
                    await this.ingestionTrackingAuditService.AddIngestionTrackingAuditAsync(audit);
            });

        public ValueTask<IngestionTrackingAudit> ModifyOrAddIngestionTrackingAuditAsync(IngestionTrackingAudit audit) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingAudit(audit);
                ValidateIngestionTrackingAuditId(audit.Id);
                var maybeIngestionTracking = await this.ingestionTrackingAuditService.RetrieveIngestionTrackingAuditByIdAsync(audit.Id);

                if (maybeIngestionTracking != null)
                {
                    return await this.ingestionTrackingAuditService.ModifyIngestionTrackingAuditAsync(audit);
                }
                else
                {
                    return await this.ingestionTrackingAuditService.AddIngestionTrackingAuditAsync(audit);
                }
            });

        public ValueTask<IngestionTrackingAudit> ModifyIngestionTrackingAuditAsync(IngestionTrackingAudit audit) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingAudit(audit);

                return await this.ingestionTrackingAuditService.ModifyIngestionTrackingAuditAsync(audit);
            });

        public ValueTask<IngestionTrackingAudit> RemoveIngestionTrackingAuditByIdAsync(Guid auditId) =>
            TryCatch(async () =>
            {
                ValidateIngestionTrackingAuditId(auditId);

                return await this.ingestionTrackingAuditService.RemoveIngestionTrackingAuditByIdAsync(auditId);
            });
    }
}
