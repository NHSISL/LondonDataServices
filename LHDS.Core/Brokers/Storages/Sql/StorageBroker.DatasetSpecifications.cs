// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using Microsoft.EntityFrameworkCore;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial class StorageBroker
    {
        public DbSet<DataSetSpecification> DataSetSpecifications { get; set; }

        public async ValueTask<DataSetSpecification> InsertDataSetSpecificationAsync(
            DataSetSpecification dataSetSpecification,
            CancellationToken cancellationToken = default) =>
                await InsertAsync(dataSetSpecification, cancellationToken);

        public async ValueTask<IQueryable<DataSetSpecification>> SelectAllDataSetSpecificationsAsync(
            CancellationToken cancellationToken = default) =>
                await SelectAllAsync<DataSetSpecification>(cancellationToken);

        public async ValueTask<DataSetSpecification> SelectDataSetSpecificationByIdAsync(
            Guid dataSetSpecificationId,
            CancellationToken cancellationToken = default) =>
                await SelectAsync<DataSetSpecification>(new object[] { dataSetSpecificationId }, cancellationToken);

        public async ValueTask<DataSetSpecification> UpdateDataSetSpecificationAsync(
            DataSetSpecification dataSetSpecification,
            CancellationToken cancellationToken = default) =>
                await UpdateAsync(dataSetSpecification, cancellationToken);

        public async ValueTask<DataSetSpecification> DeleteDataSetSpecificationAsync(
            DataSetSpecification dataSetSpecification,
            CancellationToken cancellationToken = default) =>
                await DeleteAsync(dataSetSpecification, cancellationToken);
    }
}