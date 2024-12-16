// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Services.Foundations.IngestionTrackingAudits;
using LHDS.Core.Services.Foundations.IngestionTrackings;
using LHDS.Core.Services.Foundations.Suppliers;
using Tynamix.ObjectFiller;

namespace LHDS.Core.SeedGenerator.Services
{
    public partial class Generate : IGenerate
    {
        private readonly ISupplierService supplierService;
        private readonly IIngestionTrackingService ingestionTrackingService;
        private readonly IIngestionTrackingAuditService auditService;
        private readonly ILoggingBroker loggingBroker;

        public Generate(
            ISupplierService supplierService,
            IIngestionTrackingService ingestionTrackingService,
            IIngestionTrackingAuditService auditService,
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
                List<Supplier> suppliers = await SetupSuppliers(supplierCount);

                List<IngestionTracking> ingestionTrackingRecords =
                    await SetupIngestionTrackingRecords(recordCount, suppliers);

                await SetupAuditRecords(auditCount, ingestionTrackingRecords);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                this.loggingBroker.LogError(ex);
            }
        }

        private async Task SetupAuditRecords(int auditCount, List<IngestionTracking> ingestionTrackingRecords)
        {
            try
            {
                List<IngestionTrackingAudit> auditRecords = GenerateAuditRecords(auditCount, ingestionTrackingRecords);

                foreach (IngestionTrackingAudit audit in auditRecords)
                {
                    try
                    {
                        var result = await this.auditService.AddIngestionTrackingAuditAsync(audit);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        this.loggingBroker.LogError(ex);
                    }
                }

                Console.WriteLine($"Created {auditRecords.Count} audit records.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private async Task<List<IngestionTracking>> SetupIngestionTrackingRecords(int recordCount, List<Supplier> suppliers)
        {
            try
            {
                List<IngestionTracking> ingestionTrackingRecords =
                GenerateIngestionTrackingRecords(recordCount, suppliers);

                foreach (IngestionTracking ingestionTracking in ingestionTrackingRecords)
                {
                    try
                    {
                        var result = await this.ingestionTrackingService.AddIngestionTrackingAsync(ingestionTracking);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        this.loggingBroker.LogError(ex);
                    }
                }

                Console.WriteLine($"Created {ingestionTrackingRecords.Count} ingestion tracking records.");
                return ingestionTrackingRecords;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private async Task<List<Supplier>> SetupSuppliers(int supplierCount)
        {
            try
            {
                List<Supplier> suppliers = GenerateSuppliers(supplierCount);

                foreach (Supplier supplier in suppliers)
                {
                    try
                    {
                        var result = await this.supplierService.AddSupplierAsync(supplier);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        this.loggingBroker.LogError(ex);
                    }
                }

                Console.WriteLine($"Created {suppliers.Count} suppliers.");

                return suppliers;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
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

        private List<IngestionTrackingAudit> GenerateAuditRecords(int auditCount, List<IngestionTracking> ingestionTrackingRecords)
        {
            return CreateRandomAuditRecords(auditCount, ingestionTrackingRecords);
        }

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static string GetRandomString(int minCharacters = 2, int maxCharacters = 10) =>
            new MnemonicString(wordCount: GetRandomNumber(min: minCharacters, max: maxCharacters)).GetValue();

        private static int GetRandomNumber(int min = 2, int max = 10) =>
            new IntRange(min: 2, max: 10).GetValue();
    }
}
