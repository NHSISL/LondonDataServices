// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Brokers.DateTimes;
using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Brokers.Storages.Sql;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;
using LHDS.ConfigImportExportTool.Services.Foundations.DataSets;

namespace LHDS.Core.Services.Foundations.DataSets
{
    internal partial class DataSetService : IDataSetService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public DataSetService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<DataSet> AddDataSetAsync(DataSet dataSet) =>
            TryCatch(async () =>
            {
                await ValidateDataSetOnAddAsync(dataSet);

                return await this.storageBroker.InsertDataSetAsync(dataSet);
            });

        public ValueTask<IQueryable<DataSet>> RetrieveAllDataSetsAsync() =>
            TryCatch(async() => await this.storageBroker.SelectAllDataSetsAsync());

        public ValueTask<DataSet> RetrieveDataSetByIdAsync(Guid dataSetId) =>
            TryCatch(async () =>
            {
                ValidateDataSetId(dataSetId);

                DataSet maybeDataSet = await this.storageBroker
                    .SelectDataSetByIdAsync(dataSetId);

                ValidateStorageDataSet(maybeDataSet, dataSetId);

                return maybeDataSet;
            });

        public ValueTask<DataSet> ModifyDataSetAsync(DataSet dataSet) =>
            TryCatch(async () =>
            {
                await ValidateDataSetOnModifyAsync(dataSet);

                DataSet maybeDataSet =
                    await this.storageBroker.SelectDataSetByIdAsync(dataSet.Id);

                ValidateStorageDataSet(maybeDataSet, dataSet.Id);
                ValidateAgainstStorageDataSetOnModify(inputDataSet: dataSet, storageDataSet: maybeDataSet);

                return await this.storageBroker.UpdateDataSetAsync(dataSet);
            });

        public ValueTask<DataSet> RemoveDataSetByIdAsync(Guid dataSetId) =>
            TryCatch(async () =>
            {
                ValidateDataSetId(dataSetId);

                DataSet maybeDataSet = await this.storageBroker
                    .SelectDataSetByIdAsync(dataSetId);

                ValidateStorageDataSet(maybeDataSet, dataSetId);

                return await this.storageBroker.DeleteDataSetAsync(maybeDataSet);
            });
    }
}