// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSets;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<DataSet> InsertDataSetAsync(DataSet dataSet, CancellationToken cancellationToken = default);
        ValueTask<IQueryable<DataSet>> SelectAllDataSetsAsync(CancellationToken cancellationToken = default);
        ValueTask<DataSet> SelectDataSetByIdAsync(Guid dataSetId, CancellationToken cancellationToken = default);
        ValueTask<DataSet> UpdateDataSetAsync(DataSet dataSet, CancellationToken cancellationToken = default);
        ValueTask<DataSet> DeleteDataSetAsync(DataSet dataSet, CancellationToken cancellationToken = default);
    }
}