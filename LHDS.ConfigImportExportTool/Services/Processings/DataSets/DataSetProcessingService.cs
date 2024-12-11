// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;
using LHDS.ConfigImportExportTool.Services.Foundations.DataSets;

namespace LHDS.ConfigImportExportTool.Services.Processings.DataSets
{
    internal partial class DataSetProcessingService : IDataSetProcessingService
    {
        private readonly IDataSetService dataSetService;
        private readonly ILoggingBroker loggingBroker;

        public DataSetProcessingService(
            IDataSetService dataSetService, 
            ILoggingBroker loggingBroker)
        {
            this.dataSetService = dataSetService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<IQueryable<DataSet>> RetrieveAllDataSetsAsync() =>
            TryCatch(async() => await this.dataSetService.RetrieveAllDataSetsAsync());
    }
}
