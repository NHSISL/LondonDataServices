// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.ResolvedAddresses;

namespace LHDS.Core.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressService : IResolvedAddressService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public ResolvedAddressService(
            IStorageBroker storageBroker,
            IIdentifierBroker identifierBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.identifierBroker = identifierBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<ResolvedAddress> AddResolvedAddressAsync(ResolvedAddress resolvedAddress) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddressOnAdd(resolvedAddress);

                return await this.storageBroker.InsertResolvedAddressAsync(resolvedAddress);
            });

        public ValueTask BulkAddResolvedAddressesAsync(List<ResolvedAddress> resolvedAddresses, string fileName) =>
            throw new NotImplementedException();

        public IQueryable<ResolvedAddress> RetrieveAllResolvedAddresses() =>
            TryCatch(() => this.storageBroker.SelectAllResolvedAddresses());

        public ValueTask<ResolvedAddress> RetrieveResolvedAddressByIdAsync(Guid resolvedAddressId) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddressId(resolvedAddressId);

                ResolvedAddress maybeResolvedAddress = await this.storageBroker
                    .SelectResolvedAddressByIdAsync(resolvedAddressId);

                ValidateStorageResolvedAddress(maybeResolvedAddress, resolvedAddressId);

                return maybeResolvedAddress;
            });

        public ValueTask<ResolvedAddress> ModifyResolvedAddressAsync(ResolvedAddress resolvedAddress) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddressOnModify(resolvedAddress);

                ResolvedAddress maybeResolvedAddress =
                    await this.storageBroker.SelectResolvedAddressByIdAsync(resolvedAddress.Id);

                ValidateStorageResolvedAddress(maybeResolvedAddress, resolvedAddress.Id);
                ValidateAgainstStorageResolvedAddressOnModify(inputResolvedAddress: resolvedAddress, storageResolvedAddress: maybeResolvedAddress);

                return await this.storageBroker.UpdateResolvedAddressAsync(resolvedAddress);
            });

        public ValueTask<ResolvedAddress> RemoveResolvedAddressByIdAsync(Guid resolvedAddressId) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddressId(resolvedAddressId);

                ResolvedAddress maybeResolvedAddress = await this.storageBroker
                    .SelectResolvedAddressByIdAsync(resolvedAddressId);

                ValidateStorageResolvedAddress(maybeResolvedAddress, resolvedAddressId);

                return await this.storageBroker.DeleteResolvedAddressAsync(maybeResolvedAddress);
            });
    }
}