// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
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

        public ValueTask<DataSet> AddDataSetAsync(DataSet dataSet) =>
            TryCatch(async () =>
            {
                ValidateDataSetOnAdd(dataSet);

                return await this.dataSetService.AddDataSetAsync(dataSet);
            });

        public IQueryable<DataSet> RetrieveAllDataSets() =>
            TryCatch(() => this.dataSetService.RetrieveAllDataSets());

        public async ValueTask<DataSet> RetrieveDataSetByIdAsync(Guid dataSetId) =>
            await this.dataSetService.RetrieveDataSetByIdAsync(dataSetId);
    }
}
