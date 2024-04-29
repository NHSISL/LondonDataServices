// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using CsvHelper.Configuration;
using Hl7.Fhir.Model;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Processings.CsvMappers;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.ResolvedAddresses;

namespace LHDS.Core.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationService : IResolvedAddressOrchestrationService
    {
        private readonly IDocumentProcessingService documentProcessingService;
        private readonly IResolvedAddressProcessingService resolvedAddressProcessingService;
        private readonly ICsvMapperProcessingService csvMapperProcessingService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly BlobContainers blobContainers;

        public ResolvedAddressOrchestrationService(
            IDocumentProcessingService documentProcessingService,
            IResolvedAddressProcessingService resolvedAddressProcessingService,
            ICsvMapperProcessingService csvMapperProcessingService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker,
            BlobContainers blobContainers)
        {
            this.documentProcessingService = documentProcessingService;
            this.resolvedAddressProcessingService = resolvedAddressProcessingService;
            this.csvMapperProcessingService = csvMapperProcessingService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
            this.blobContainers = blobContainers;
        }

        public ValueTask AddDocumentAsync(byte[] data, string fileName, string container) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddressArgsOnAdd(data, fileName, container);

                Document document = new Document
                {
                    FileName = fileName,
                    DocumentData = data
                };

                await this.documentProcessingService.AddDocumentAsync(document, container);
            });

        public ValueTask RemoveDocumentByFileNameAsync(string fileName, string container) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddressArgsOnRemove(fileName, container);
                await this.documentProcessingService.RemoveDocumentByFileNameAsync(fileName, container);
            });

        public ValueTask<Guid> UploadResolvedAddressesAsync() =>
            TryCatch(async () =>
            {
                List<ResolvedAddress> resolvedAddresses =
                    this.resolvedAddressProcessingService.RetrieveAllResolvedAddresses().
                        Where(resolvedAddresses => resolvedAddresses.IsMatched == true &&
                            resolvedAddresses.IsProcessed == false).ToList();

                string resolvedAddressesCsv =
                    await this.csvMapperProcessingService.MapObjectToCsvAsync(resolvedAddresses, false, true);

                Guid batchReferenceId = identifierBroker.GetIdentifier();
                string fileName = $"{batchReferenceId}.csv";
                byte[] documentData = Encoding.UTF8.GetBytes(resolvedAddressesCsv);
                string container = blobContainers.Addresses;

                Document resolvedAddressesDocument = new Document
                {
                    FileName = fileName,
                    DocumentData = documentData
                };

                //string addedFileName = 
                await this.documentProcessingService.
                    AddDocumentAsync(resolvedAddressesDocument, container);

                foreach (ResolvedAddress resolvedAddress in resolvedAddresses)
                {
                    resolvedAddress.IsProcessed = true;
                    resolvedAddress.BatchReference = batchReferenceId;

                    await this.resolvedAddressProcessingService.ModifyResolvedAddressAsync(resolvedAddress);
                }

                return batchReferenceId;
            });
    }
}
