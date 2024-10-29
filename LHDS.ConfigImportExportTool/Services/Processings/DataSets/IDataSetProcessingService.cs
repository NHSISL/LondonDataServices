// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;

namespace LHDS.ConfigImportExportTool.Services.Processings.DataSets
{
    internal interface IDataSetProcessingService
    {
        ValueTask<IQueryable<DataSet>> RetrieveAllDataSetsAsync();
    }
}
