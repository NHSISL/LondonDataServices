// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Coordinations.AddressCoordinations;
using LHDS.Core.Services.Orchestrations.Addresses;
using LHDS.Core.Services.Orchestrations.ResolvedAddresses;

namespace LHDS.Core.Services.Coordinations.AddressCoordinations
{
    public partial class AddressCoordinationService : IAddressCoordinationService
    {
        private readonly IAddressOrchestrationService addressOrchestrationService;
        private readonly IResolvedAddressOrchestrationService resolvedAddressOrchestrationService;
        private readonly ILoggingBroker loggingBroker;
        private readonly AddressConfiguration addressConfiguration;
        private readonly BlobContainers blobContainers;

        public AddressCoordinationService(
            IAddressOrchestrationService addressOrchestrationService,
            IResolvedAddressOrchestrationService resolvedAddressOrchestrationService,
            ILoggingBroker loggingBroker,
            AddressConfiguration addressConfiguration,
            BlobContainers blobContainers)
        {
            this.addressOrchestrationService = addressOrchestrationService;
            this.resolvedAddressOrchestrationService = resolvedAddressOrchestrationService;
            this.loggingBroker = loggingBroker;
            this.addressConfiguration = addressConfiguration;
            this.blobContainers = blobContainers;
        }

        public ValueTask LoadAddressDataAsync(Stream input, string filename) =>
            TryCatch(async () =>
            {
                ValidateDataOnProcessData(input, filename);
                await this.addressOrchestrationService.BulkAddAddressesAsync(input: input, filename);
            });

        public ValueTask SyncAddressesWithAssignAsync() =>
            TryCatch(async () =>
            {
                await this.addressOrchestrationService.SyncAddressesWithAssignAsync();
            });

        public ValueTask LoadAddressesToResolveAsync(Stream data, string filename) =>
            TryCatch(async () =>
            {
                ValidateDataOnProcessData(data, filename);
                await this.resolvedAddressOrchestrationService.UploadAddressesToReslveAsync(data, filename);
            });

        public ValueTask MatchAddressDataAsync() =>
            TryCatch(async () =>
            {
                await this.resolvedAddressOrchestrationService.MatchAddressDataAsync();
            });

        public ValueTask<Guid?> ExportResolvedAddressesAsync() =>
            TryCatch(async () =>
            {
                return await this.resolvedAddressOrchestrationService.ExportResolvedAddressesAsync();
            });
    }
}
