// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
                ValidateDataSet(dataSet);

                return await this.dataSetService.AddDataSetAsync(dataSet);
            });

        public IQueryable<DataSet> RetrieveAllDataSets() =>
            TryCatch(() => this.dataSetService.RetrieveAllDataSets());

        public ValueTask<DataSet> RetrieveDataSetByIdAsync(Guid dataSetId) =>
            TryCatch(async () =>
            {
                ValidateDataSetId(dataSetId);

                return await this.dataSetService.RetrieveDataSetByIdAsync(dataSetId);
            });

        public ValueTask<DataSet> RetrieveOrAddDataSetAsync(DataSet dataSet) =>
            TryCatch(async () =>
            {
                ValidateDataSet(dataSet);

                return await this.dataSetService.RetrieveDataSetByIdAsync(dataSet.Id) ??
                    await this.dataSetService.AddDataSetAsync(dataSet);
            });

        public ValueTask<DataSet> ModifyOrAddDataSetAsync(DataSet dataSet) =>
            TryCatch(async () =>
            {
                ValidateDataSet(dataSet);
                ValidateDataSetId(dataSet.Id);
                var maybeDataSet = await this.dataSetService.RetrieveDataSetByIdAsync(dataSet.Id);

                if (maybeDataSet != null)
                {
                    return await this.dataSetService.ModifyDataSetAsync(dataSet);
                }
                else
                {
                    return await this.dataSetService.AddDataSetAsync(dataSet);
                }
            });

        public ValueTask<DataSet> ModifyDataSetAsync(DataSet dataSet) =>
            TryCatch(async () =>
            {
                ValidateDataSet(dataSet);

                return await this.dataSetService.ModifyDataSetAsync(dataSet);
            });

        public ValueTask<DataSet> RemoveDataSetByIdAsync(Guid dataSetId) =>
            TryCatch(async () =>
            {
                ValidateDataSetId(dataSetId);

                return await this.dataSetService.RemoveDataSetByIdAsync(dataSetId);
            });
    }
}