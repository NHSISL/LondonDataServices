// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.DataSets;

namespace LHDS.Core.Services.Foundations.DataSets
{
    public partial class DataSetService : IDataSetService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ISecurityBroker securityBroker;
        private readonly ILoggingBroker loggingBroker;

        public DataSetService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ISecurityBroker securityBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.securityBroker = securityBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<DataSet> AddDataSetAsync(DataSet dataSet) =>
            TryCatch(async () =>
            {
                DataSet dataSetWithAddAuditApplied = await ApplyAddAuditAsync(dataSet);
                await ValidateDataSetOnAddAsync(dataSetWithAddAuditApplied);

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

                DataSet maybeDataSet = await this.storageBroker.SelectDataSetByIdAsync(dataSetId);

                ValidateStorageDataSet(maybeDataSet, dataSetId);

                DataSet dataSetWithDeleteAuditApplied = await ApplyDeleteAuditAsync(maybeDataSet);

                DataSet updatedDataSet = 
                    await this.storageBroker.UpdateDataSetAsync(dataSetWithDeleteAuditApplied);

                await ValidateAgainstStorageDataSetOnDeleteAsync(
                    updatedDataSet, 
                    dataSetWithDeleteAuditApplied);

                return await this.storageBroker.DeleteDataSetAsync(updatedDataSet);
            });

        virtual internal async ValueTask<DataSet> ApplyAddAuditAsync(
            DataSet dataSet)
        {
            ValidateDataSetIsNotNull(dataSet);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            dataSet.CreatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            dataSet.CreatedDate = auditDateTimeOffset;
            dataSet.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            dataSet.UpdatedDate = auditDateTimeOffset;

            return dataSet;
        }
        virtual internal async ValueTask<DataSet> ApplyDeleteAuditAsync(DataSet dataSet)
        {
            ValidateDataSetIsNotNull(dataSet);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            dataSet.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            dataSet.UpdatedDate = auditDateTimeOffset;
            return dataSet;
        }
    }
}