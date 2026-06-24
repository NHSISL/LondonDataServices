// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<AddressToUprnFileLog> AddressToUprnFileLogs { get; set; }

        public async ValueTask<AddressToUprnFileLog> InsertAddressToUprnFileLogAsync(
            AddressToUprnFileLog addressToUprnFileLog,
            CancellationToken cancellationToken = default) =>
                await InsertAsync(addressToUprnFileLog, cancellationToken);

        public async ValueTask<IQueryable<AddressToUprnFileLog>> SelectAllAddressToUprnFileLogsAsync(
            CancellationToken cancellationToken = default) =>
                await SelectAllAsync<AddressToUprnFileLog>(cancellationToken);

        public async ValueTask<AddressToUprnFileLog> SelectAddressToUprnFileLogByIdAsync(
            Guid addressToUprnFileLogId,
            CancellationToken cancellationToken = default) =>
                await SelectAsync<AddressToUprnFileLog>(new object[] { addressToUprnFileLogId }, cancellationToken);

        public async ValueTask<AddressToUprnFileLog> UpdateAddressToUprnFileLogAsync(
            AddressToUprnFileLog addressToUprnFileLog,
            CancellationToken cancellationToken = default) =>
                await UpdateAsync(addressToUprnFileLog, cancellationToken);

        public async ValueTask<AddressToUprnFileLog> DeleteAddressToUprnFileLogAsync(
            AddressToUprnFileLog addressToUprnFileLog,
            CancellationToken cancellationToken = default) =>
                await DeleteAsync(addressToUprnFileLog, cancellationToken);
    }
}
