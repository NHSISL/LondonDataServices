// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
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
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly AddressConfiguration addressConfiguration;
        private readonly BlobContainers blobContainers;

        public AddressCoordinationService(
            IAddressExtractionOrchestrationService addressExtractionOrchestrationService,
            IAddressPersistanceOrchestrationService addressPersistanceOrchestrationService,
            IResolvedAddressOrchestrationService resolvedAddressOrchestrationService,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker,
            AddressConfiguration addressConfiguration,
            BlobContainers blobContainers)
        {
            this.addressExtractionOrchestrationService = addressExtractionOrchestrationService;
            this.addressPersistanceOrchestrationService = addressPersistanceOrchestrationService;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
            this.addressConfiguration = addressConfiguration;
            this.blobContainers = blobContainers;
        }

        public ValueTask<List<Address>> LoadAddressData(byte[] data, string filename) =>
            TryCatch(async () =>
            {
                ValidateDataOnProcessData(data, filename);

                List<Address> extractedAddress =
                    await this.addressExtractionOrchestrationService.ProcessAddressesAsync(data, filename);

                ValidateAddressListIsNotNull(extractedAddress);

                return await this.addressPersistanceOrchestrationService.PersistAddressAsync(extractedAddress);
            });

        public ValueTask<List<ResolvedAddress>> MatchAddressDataAsync(byte[] data, string filename) =>
            TryCatch(async () =>
            {
                ValidateDataOnProcessData(data, filename);

                List<ResolvedAddress> extractedResolvedAddresses =
                    await this.addressExtractionOrchestrationService.ProcessResolvedAddressesAsync(data, filename);

                var exceptions = new List<Exception>();
                List<ResolvedAddress> matchedAddresses = new List<ResolvedAddress>();

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

                        matchedAddresses.Add(matchedResolvedAddress);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }

                if (exceptions.Any())
                {
                    string[] splitFilePath = filename.Split("/");
                    splitFilePath[3] = this.addressConfiguration.ErrorFolder;
                    string errorPath = String.Join("/", splitFilePath);
                    string addressContainer = this.blobContainers.Addresses;

                    await this.resolvedAddressOrchestrationService.
                        AddDocumentAsync(data, fileName: errorPath, container: addressContainer);

                    await this.resolvedAddressOrchestrationService.
                        RemoveDocumentByFileNameAsync(filename, container: addressContainer);

                    throw new AggregateException(
                        $"Unable to match {exceptions.Count} address in file {filename}. " +
                        $"File has been moved to the error folder.",
                        exceptions);
                }

                return matchedAddresses;
            });

        public ValueTask<List<Address>> UploadResolvedAddresses() =>
            throw new System.NotImplementedException();
    }
}
