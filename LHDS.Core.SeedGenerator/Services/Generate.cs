// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Audits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Services.Foundations.Audits;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.Suppliers;
using Tynamix.ObjectFiller;

namespace LHDS.Core.SeedGenerator.Services
{
    public partial class Generate : IGenerate
    {
        private readonly ISupplierService supplierService;
        private readonly IIngestionTrackingService ingestionTrackingService;
        private readonly IAuditService auditService;
        private readonly ILoggingBroker loggingBroker;

        public Generate(
            ISupplierService supplierService,
            IIngestionTrackingService ingestionTrackingService,
            IAuditService auditService,
            ILoggingBroker loggingBroker)
        {
            this.supplierService = supplierService;
            this.ingestionTrackingService = ingestionTrackingService;
            this.auditService = auditService;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask GenerateRecords(int supplierCount, int recordCount, int auditCount)
        {
            try
            {
                List<Supplier> suppliers = GenerateSuppliers(supplierCount);
                List<IngestionTracking> ingestionTrackingRecords = GenerateIngestionTrackingRecords(recordCount, suppliers);
                List<Audit> auditRecords = GenerateAuditRecords(auditCount, ingestionTrackingRecords);

                foreach (Supplier supplier in suppliers)
                {
                    var result = await this.supplierService.AddSupplierAsync(supplier);
                }

                //this.loggingBroker.LogInformation($"Created {suppliers.Count} suppliers.");

                foreach (IngestionTracking ingestionTracking in ingestionTrackingRecords)
                {
                    var result = await this.ingestionTrackingService.AddIngestionTrackingAsync(ingestionTracking);
                }

                //this.loggingBroker
                //    .LogInformation($"Created {ingestionTrackingRecords.Count} ingestion tracking records.");

                foreach (Audit audit in auditRecords)
                {
                    var result = await this.auditService.AddAuditAsync(audit);
                }

                //this.loggingBroker
                //    .LogInformation($"Created {auditRecords.Count} audit records.");
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
            }
        }

        private List<Supplier> GenerateSuppliers(int supplierCount)
        {
            return CreateRandomSuppliers(supplierCount);
        }

        private List<IngestionTracking> GenerateIngestionTrackingRecords(int recordCount, List<Supplier> suppliers)
        {
            return CreateRandomIngestionTrackingRecords(recordCount, suppliers);
        }

        private List<Audit> GenerateAuditRecords(int auditCount, List<IngestionTracking> ingestionTrackingRecords)
        {
            return CreateRandomAuditRecords(auditCount, ingestionTrackingRecords);
        }

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
    }
}
