// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;
using LHDS.ConfigImportExportTool.Services.Foundations.DataSets;

namespace LHDS.ConfigImportExportTool.Services.Processings.DataSets
{
    internal class DataSetProcessingService : IDataSetProcessingService
    {
        private readonly IDataSetService ataSetService;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public async ValueTask<IQueryable<DataSet>> RetrieveAllDataSetsAsync() =>
            throw new NotImplementedException();
    }
}
