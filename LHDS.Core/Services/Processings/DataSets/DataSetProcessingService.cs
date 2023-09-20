// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Services.Foundations.DataSets;

namespace LHDS.Core.Services.Processings.DataSets
{
    public partial class DataSetProcessingService : IDataSetProcessingService
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

        public async ValueTask<DataSet> AddDataSetAsync(DataSet dataSet) =>
            await this.dataSetService.AddDataSetAsync(dataSet);
    }
}
