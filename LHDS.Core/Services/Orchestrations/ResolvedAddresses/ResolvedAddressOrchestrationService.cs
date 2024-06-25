// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        public ValueTask AddDocumentAsync(byte[] data, string fileName, string container) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddressArgsOnAdd(data, fileName, container);

                using (Stream input = new MemoryStream(data))
                {
                    await this.documentProcessingService.AddDocumentAsync(input, fileName, container);
                }
            });

        public ValueTask RemoveDocumentByFileNameAsync(string fileName, string container) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddressArgsOnRemove(fileName, container);
                await this.documentProcessingService.RemoveDocumentByFileNameAsync(fileName, container);
            });

        public ValueTask<Guid?> UploadResolvedAddressesAsync() =>
            TryCatch(async () =>
            {
                List<ResolvedAddress> resolvedAddresses =
                    this.resolvedAddressProcessingService.RetrieveAllResolvedAddresses()
                        .Where(resolvedAddresses => resolvedAddresses.IsMatched == true
                            && resolvedAddresses.IsProcessed == false).ToList();

                if (resolvedAddresses.Count > 0)
                {
                    List<ResolvedAddressReturn> returnAddresses = resolvedAddresses.Select(resolvedAddress =>
                        new ResolvedAddressReturn
                        {
                            UniqueReference = resolvedAddress.UniqueReference,
                            UPRN = resolvedAddress.MatchedUPRN,
                            UPSN = resolvedAddress.MatchedUPSN,
                            OrganisationName = resolvedAddress.MatchedOrganisationName,
                            DepartmentName = resolvedAddress.MatchedDepartmentName,
                            SubBuildingName = resolvedAddress.MatchedSubBuildingName,
                            BuildingName = resolvedAddress.MatchedBuildingName,
                            BuildingNumber = resolvedAddress.MatchedBuildingNumber,
                            DependentThoroughfare = resolvedAddress.MatchedDependentThoroughfare,
                            Thoroughfare = resolvedAddress.MatchedThoroughfare,
                            DoubleDependentLocality = resolvedAddress.MatchedDoubleDependentLocality,
                            DependentLocality = resolvedAddress.MatchedDependentLocality,
                            PostTown = resolvedAddress.MatchedPostTown,
                            PostCode = resolvedAddress.MatchedPostCode,
                        }).ToList();

                    string resolvedAddressesCsv =
                        await this.csvHelperBroker.MapObjectToCsvAsync(returnAddresses, false, null, true);

                    Guid batchReferenceId = identifierBroker.GetIdentifier();
                    string fileName = $"{batchReferenceId}.csv";
                    byte[] documentData = Encoding.UTF8.GetBytes(resolvedAddressesCsv);
                    string container = blobContainers.Addresses;
                    var exceptions = new List<Exception>();

                    using (Stream input = new MemoryStream(documentData))
                    {
                        await this.documentProcessingService.AddDocumentAsync(input, fileName, container);
                    }

                    foreach (ResolvedAddress resolvedAddress in resolvedAddresses)
                    {
                        try
                        {
                            await TryCatch(async () =>
                            {
                                resolvedAddress.IsProcessed = true;
                                resolvedAddress.BatchReference = batchReferenceId;
                                resolvedAddress.UpdatedDate = dateTimeBroker.GetCurrentDateTimeOffset();
                                await this.resolvedAddressProcessingService.ModifyResolvedAddressAsync(resolvedAddress);
                            });
                        }
                        catch (Exception ex)
                        {
                            exceptions.Add(ex);
                        }
                    }

                    if (exceptions.Any())
                    {
                        throw new AggregateException(
                            message: $"Unable to modify resolved address for {exceptions.Count} resolved addresses " +
                                $"in batch: {batchReferenceId}",
                            exceptions);
                    }

                    return batchReferenceId;
                }
                else
                {
                    return null;
                }

            });
    }
}
