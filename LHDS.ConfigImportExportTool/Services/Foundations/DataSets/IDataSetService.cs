// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;

namespace LHDS.ConfigImportExportTool.Services.Foundations.DataSets
{
    public interface IDataSetService
    {
        ValueTask<IQueryable<DataSet>> RetrieveAllDataSetsAsync();
    }
}