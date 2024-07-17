// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        public ValueTask UploadAddressesToReslveAsync(Stream input, string fileName)
        {
            throw new NotImplementedException();
        }

        public ValueTask MatchAddressDataAsync() =>
            TryCatch(async () =>
            {
                ResolvedAddress? unMatchedResolvedAddress;

                while ((unMatchedResolvedAddress = resolvedAddressProcessingService.RetrieveAllResolvedAddresses()
                    .FirstOrDefault(x => x.Matched == false && x.IsProcessing == false && x.RetryCount < 4)) != null)
                {
                    if (unMatchedResolvedAddress != null)
                    {
                        unMatchedResolvedAddress.IsProcessing = true;
                        unMatchedResolvedAddress.RetryCount += 1;
                        unMatchedResolvedAddress.UpdatedDate = dateTimeBroker.GetCurrentDateTimeOffset();

                        ResolvedAddress updatedAddress = await resolvedAddressProcessingService.
                            ModifyResolvedAddressAsync(unMatchedResolvedAddress);

                        AssignAddress foundAssignAddress =
                            await assignProcessingService.MatchAddressAsync(
                                unMatchedResolvedAddress.UnstructuredPostalAddress);

                        ValidateUPRNHasValue(foundAssignAddress.UPRN);

                        Address? foundOrdananceAddress =
                            await addressProcessingService
                                .RetrieveAddressByUPRNAsync(foundAssignAddress.UPRN.ToString());

                        ResolvedAddress newResolvedAddress =
                            MapOrdananceWithAssign(
                                updatedAddress,
                                foundAssignAddress,
                                foundOrdananceAddress);

                        newResolvedAddress.UpdatedDate = dateTimeBroker.GetCurrentDateTimeOffset();

                        await resolvedAddressProcessingService
                            .ModifyResolvedAddressAsync(newResolvedAddress);
                    }
                }
            });

        public static ResolvedAddress MapOrdananceWithAssign(
            ResolvedAddress unMatchedResolvedAddress,
            AssignAddress foundAssignAddress,
            Address? foundOrdananceAddress)
        {
            ResolvedAddress UpdatedResolovedAddress = unMatchedResolvedAddress;
            UpdatedResolovedAddress.UPSN = foundOrdananceAddress.UPSN ?? null;
            UpdatedResolovedAddress.OrganisationName = foundOrdananceAddress.OrganisationName;
            UpdatedResolovedAddress.DepartmentName = foundOrdananceAddress.DepartmentName;
            UpdatedResolovedAddress.SubBuildingName = foundOrdananceAddress.SubBuildingName;
            UpdatedResolovedAddress.BuildingName = foundOrdananceAddress.BuildingName;
            UpdatedResolovedAddress.BuildingNumber = foundOrdananceAddress.BuildingNumber;
            UpdatedResolovedAddress.DependentThoroughfare = foundOrdananceAddress.DependentThoroughfare;
            UpdatedResolovedAddress.Thoroughfare = foundOrdananceAddress.Thoroughfare;
            UpdatedResolovedAddress.DoubleDependentLocality = foundOrdananceAddress.DoubleDependentLocality;
            UpdatedResolovedAddress.DependentLocality = foundOrdananceAddress.DependentLocality;
            UpdatedResolovedAddress.PostTown = foundOrdananceAddress.PostTown;
            UpdatedResolovedAddress.PostCode = foundOrdananceAddress.PostCode;
            UpdatedResolovedAddress.AddressFormatQuality = foundAssignAddress.AddressFormat;
            UpdatedResolovedAddress.PostCodeQuality = foundAssignAddress.PostcodeQuality;
            UpdatedResolovedAddress.Matched = foundAssignAddress.Matched;
            UpdatedResolovedAddress.Qualifier = foundAssignAddress.Qualifier;
            UpdatedResolovedAddress.Classification = foundAssignAddress.Classification;
            UpdatedResolovedAddress.Algorithm = foundAssignAddress.Algorithm;
            UpdatedResolovedAddress.MatchPattern = foundAssignAddress.MatchPattern.ToString();
            UpdatedResolovedAddress.IsProcessing = false;
            UpdatedResolovedAddress.IsExported = false;
            UpdatedResolovedAddress.RetryCount = 0;

            return UpdatedResolovedAddress;
        }

        public ValueTask<Guid?> ExportResolvedAddressesAsync() =>
            TryCatch(async () =>
            {
                throw new NotImplementedException();

                return await ValueTask.FromResult<Guid?>(null);

                //List<ResolvedAddress> resolvedAddresses =
                //    this.resolvedAddressProcessingService.RetrieveAllResolvedAddresses()
                //        .Where(resolvedAddresses => resolvedAddresses.IsMatched == true
                //            && resolvedAddresses.IsProcessed == false).ToList();

                //if (resolvedAddresses.Count > 0)
                //{
                //    List<ResolvedAddressReturn> returnAddresses = resolvedAddresses.Select(resolvedAddress =>
                //        new ResolvedAddressReturn
                //        {
                //            UniqueReference = resolvedAddress.UniqueReference,
                //            UPRN = resolvedAddress.MatchedUPRN,
                //            UPSN = resolvedAddress.MatchedUPSN,
                //            OrganisationName = resolvedAddress.MatchedOrganisationName,
                //            DepartmentName = resolvedAddress.MatchedDepartmentName,
                //            SubBuildingName = resolvedAddress.MatchedSubBuildingName,
                //            BuildingName = resolvedAddress.MatchedBuildingName,
                //            BuildingNumber = resolvedAddress.MatchedBuildingNumber,
                //            DependentThoroughfare = resolvedAddress.MatchedDependentThoroughfare,
                //            Thoroughfare = resolvedAddress.MatchedThoroughfare,
                //            DoubleDependentLocality = resolvedAddress.MatchedDoubleDependentLocality,
                //            DependentLocality = resolvedAddress.MatchedDependentLocality,
                //            PostTown = resolvedAddress.MatchedPostTown,
                //            PostCode = resolvedAddress.MatchedPostCode,
                //        }).ToList();

                //    string resolvedAddressesCsv =
                //        await this.csvHelperBroker.MapObjectToCsvAsync(returnAddresses, false, null, true);

                //    Guid batchReferenceId = identifierBroker.GetIdentifier();
                //    string fileName = $"{batchReferenceId}.csv";
                //    byte[] documentData = Encoding.UTF8.GetBytes(resolvedAddressesCsv);
                //    string container = blobContainers.Addresses;
                //    var exceptions = new List<Exception>();

                //    using (Stream input = new MemoryStream(documentData))
                //    {
                //        await this.documentProcessingService.AddDocumentAsync(input, fileName, container);
                //    }

                //    foreach (ResolvedAddress resolvedAddress in resolvedAddresses)
                //    {
                //        try
                //        {
                //            await TryCatch(async () =>
                //            {
                //                resolvedAddress.IsProcessed = true;
                //                resolvedAddress.BatchReference = batchReferenceId;
                //                resolvedAddress.UpdatedDate = dateTimeBroker.GetCurrentDateTimeOffset();
                //                await this.resolvedAddressProcessingService.ModifyResolvedAddressAsync(resolvedAddress);
                //            });
                //        }
                //        catch (Exception ex)
                //        {
                //            exceptions.Add(ex);
                //        }
                //    }

                //    if (exceptions.Any())
                //    {
                //        throw new AggregateException(
                //            message: $"Unable to modify resolved address for {exceptions.Count} resolved addresses " +
                //                $"in batch: {batchReferenceId}",
                //            exceptions);
                //    }

                //    return batchReferenceId;
                //}
                //else
                //{
                //    return null;
                //}

            });
    }
}
