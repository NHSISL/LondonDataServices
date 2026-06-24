// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Addresses;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask BulkInsertAddressesAsync(
            List<Address> address,
            bool useTransaction = true,
            CancellationToken cancellationToken = default);

        ValueTask<Address> InsertAddressAsync(Address address, CancellationToken cancellationToken = default);
        ValueTask<IQueryable<Address>> SelectAllAddressesAsync(CancellationToken cancellationToken = default);
        ValueTask<Address> SelectAddressByIdAsync(Guid addressId, CancellationToken cancellationToken = default);
        ValueTask<Address> UpdateAddressAsync(Address address, CancellationToken cancellationToken = default);

        ValueTask BulkUpdateAddressesAsync(
            List<Address> address,
            bool useTransaction = true,
            CancellationToken cancellationToken = default);

        ValueTask<Address> DeleteAddressAsync(Address address, CancellationToken cancellationToken = default);
    }
}
