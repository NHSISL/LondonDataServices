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
using LHDS.Core.Models.Foundations.DataSetSpecifications;

namespace LHDS.Core.Services.Foundations.DataSetSpecifications
{
    public partial class DataSetSpecificationService : IDataSetSpecificationService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly ISecurityAuditBroker securityAuditBroker;

        public DataSetSpecificationService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker,
            ISecurityAuditBroker securityAuditBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
            this.securityAuditBroker = securityAuditBroker;
        }

        public ValueTask<DataSetSpecification> AddDataSetSpecificationAsync(
            DataSetSpecification dataSetSpecification) =>
            TryCatch(async () =>
            {
                DataSetSpecification dataSetSpecificationWithAuditApplied =
                    await this.securityAuditBroker.ApplyAddAuditValuesAsync(dataSetSpecification);

                await ValidateDataSetSpecificationOnAddAsync(dataSetSpecificationWithAuditApplied);

                return await this.storageBroker.InsertDataSetSpecificationAsync(dataSetSpecificationWithAuditApplied);
            });

        public ValueTask<IQueryable<DataSetSpecification>> RetrieveAllDataSetSpecificationsAsync() =>
            TryCatch(async () => await this.storageBroker.SelectAllDataSetSpecificationsAsync());

        public ValueTask<DataSetSpecification> RetrieveDataSetSpecificationByIdAsync(Guid dataSetSpecificationId) =>
            TryCatch(async () =>
            {
                ValidateDataSetSpecificationId(dataSetSpecificationId);

                DataSetSpecification maybeDataSetSpecification = await this.storageBroker
                    .SelectDataSetSpecificationByIdAsync(dataSetSpecificationId);

                ValidateStorageDataSetSpecification(maybeDataSetSpecification, dataSetSpecificationId);

                return maybeDataSetSpecification;
            });

        public ValueTask<DataSetSpecification> ModifyDataSetSpecificationAsync(
            DataSetSpecification dataSetSpecification) =>
            TryCatch(async () =>
            {
                DataSetSpecification dataSetSpecificationWithAuditApplied =
                    await this.securityAuditBroker.ApplyModifyAuditValuesAsync(dataSetSpecification);

                await ValidateDataSetSpecificationOnModifyAsync(dataSetSpecificationWithAuditApplied);

                DataSetSpecification maybeStorageDataSet =
                    await this.storageBroker.SelectDataSetSpecificationByIdAsync(
                        dataSetSpecificationWithAuditApplied.Id);

                ValidateStorageDataSetSpecification(maybeStorageDataSet, dataSetSpecificationWithAuditApplied.Id);

                ValidateAgainstStorageDataSetSpecificationOnModify(
                    inputDataSetSpecification: dataSetSpecificationWithAuditApplied,
                    storageDataSetSpecification: maybeStorageDataSet);

                return await this.storageBroker.UpdateDataSetSpecificationAsync(dataSetSpecificationWithAuditApplied);
            });

        public ValueTask<DataSetSpecification> RemoveDataSetSpecificationByIdAsync(Guid dataSetSpecificationId) =>
            TryCatch(async () =>
            {
                ValidateDataSetSpecificationId(dataSetSpecificationId);

                DataSetSpecification storageDataSetSpecification =
                    await this.storageBroker.SelectDataSetSpecificationByIdAsync(dataSetSpecificationId);

                ValidateStorageDataSetSpecification(storageDataSetSpecification, dataSetSpecificationId);

                DataSetSpecification dataSetSpecificationWithAuditApplied =
                    await this.securityAuditBroker.ApplyRemoveAuditValuesAsync(storageDataSetSpecification);

                await ValidateDataSetSpecificationOnDeleteAsync(dataSetSpecificationWithAuditApplied);

                return await this.storageBroker.DeleteDataSetSpecificationAsync(dataSetSpecificationWithAuditApplied);
            });
    }
}