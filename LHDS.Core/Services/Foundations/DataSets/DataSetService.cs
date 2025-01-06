// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.DataSets;

namespace LHDS.Core.Services.Foundations.DataSets
{
    public partial class DataSetService : IDataSetService
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

        public IQueryable<DataSet> RetrieveAllDataSets() =>
            TryCatch(() => this.storageBroker.SelectAllDataSets());

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