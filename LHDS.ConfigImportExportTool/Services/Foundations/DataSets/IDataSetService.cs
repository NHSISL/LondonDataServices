// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;

namespace LHDS.ConfigImportExportTool.Services.Foundations.DataSets
{
    public interface IDataSetService
    {
        ValueTask<DataSet> AddDataSetAsync(DataSet dataSet);
        ValueTask<IQueryable<DataSet>> RetrieveAllDataSetsAsync();
        ValueTask<DataSet> RetrieveDataSetByIdAsync(Guid dataSetId);
        ValueTask<DataSet> ModifyDataSetAsync(DataSet dataSet);
        ValueTask<DataSet> RemoveDataSetByIdAsync(Guid dataSetId);
    }
}