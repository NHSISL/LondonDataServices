// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.ResolvedAddresses;

namespace LHDS.Core.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationService : IResolvedAddressOrchestrationService
    {
        private readonly IDocumentProcessingService documentProcessingService;
        private readonly IResolvedAddressProcessingService resolvedAddressProcessingService;
        private readonly ILoggingBroker loggingBroker;
        private readonly ICsvHelperBroker csvHelperBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly BlobContainers blobContainers;

        public ResolvedAddressOrchestrationService(
            IDocumentProcessingService documentProcessingService,
            IResolvedAddressProcessingService resolvedAddressProcessingService,
            ILoggingBroker loggingBroker,
            ICsvHelperBroker csvHelperBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker,
            BlobContainers blobContainers)
        {
            this.documentProcessingService = documentProcessingService;
            this.resolvedAddressProcessingService = resolvedAddressProcessingService;
            this.loggingBroker = loggingBroker;
            this.csvHelperBroker = csvHelperBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
            this.blobContainers = blobContainers;
        }

        public ValueTask UploadAddressesToReslveAsync(Stream input, string fileName) =>
        TryCatch(async () =>
        {
            ValidateOnUploadAddressesToResolve(input, fileName);

            using (StreamReader streamReader = new StreamReader(input))
            {
                input.Position = 0;
                string content = streamReader.ReadToEnd();

                Dictionary<string, int> fieldMappings =
                    new Dictionary<string, int>
                    {
                        { nameof(ResolvedAddress.UniqueReference), 0 },
                        { nameof(ResolvedAddress.UnstructuredPostalAddress), 2 }
                    };

                List<ResolvedAddress> resolvedAddresses = await this.csvHelperBroker
                    .MapCsvToObjectAsync<ResolvedAddress>(data: content, hasHeaderRecord: true, fieldMappings);

                await this.resolvedAddressProcessingService
                    .BulkAddResolvedAddressesAsync(resolvedAddresses, fileName);
            }
        });

        public ValueTask MatchAddressDataAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask<List<Guid>> ExportResolvedAddressesAsync() =>
            TryCatch(async () =>
            {
                throw new NotImplementedException();

                List<Guid> batchReferenceIds = new List<Guid>();

                // 1) Create a while loop to fetch the top 10,000 items where
                //    Matched = true, IsProcessing = false, IsExported = false and retry count <= 3
                // 2) Bulk update the items to set IsProcessing = true,
                //    BatchReference = batchReferenceId, retrycount += 1
                // 3) Create a CSV file with the data
                // 4) Upload the CSV file to the blob storage
                // 5) Bulk update the batch of items to set IsProcessing = false
                // 6) Add aggregate exception handling to catch excetions in the while loop.
                //    If any exceptions are caught, log them and continue reset all items in that bacth to
                //    IsProcessing = false, IsExported = false and increment the retry count by 1.
                // 7) Return the batchReferenceIds

                return await ValueTask.FromResult(batchReferenceIds);
            });
    }
}
