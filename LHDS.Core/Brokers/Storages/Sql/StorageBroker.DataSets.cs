// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSets;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<DataSet> DataSets { get; set; }

        public async ValueTask<DataSet> InsertDataSetAsync(
            DataSet dataSet,
            CancellationToken cancellationToken = default) =>
                await InsertAsync(dataSet, cancellationToken);

        public async ValueTask<IQueryable<DataSet>> SelectAllDataSetsAsync(
            CancellationToken cancellationToken = default) =>
                await SelectAllAsync<DataSet>(cancellationToken);

        public async ValueTask<DataSet> SelectDataSetByIdAsync(
            Guid dataSetId,
            CancellationToken cancellationToken = default) =>
                await SelectAsync<DataSet>(new object[] { dataSetId }, cancellationToken);

        public async ValueTask<DataSet> UpdateDataSetAsync(
            DataSet dataSet,
            CancellationToken cancellationToken = default) =>
                await UpdateAsync(dataSet, cancellationToken);

        public async ValueTask<DataSet> DeleteDataSetAsync(
            DataSet dataSet,
            CancellationToken cancellationToken = default) =>
                await DeleteAsync(dataSet, cancellationToken);
    }
}