// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;

namespace LHDS.ConfigImportExportTool.Brokers.Storages.Sql
{
    public partial interface IStorageBroker
    {
        ValueTask<DataSet> InsertDataSetAsync(DataSet dataSet);
        ValueTask<IQueryable<DataSet>> SelectAllDataSetsAsync();
        ValueTask<DataSet> SelectDataSetByIdAsync(Guid dataSetId);
        ValueTask<DataSet> UpdateDataSetAsync(DataSet dataSet);
        ValueTask<DataSet> DeleteDataSetAsync(DataSet dataSet);
    }
}