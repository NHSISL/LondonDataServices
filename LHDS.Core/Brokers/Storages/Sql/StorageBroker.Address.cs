// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<Address> Addresses { get; set; }

        public async ValueTask BulkInsertAddressesAsync(
            List<Address> addresses,
            bool useTransaction = true,
            CancellationToken cancellationToken = default) =>
                await BulkInsertAsync(addresses, useTransaction, cancellationToken);

        public async ValueTask<Address> InsertAddressAsync(
            Address address,
            CancellationToken cancellationToken = default) =>
                await InsertAsync(address, cancellationToken);

        public async ValueTask<IQueryable<Address>> SelectAllAddressesAsync(
            CancellationToken cancellationToken = default) =>
                await SelectAllAsync<Address>(cancellationToken);

        public async ValueTask<Address> SelectAddressByIdAsync(
            Guid addressId,
            CancellationToken cancellationToken = default) =>
                await SelectAsync<Address>(new object[] { addressId }, cancellationToken);

        public async ValueTask<Address> UpdateAddressAsync(
            Address address,
            CancellationToken cancellationToken = default) =>
                await UpdateAsync(address, cancellationToken);

        public async ValueTask BulkUpdateAddressesAsync(
            List<Address> addresses,
            bool useTransaction = true,
            CancellationToken cancellationToken = default) =>
                await BulkUpdateAsync(addresses, useTransaction, cancellationToken);

        public async ValueTask<Address> DeleteAddressAsync(
            Address address,
            CancellationToken cancellationToken = default) =>
                await DeleteAsync(address, cancellationToken);
    }
}
