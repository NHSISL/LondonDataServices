// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataTypes;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<DataType> DataTypes { get; set; }

        public async ValueTask<DataType> InsertDataTypeAsync(
            DataType dataType,
            CancellationToken cancellationToken = default) =>
                await InsertAsync(dataType, cancellationToken);

        public async ValueTask<IQueryable<DataType>> SelectAllDataTypesAsync(
            CancellationToken cancellationToken = default) =>
                await SelectAllAsync<DataType>(cancellationToken);

        public async ValueTask<DataType> SelectDataTypeByIdAsync(
            Guid dataTypeId,
            CancellationToken cancellationToken = default) =>
                await SelectAsync<DataType>(new object[] { dataTypeId }, cancellationToken);

        public async ValueTask<DataType> UpdateDataTypeAsync(
            DataType dataType,
            CancellationToken cancellationToken = default) =>
                await UpdateAsync(dataType, cancellationToken);

        public async ValueTask<DataType> DeleteDataTypeAsync(
            DataType dataType,
            CancellationToken cancellationToken = default) =>
                await DeleteAsync(dataType, cancellationToken);
    }
}
