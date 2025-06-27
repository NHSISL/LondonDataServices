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
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AssignAddresses;
using LHDS.Core.Models.Foundations.Audits;
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
        private readonly IAuditBroker auditBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly ICsvHelperBroker csvHelperBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly ISecurityBroker securityBroker;
        private readonly BlobContainers blobContainers;

        public ResolvedAddressOrchestrationService(
            IDocumentProcessingService documentProcessingService,
            IResolvedAddressProcessingService resolvedAddressProcessingService,
            IAssignProcessingService assignProcessingService,
            IAddressProcessingService addressProcessingService,
            IAuditBroker auditBroker,
            ILoggingBroker loggingBroker,
            ICsvHelperBroker csvHelperBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker,
            ISecurityBroker securityBroker,
            BlobContainers blobContainers)
        {
            this.documentProcessingService = documentProcessingService;
            this.resolvedAddressProcessingService = resolvedAddressProcessingService;
            this.assignProcessingService = assignProcessingService;
            this.addressProcessingService = addressProcessingService;
            this.auditBroker = auditBroker;
            this.loggingBroker = loggingBroker;
            this.csvHelperBroker = csvHelperBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
            this.securityBroker = securityBroker;
            this.blobContainers = blobContainers;
        }

        public ValueTask UploadAddressesToResolveAsync(Stream input, string fileName) =>
        TryCatch(async () =>
        {
            ValidateOnUploadAddressesToResolve(input, fileName);
            Guid correlationId = await this.identifierBroker.GetIdentifierAsync();

            await this.auditBroker.LogAsync(
                auditType: "Resolved Address Upload",
                title: "Uploading Resolved Addresses",
                message: $"Uploading addresses to resolve with correlation id {correlationId}",
                fileName: null,
                correlationId: correlationId.ToString());

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

                await this.auditBroker.LogAsync(
                    auditType: "Resolved Address Upload",
                    title: "Uploaded Resolved Addresses",
                    message: $"Uploaded addresses to resolve with correlation id {correlationId}",
                    fileName: null,
                    correlationId: correlationId.ToString());
            }
        });

        public ValueTask MatchAddressDataAsync() =>
        TryCatch(async () =>
        {
            Guid correlationId = await this.identifierBroker.GetIdentifierAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            ResolvedAddress? unMatchedResolvedAddress;
            var exceptions = new List<Exception>();
            List<Audit> audits = new List<Audit>();

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
                        DateTimeOffset matchingDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

                        Audit resolvedAddressMatchingAudit = new Audit
                        {
                            Id = await this.identifierBroker.GetIdentifierAsync(),
                            AuditType = "Resolved Address Match",
                            Title = "Matching Resolved Address",
                            Message = $"Matching resolved address for {unMatchedResolvedAddress.UniqueReference}",
                            CorrelationId = correlationId.ToString(),
                            FileName = null,
                            LogLevel = "Information",
                            CreatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty,
                            CreatedDate = matchingDateTimeOffset,
                            UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty,
                            UpdatedDate = matchingDateTimeOffset,
                        };

                        audits.Add(resolvedAddressMatchingAudit);

                        unMatchedResolvedAddress.IsProcessing = true;
                        unMatchedResolvedAddress.RetryCount += 1;
                        unMatchedResolvedAddress.UpdatedDate = await dateTimeBroker.GetCurrentDateTimeOffsetAsync();

                        ResolvedAddress updatedAddress = await resolvedAddressProcessingService.
                            ModifyResolvedAddressAsync(unMatchedResolvedAddress);

                        AssignAddress foundAssignAddress =
                            await assignProcessingService.MatchAddressAsync(
                                unMatchedResolvedAddress.UnstructuredPostalAddress);

                        Address? foundOrdananceAddress = null;

                        if (foundAssignAddress != null &&
                            foundAssignAddress.BestMatch != null &&
                            !string.IsNullOrWhiteSpace(foundAssignAddress.BestMatch.UPRN))
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

                        ValidateNewResolvedAddress(newResolvedAddress);
                        newResolvedAddress.UpdatedDate = await dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                        newResolvedAddress.IsProcessed = true;

                        await resolvedAddressProcessingService
                            .ModifyResolvedAddressAsync(newResolvedAddress);

                        DateTimeOffset matchingCompleteDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

                        Audit resolvedAddressMatchingCompleteAudit = new Audit
                        {
                            Id = await this.identifierBroker.GetIdentifierAsync(),
                            AuditType = "Resolved Address Match",
                            Title = "Resolved Address Matching Complete",
                            Message = $"Resolved address matching complete for {unMatchedResolvedAddress.UniqueReference}",
                            CorrelationId = correlationId.ToString(),
                            FileName = null,
                            LogLevel = "Information",
                            CreatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty,
                            CreatedDate = matchingCompleteDateTimeOffset,
                            UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty,
                            UpdatedDate = matchingCompleteDateTimeOffset,
                        };

                        audits.Add(resolvedAddressMatchingCompleteAudit);
                    });

                    await auditBroker.BulkLogAsync(audits);
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

        virtual internal ResolvedAddress MapOrdananceWithAssign(
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
            updatedResolovedAddress.Qualifier = foundAssignAddress?.BestMatch?.Qualifier;
            updatedResolovedAddress.Classification = foundAssignAddress?.BestMatch?.Classification;
            updatedResolovedAddress.Algorithm = foundAssignAddress?.BestMatch?.Algorithm;
            updatedResolovedAddress.MatchPattern = foundAssignAddress?.Pattern ?? null;
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
                        Guid correlationId = await this.identifierBroker.GetIdentifierAsync();

                        await this.auditBroker.LogAsync(
                            auditType: "Resolved Address Export",
                            title: "Exporting Resolved Addresses",
                            message: $"Exporting resolved addresses with correlation id {correlationId}",
                            fileName: null,
                            correlationId: correlationId.ToString());

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

                        await this.auditBroker.LogAsync(
                             auditType: "Resolved Address Export",
                             title: "Exported Resolved Addresses",
                             message: $"Exported resolved addresses with correlation id {correlationId}",
                             fileName: null,
                             correlationId: correlationId.ToString());
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