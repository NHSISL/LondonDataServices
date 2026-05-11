// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSetSpecifications;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<DataSetSpecification> InsertDataSetSpecificationAsync(
            DataSetSpecification dataSetSpecification,
            CancellationToken cancellationToken = default);

        ValueTask<IQueryable<DataSetSpecification>> SelectAllDataSetSpecificationsAsync(
            CancellationToken cancellationToken = default);

        ValueTask<DataSetSpecification> SelectDataSetSpecificationByIdAsync(
            Guid dataSetSpecificationId,
            CancellationToken cancellationToken = default);

        ValueTask<DataSetSpecification> UpdateDataSetSpecificationAsync(
            DataSetSpecification dataSetSpecification,
            CancellationToken cancellationToken = default);

        ValueTask<DataSetSpecification> DeleteDataSetSpecificationAsync(
            DataSetSpecification dataSetSpecification,
            CancellationToken cancellationToken = default);
    }
}