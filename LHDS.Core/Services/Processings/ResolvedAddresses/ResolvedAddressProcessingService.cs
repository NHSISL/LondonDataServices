// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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

                return await this.resolvedAddressService.AddResolvedAddressAsync(resolvedAddress);
            });

        public IQueryable<ResolvedAddress> RetrieveAllResolvedAddresses() =>
            TryCatch(() => this.resolvedAddressService.RetrieveAllResolvedAddresses());

        public ValueTask<ResolvedAddress> RetrieveResolvedAddressByIdAsync(Guid resolvedAddressId) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddressId(resolvedAddressId);

                return await this.resolvedAddressService.RetrieveResolvedAddressByIdAsync(resolvedAddressId);
            });

        public ValueTask<ResolvedAddress> RetrieveOrAddResolvedAddressAsync(ResolvedAddress resolvedAddress) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddress(resolvedAddress);

                return await this.resolvedAddressService.RetrieveResolvedAddressByIdAsync(resolvedAddress.Id) ??
                    await this.resolvedAddressService.AddResolvedAddressAsync(resolvedAddress);
            });

        public ValueTask<ResolvedAddress> ModifyOrAddResolvedAddressAsync(ResolvedAddress resolvedAddress) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddress(resolvedAddress);
                ValidateResolvedAddressId(resolvedAddress.Id);
                var maybeResolvedAddress = await this.resolvedAddressService.RetrieveResolvedAddressByIdAsync(resolvedAddress.Id);

                if (maybeResolvedAddress != null)
                {
                    return await this.resolvedAddressService.ModifyResolvedAddressAsync(resolvedAddress);
                }
                else
                {
                    return await this.resolvedAddressService.AddResolvedAddressAsync(resolvedAddress);
                }
            });

        public ValueTask<ResolvedAddress> ModifyResolvedAddressAsync(ResolvedAddress resolvedAddress) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddress(resolvedAddress);

                return await this.resolvedAddressService.ModifyResolvedAddressAsync(resolvedAddress);
            });

        public ValueTask<ResolvedAddress> RemoveResolvedAddressByIdAsync(Guid resolvedAddressId) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddressId(resolvedAddressId);

                return await this.resolvedAddressService.RemoveResolvedAddressByIdAsync(resolvedAddressId);
            });

        public ValueTask<bool> IsExactMatchForResolvedAddressAsync(string address) =>
            throw new NotImplementedException();
    }
}