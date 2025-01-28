// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AssignAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Processings.Addresses;
using LHDS.Core.Services.Processings.Assigns;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.ResolvedAddresses;

namespace LHDS.Core.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationService : IResolvedAddressOrchestrationService
    {
        private readonly IDocumentProcessingService documentProcessingService;
        private readonly IResolvedAddressProcessingService resolvedAddressProcessingService;
        private readonly IAssignProcessingService assignProcessingService;
        private readonly IAddressProcessingService addressProcessingService;
        private readonly ILoggingBroker loggingBroker;
        private readonly ICsvHelperBroker csvHelperBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly BlobContainers blobContainers;

        public ResolvedAddressOrchestrationService(
            IDocumentProcessingService documentProcessingService,
            IResolvedAddressProcessingService resolvedAddressProcessingService,
            IAssignProcessingService assignProcessingService,
            IAddressProcessingService addressProcessingService,
            ILoggingBroker loggingBroker,
            ICsvHelperBroker csvHelperBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker,
            BlobContainers blobContainers)
        {
            this.documentProcessingService = documentProcessingService;
            this.resolvedAddressProcessingService = resolvedAddressProcessingService;
            this.assignProcessingService = assignProcessingService;
            this.addressProcessingService = addressProcessingService;
            this.loggingBroker = loggingBroker;
            this.csvHelperBroker = csvHelperBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
            this.blobContainers = blobContainers;
        }

        public ValueTask UploadAddressesToResolveAsync(Stream input, string fileName) =>
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

        public ValueTask MatchAddressDataAsync() =>
        TryCatch(async () =>
        {
            ResolvedAddress? unMatchedResolvedAddress;
            var resolvedAddressAudits = new List<ResolvedAddress>();
            var exceptions = new List<Exception>();

            while (true)
            {
                var retrievedResolvedAddresses = await resolvedAddressProcessingService
                    .RetrieveAllResolvedAddressesAsync();

                unMatchedResolvedAddress = retrievedResolvedAddresses
                    .FirstOrDefault(address =>
                        address.IsProcessed == false &&
                        address.IsProcessing == false &&
                        address.RetryCount < 4);

                if (unMatchedResolvedAddress is null)
                {
                    break;
                }

                try
                {
                    await TryCatch(async () =>
                    {
                        unMatchedResolvedAddress.IsProcessing = true;
                        unMatchedResolvedAddress.RetryCount += 1;
                        unMatchedResolvedAddress.UpdatedDate = await dateTimeBroker.GetCurrentDateTimeOffsetAsync();

                        ResolvedAddress updatedAddress = await resolvedAddressProcessingService.
                            ModifyResolvedAddressAsync(unMatchedResolvedAddress);

                        AssignAddress foundAssignAddress =
                            await assignProcessingService.MatchAddressAsync(
                                unMatchedResolvedAddress.UnstructuredPostalAddress);

                        Address? foundOrdananceAddress = null;

                        if (foundAssignAddress != null && !string.IsNullOrWhiteSpace(foundAssignAddress.BestMatch.UPRN))
                        {
                            foundOrdananceAddress =
                                await addressProcessingService
                                    .RetrieveAddressByUPRNAsync(foundAssignAddress.BestMatch.UPRN);
                        }

                        ResolvedAddress newResolvedAddress =
                            MapOrdananceWithAssign(
                                updatedAddress,
                                foundAssignAddress,
                                foundOrdananceAddress);

                        ValidateAddressUPRN(newResolvedAddress.UPRN);
                        newResolvedAddress.UpdatedDate = await dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                        newResolvedAddress.IsProcessed = true;

                        await resolvedAddressProcessingService
                            .ModifyResolvedAddressAsync(newResolvedAddress);
                    });
                }
                catch (Exception ex)
                {
                    ResolvedAddress failedToProcessClean = unMatchedResolvedAddress.DeepClone();

                    ResolvedAddress failedToProcess = await this.resolvedAddressProcessingService
                        .RetrieveResolvedAddressByIdAsync(failedToProcessClean.Id);

                    failedToProcess.IsProcessing = false;
                    failedToProcess.UpdatedDate = await dateTimeBroker.GetCurrentDateTimeOffsetAsync();

                    await resolvedAddressProcessingService
                        .ModifyResolvedAddressAsync(failedToProcess);

                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(
                    $"Unable to retrieve message for {exceptions.Count} ResolvedAddresses",
                    exceptions);
            }
        });

        public static ResolvedAddress MapOrdananceWithAssign(
            ResolvedAddress unMatchedResolvedAddress,
            AssignAddress? foundAssignAddress,
            Address? foundOrdananceAddress)
        {
            ResolvedAddress updatedResolovedAddress = unMatchedResolvedAddress;
            updatedResolovedAddress.UPRN = foundOrdananceAddress?.UPRN ?? null;
            updatedResolovedAddress.UPSN = foundOrdananceAddress?.UPSN ?? null;
            updatedResolovedAddress.OrganisationName = foundOrdananceAddress?.OrganisationName;
            updatedResolovedAddress.DepartmentName = foundOrdananceAddress?.DepartmentName;
            updatedResolovedAddress.SubBuildingName = foundOrdananceAddress?.SubBuildingName;
            updatedResolovedAddress.BuildingName = foundOrdananceAddress?.BuildingName;
            updatedResolovedAddress.BuildingNumber = foundOrdananceAddress?.BuildingNumber;
            updatedResolovedAddress.DependentThoroughfare = foundOrdananceAddress?.DependentThoroughfare;
            updatedResolovedAddress.Thoroughfare = foundOrdananceAddress?.Thoroughfare;
            updatedResolovedAddress.DoubleDependentLocality = foundOrdananceAddress?.DoubleDependentLocality;
            updatedResolovedAddress.DependentLocality = foundOrdananceAddress?.DependentLocality;
            updatedResolovedAddress.PostTown = foundOrdananceAddress?.PostTown;
            updatedResolovedAddress.PostCode = foundOrdananceAddress?.PostCode;
            updatedResolovedAddress.AddressFormatQuality = foundAssignAddress?.AddressFormat;
            updatedResolovedAddress.PostCodeQuality = foundAssignAddress?.PostcodeQuality;
            updatedResolovedAddress.MatchedWithAssign = foundAssignAddress?.Matched ?? false;
            updatedResolovedAddress.Qualifier = foundAssignAddress?.BestMatch.Qualifier;
            updatedResolovedAddress.Classification = foundAssignAddress?.BestMatch.Classification;
            updatedResolovedAddress.Algorithm = foundAssignAddress?.BestMatch.Algorithm;
            updatedResolovedAddress.MatchPattern = foundAssignAddress?.Pattern;
            updatedResolovedAddress.IsProcessing = false;
            updatedResolovedAddress.IsExported = false;
            updatedResolovedAddress.RetryCount = 0;

            return updatedResolovedAddress;
        }

        public ValueTask<List<Guid>> ExportResolvedAddressesAsync() =>
        TryCatch(async () =>
        {
            List<ResolvedAddress> unMatchedResolvedAddresses;
            int batchCount = 10000;
            List<Guid> batchReferenceIds = new List<Guid>();
            var exceptions = new List<Exception>();

            while (true)
            {
                var retrievedResolvedAddresses = await resolvedAddressProcessingService
                    .RetrieveAllResolvedAddressesAsync();

                unMatchedResolvedAddresses = retrievedResolvedAddresses
                    .Where(address =>
                        address.IsExported == false &&
                        address.IsProcessing == false &&
                        address.RetryCount < 4)
                    .Take(batchCount)
                    .ToList();

                if (unMatchedResolvedAddresses.Count == 0)
                {
                    break;
                }

                Guid batchReference = await this.identifierBroker.GetIdentifierAsync();
                batchReferenceIds.Add(batchReference);

                try
                {
                    await TryCatch(async () =>
                    {
                        unMatchedResolvedAddresses.ForEach(setProcessing =>
                        {
                            setProcessing.IsProcessing = true;
                            setProcessing.RetryCount += 1;
                            setProcessing.BatchReference = batchReference;
                        });

                        await resolvedAddressProcessingService
                            .BulkModifyResolvedAddressesAsync(unMatchedResolvedAddresses);

                        Dictionary<string, int> fieldMappings = new Dictionary<string, int>
                        {
                            { nameof(ResolvedAddress.UniqueReference), 0 },
                            { nameof(ResolvedAddress.UPRN), 1 },
                            { nameof(ResolvedAddress.UPSN), 2 },
                            { nameof(ResolvedAddress.OrganisationName), 3 },
                            { nameof(ResolvedAddress.DepartmentName), 4 },
                            { nameof(ResolvedAddress.SubBuildingName), 5 },
                            { nameof(ResolvedAddress.BuildingName), 6 },
                            { nameof(ResolvedAddress.BuildingNumber), 7 },
                            { nameof(ResolvedAddress.DependentThoroughfare), 8 },
                            { nameof(ResolvedAddress.Thoroughfare), 9 },
                            { nameof(ResolvedAddress.DoubleDependentLocality), 10 },
                            { nameof(ResolvedAddress.DependentLocality), 11 },
                            { nameof(ResolvedAddress.PostTown), 12 },
                            { nameof(ResolvedAddress.PostCode), 13 },
                            { nameof(ResolvedAddress.AddressFormatQuality), 14 },
                            { nameof(ResolvedAddress.PostCodeQuality), 15 },
                            { nameof(ResolvedAddress.MatchedWithAssign), 16 },
                            { nameof(ResolvedAddress.Qualifier), 17 },
                            { nameof(ResolvedAddress.Classification), 18 },
                            { nameof(ResolvedAddress.Algorithm), 19 },
                            { nameof(ResolvedAddress.MatchPattern), 20 },
                            { nameof(ResolvedAddress.UnstructuredPostalAddress), 21 }
                        };

                        string processedData = await this.csvHelperBroker
                           .MapObjectToCsvAsync(
                            @object: unMatchedResolvedAddresses,
                            addHeaderRecord: true,
                            fieldMappings: fieldMappings,
                            shouldAddTrailingComma: false);

                        byte[] processedBytes = Encoding.UTF8.GetBytes(processedData);
                        batchReferenceIds.Add(batchReference);
                        string csvFileName = $"out/{batchReference}.csv";

                        using (Stream processed = new MemoryStream(processedBytes))
                        {
                            await this.documentProcessingService.AddDocumentAsync(
                                input: processed,
                                csvFileName,
                                container: blobContainers.Addresses);
                        }

                        List<ResolvedAddress> doneProcessingResolvedAddresses =
                            unMatchedResolvedAddresses.DeepClone();

                        doneProcessingResolvedAddresses.ForEach(setFinishedProcessing =>
                        {
                            setFinishedProcessing.BatchReference = batchReference;
                            setFinishedProcessing.IsProcessing = false;
                            setFinishedProcessing.RetryCount = 0;
                            setFinishedProcessing.IsExported = true;
                            setFinishedProcessing.IsProcessed = true;
                        });

                        await resolvedAddressProcessingService
                            .BulkModifyResolvedAddressesAsync(doneProcessingResolvedAddresses);
                    });
                }
                catch (Exception ex)
                {
                    IQueryable<ResolvedAddress> resolvedAddresses =
                        await resolvedAddressProcessingService.RetrieveAllResolvedAddressesAsync();

                    List<ResolvedAddress> failedToExport = resolvedAddresses
                        .Where(address => address.BatchReference == batchReference)
                        .ToList();

                    failedToExport.ForEach(setFinishedExporting =>
                    {
                        setFinishedExporting.IsProcessing = false;
                        setFinishedExporting.IsExported = false;
                        setFinishedExporting.IsProcessed = false;
                    });

                    await resolvedAddressProcessingService
                        .BulkModifyResolvedAddressesAsync(failedToExport);

                    exceptions.Add(ex);
                    batchReferenceIds.Remove(batchReference);
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(
                    $"Unable to export addresses for {exceptions.Count} ResolvedAddresses", exceptions);
            }

            return await ValueTask.FromResult(batchReferenceIds);
        });
    }
}