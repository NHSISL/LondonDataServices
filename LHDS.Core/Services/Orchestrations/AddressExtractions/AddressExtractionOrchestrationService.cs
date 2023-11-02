// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using LHDS.Core.Services.Foundations.AddressExtractionAudits;
using LHDS.Core.Services.Foundations.AddressParsers;

namespace LHDS.Core.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExtractionOrchestrationService : IAddressExtractionOrchestrationService
    {
        private readonly IAddressParserService addressParserService;
        private readonly IAddressExtractionAuditService addressExtractionAuditService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public AddressExtractionOrchestrationService(
            IAddressParserService addressParserService,
            IAddressExtractionAuditService addressExtractionAuditService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.addressParserService = addressParserService;
            this.addressExtractionAuditService = addressExtractionAuditService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<List<Address>> ProcessDataAsync(byte[] data) =>
            TryCatch(async () =>
            {
                ValidateDataOnProcessData(data);
                return await ProcessAddressDataAsync(data);
            });

        private async ValueTask<List<Address>> ProcessAddressDataAsync(byte[] data)
        {
            try
            {
                List<Address> addresses = new List<Address>();
                using (MemoryStream memoryStream = new MemoryStream(data))
                using (ZipArchive archive = new ZipArchive(memoryStream))
                {
                    Console.WriteLine($"Zip entries: {archive.Entries.Count}");
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.Name.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                        {
                            using (var entryStream = entry.Open())
                            using (var tempMemoryStream = new MemoryStream())
                            {
                                await entryStream.CopyToAsync(tempMemoryStream);
                                byte[] csvData = tempMemoryStream.ToArray();

                                Console.WriteLine($"Csv name is: {entry.Name}");

                                List<Address> csvAddresses =
                                    await this.addressParserService.ProcessCsvAsync(csvData);

                                Console.WriteLine($"csv address list count is: {csvAddresses.Count}");
                                addresses.AddRange(csvAddresses);

                                var audit = new AddressExtractionAudit
                                {
                                    Id = Guid.NewGuid(),
                                    CorrelationId = Guid.NewGuid(),
                                    FileName = $"{entry}",
                                    Message = "Success",
                                    MessageId = "",
                                    CreatedBy = "System",
                                    UpdatedBy = "System",
                                    UpdatedDate = this.dateTimeBroker.GetCurrentDateTimeOffset(),
                                    CreatedDate = this.dateTimeBroker.GetCurrentDateTimeOffset(),
                                };

                                await this.addressExtractionAuditService.AddAddressExtractionAuditAsync(audit);
                            }
                        }
                        else if (entry.Name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                        {
                            byte[] nestedZipData;
                            Console.WriteLine($"Csv name is: {entry.Name}");
                            using (MemoryStream nestedMemoryStream = new MemoryStream())
                            using (Stream entryStream = entry.Open())
                            {
                                entryStream.CopyTo(nestedMemoryStream);
                                nestedZipData = nestedMemoryStream.ToArray();
                            }

                            if (nestedZipData.Length <= 0)
                            {
                                Console.WriteLine($"nestedZipData is null");
                                throw new Exception($"nestedZipData is null");
                            }

                            List<Address> nestedAddresses = await ProcessAddressDataAsync(nestedZipData);
                            addresses.AddRange(nestedAddresses);
                        }
                    }
                }

                return addresses;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
