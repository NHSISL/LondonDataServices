// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Coordinations.AddressCoordinations;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Orchestrations.AddressExtractions;
using LHDS.Core.Services.Orchestrations.AddressPersistances;
using LHDS.Core.Services.Orchestrations.ResolvedAddresses;

namespace LHDS.Core.Services.Coordinations.AddressCoordinations
{
    public partial class AddressCoordinationService : IAddressCoordinationService
    {
        private readonly IAddressExtractionOrchestrationService addressExtractionOrchestrationService;
        private readonly IAddressPersistanceOrchestrationService addressPersistanceOrchestrationService;
        private readonly IResolvedAddressOrchestrationService resolvedAddressOrchestrationService;
        private readonly ILoggingBroker loggingBroker;
        private readonly AddressConfiguration addressConfiguration;
        private readonly BlobContainers blobContainers;

        public AddressCoordinationService(
            IAddressExtractionOrchestrationService addressExtractionOrchestrationService,
            IAddressPersistanceOrchestrationService addressPersistanceOrchestrationService,
            IResolvedAddressOrchestrationService resolvedAddressOrchestrationService,
            ILoggingBroker loggingBroker,
            AddressConfiguration addressConfiguration,
            BlobContainers blobContainers)
        {
            this.addressExtractionOrchestrationService = addressExtractionOrchestrationService;
            this.addressPersistanceOrchestrationService = addressPersistanceOrchestrationService;
            this.resolvedAddressOrchestrationService = resolvedAddressOrchestrationService;
            this.loggingBroker = loggingBroker;
            this.addressConfiguration = addressConfiguration;
            this.blobContainers = blobContainers;
        }

        public ValueTask<List<Address>> LoadAddressDataAsync(byte[] data, string filename) =>
            TryCatch(async () =>
            {
                ValidateDataOnProcessData(data, filename);

                List<Address> extractedAddresses =
                    await this.addressExtractionOrchestrationService.ProcessAddressesAsync(data, filename);

                return extractedAddresses;
            });

        public ValueTask NormaliseAddressesAsync() =>
            TryCatch(async () =>
            {
                await this.addressExtractionOrchestrationService.NormaliseAddressesAsync();
            });

        public ValueTask MatchAddressDataAsync(byte[] data, string filename) =>
            TryCatch(async () =>
            {
                ValidateDataOnProcessData(data, filename);

                List<ResolvedAddress> extractedResolvedAddresses =
                    await this.addressExtractionOrchestrationService.ProcessResolvedAddressesAsync(data, filename);

                var exceptions = new List<Exception>();

                foreach (var resolvedAddress in extractedResolvedAddresses)
                {
                    try
                    {
                        ResolvedAddress matchedResolvedAddress = await TryCatch(async () =>
                        {
                            ResolvedAddress matchedAddress =
                                await this.addressPersistanceOrchestrationService.
                                    MatchAndPersistResolvedAddressAsync(resolvedAddress);

                            return matchedAddress;
                        });
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }

                if (exceptions.Any())
                {
                    string[] splitFilePath = filename.Split("/");
                    splitFilePath[2] = this.addressConfiguration.ErrorFolder;
                    string errorPath = String.Join("/", splitFilePath);
                    string addressContainer = this.blobContainers.Addresses;

                    using (Stream input = new MemoryStream(data))
                    {
                        await this.resolvedAddressOrchestrationService.
                            AddDocumentAsync(data, fileName: errorPath, container: addressContainer);
                    }

                    await this.resolvedAddressOrchestrationService.
                        RemoveDocumentByFileNameAsync(filename, container: addressContainer);

                    throw new AggregateException(
                        $"Unable to match {exceptions.Count} address in file {filename}. " +
                        $"File has been moved to the error folder.",
                        exceptions);
                }
            });

        public ValueTask<Guid?> UploadResolvedAddressesAsync() =>
            TryCatch(async () =>
            {
                return await this.resolvedAddressOrchestrationService.UploadResolvedAddressesAsync();
            });
    }
}
