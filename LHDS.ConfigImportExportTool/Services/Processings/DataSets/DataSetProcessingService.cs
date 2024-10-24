// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;

namespace LHDS.ConfigImportExportTool.Services.Processings.DataSets
{
    internal class DataSetProcessingService : IDataSetProcessingService
    {
        public async ValueTask<IQueryable<DataSet>> RetrieveAllDataSetsAsync() =>
            throw new NotImplementedException();
    }
}
