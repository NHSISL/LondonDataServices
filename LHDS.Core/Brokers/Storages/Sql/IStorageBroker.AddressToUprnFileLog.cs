// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<AddressToUprnFileLog> InsertAddressToUprnFileLogAsync(
            AddressToUprnFileLog addressToUprnFileLog,
            CancellationToken cancellationToken = default);

        ValueTask<IQueryable<AddressToUprnFileLog>> SelectAllAddressToUprnFileLogsAsync(
            CancellationToken cancellationToken = default);

        ValueTask<AddressToUprnFileLog> SelectAddressToUprnFileLogByIdAsync(
            Guid addressToUprnFileLogId,
            CancellationToken cancellationToken = default);

        ValueTask<AddressToUprnFileLog> UpdateAddressToUprnFileLogAsync(
            AddressToUprnFileLog addressToUprnFileLog,
            CancellationToken cancellationToken = default);

        ValueTask<AddressToUprnFileLog> DeleteAddressToUprnFileLogAsync(
            AddressToUprnFileLog addressToUprnFileLog,
            CancellationToken cancellationToken = default);
    }
}
