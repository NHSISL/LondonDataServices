// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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

        public Generate(
            ISupplierService supplierService,
            IIngestionTrackingService ingestionTrackingService,
            IAuditService auditService)
        {
            this.supplierService = supplierService;
            this.ingestionTrackingService = ingestionTrackingService;
            this.auditService = auditService;
        }

        public void GenerateRecords(int supplierCount, int recordCount, int auditCount)
        {
            List<Supplier> suppliers = GenerateSuppliers(supplierCount);
            List<IngestionTracking> ingestionTrackingRecords = GenerateIngestionTrackingRecords(recordCount, suppliers);
            List<Audit> auditRecords = GenerateAuditRecords(auditCount, ingestionTrackingRecords);
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
