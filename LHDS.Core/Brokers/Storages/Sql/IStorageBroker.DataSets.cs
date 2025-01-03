// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.DataSets;

namespace LHDS.Core.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<DataSet> InsertDataSetAsync(DataSet dataSet);
        //IQueryable<DataSet> SelectAllDataSets();
        ValueTask<IQueryable<DataSet>> SelectAllDataSetsAsync();
        ValueTask<DataSet> SelectDataSetByIdAsync(Guid dataSetId);
        ValueTask<DataSet> UpdateDataSetAsync(DataSet dataSet);
        ValueTask<DataSet> DeleteDataSetAsync(DataSet dataSet);
    }
}