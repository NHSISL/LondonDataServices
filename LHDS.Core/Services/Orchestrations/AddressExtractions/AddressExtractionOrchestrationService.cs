// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
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
        private readonly IIdentifierBroker identifierBroker;

        public AddressExtractionOrchestrationService(
            IAddressParserService addressParserService,
            IAddressExtractionAuditService addressExtractionAuditService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker)
        {
            this.addressParserService = addressParserService;
            this.addressExtractionAuditService = addressExtractionAuditService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
        }

        public ValueTask<List<Address>> ProcessDataAsync(byte[] data) =>
            TryCatch(async () =>
            {
                ValidateDataOnProcessData(data);
                return await ProcessAddressDataAsync(data);
            });

        private async ValueTask<List<Address>> ProcessAddressDataAsync(byte[] data)
        {
            List<Address> addresses = new List<Address>();

            using (MemoryStream memoryStream = new MemoryStream(data))
            using (ZipArchive archive = new ZipArchive(memoryStream))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.Name.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                    {
                        using (var entryStream = entry.Open())
                        using (var tempMemoryStream = new MemoryStream())
                        {
                            await entryStream.CopyToAsync(tempMemoryStream);
                            byte[] csvData = tempMemoryStream.ToArray();

                            List<Address> csvAddresses =
                                await this.addressParserService.ProcessCsvAsync(csvData);

                            addresses.AddRange(csvAddresses);
                            var dateStamp = this.dateTimeBroker.GetCurrentDateTimeOffset();

                            var audit = new AddressExtractionAudit
                            {
                                Id = this.identifierBroker.GetIdentifier(),
                                CorrelationId = this.identifierBroker.GetIdentifier(),
                                FileName = $"{entry}",
                                Message = "Success",
                                MessageId = "",
                                CreatedBy = "System",
                                UpdatedBy = "System",
                                UpdatedDate = dateStamp,
                                CreatedDate = dateStamp,
                            };

                            await this.addressExtractionAuditService.AddAddressExtractionAuditAsync(audit);
                        }
                    }
                    else if (entry.Name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                    {
                        byte[] nestedZipData;

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
    }
}
