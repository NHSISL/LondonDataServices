// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Foundations.ResolvedAddresses;

namespace LHDS.Core.Services.Processings.ResolvedAddresses
{
    public partial class ResolvedAddressProcessingService : IResolvedAddressProcessingService
    {
        private readonly IResolvedAddressService resolvedAddressService;
        private readonly ILoggingBroker loggingBroker;

        public ResolvedAddressProcessingService(
            IResolvedAddressService resolvedAddressService,
            ILoggingBroker loggingBroker)
        {
            this.resolvedAddressService = resolvedAddressService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<ResolvedAddress> AddResolvedAddressAsync(ResolvedAddress resolvedAddress) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddress(resolvedAddress);

                return await resolvedAddressService.AddResolvedAddressAsync(resolvedAddress);
            });

        public ValueTask BulkAddResolvedAddressesAsync(List<ResolvedAddress> resolvedAddresses, string fileName) =>
            TryCatch(async () =>
            {
                ValidateArguments(resolvedAddresses, fileName);

                await this.resolvedAddressService.BulkAddResolvedAddressesAsync(resolvedAddresses, fileName);
            });

        public IQueryable<ResolvedAddress> RetrieveAllResolvedAddresses() =>
            TryCatch(() => resolvedAddressService.RetrieveAllResolvedAddresses());

        public ValueTask<ResolvedAddress> RetrieveResolvedAddressByIdAsync(Guid resolvedAddressId) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddressId(resolvedAddressId);

                return await resolvedAddressService.RetrieveResolvedAddressByIdAsync(resolvedAddressId);
            });

        public ValueTask<ResolvedAddress> RetrieveOrAddResolvedAddressAsync(ResolvedAddress resolvedAddress) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddress(resolvedAddress);

                return await resolvedAddressService.RetrieveResolvedAddressByIdAsync(resolvedAddress.Id) ??
                    await resolvedAddressService.AddResolvedAddressAsync(resolvedAddress);
            });

        public ValueTask<ResolvedAddress> ModifyOrAddResolvedAddressAsync(ResolvedAddress resolvedAddress) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddress(resolvedAddress);
                ValidateResolvedAddressId(resolvedAddress.Id);

                var maybeResolvedAddress = resolvedAddressService.RetrieveAllResolvedAddresses()
                    .FirstOrDefault(address => address.UniqueReference == resolvedAddress.UniqueReference);

                if (maybeResolvedAddress != null)
                {
                    return await resolvedAddressService.ModifyResolvedAddressAsync(resolvedAddress);
                }
                else
                {
                    return await resolvedAddressService.AddResolvedAddressAsync(resolvedAddress);
                }
            });

        public ValueTask<ResolvedAddress> ModifyResolvedAddressAsync(ResolvedAddress resolvedAddress) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddress(resolvedAddress);

                return await resolvedAddressService.ModifyResolvedAddressAsync(resolvedAddress);
            });

        public ValueTask<ResolvedAddress> RemoveResolvedAddressByIdAsync(Guid resolvedAddressId) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddressId(resolvedAddressId);

                return await resolvedAddressService.RemoveResolvedAddressByIdAsync(resolvedAddressId);
            });
    }
}