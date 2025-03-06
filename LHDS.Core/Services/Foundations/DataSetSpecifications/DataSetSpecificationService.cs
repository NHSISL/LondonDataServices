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
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.DataSetSpecifications;

namespace LHDS.Core.Services.Foundations.DataSetSpecifications
{
    public partial class DataSetSpecificationService : IDataSetSpecificationService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly ISecurityBroker securityBroker;

        public DataSetSpecificationService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker,
            ISecurityBroker securityBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
            this.securityBroker = securityBroker;
        }

        public ValueTask<DataSetSpecification> AddDataSetSpecificationAsync(
            DataSetSpecification dataSetSpecification) =>
            TryCatch(async () =>
            {
                DataSetSpecification dataSetSpecificationWithAuditApplied = 
                    await ApplyAddAuditAsync(dataSetSpecification);
                    await ValidateDataSetSpecificationOnAddAsync(dataSetSpecificationWithAuditApplied);

                return await this.storageBroker.InsertDataSetSpecificationAsync(dataSetSpecificationWithAuditApplied);
            });

        public ValueTask<IQueryable<DataSetSpecification>> RetrieveAllDataSetSpecificationsAsync() =>
            TryCatch(async() => await this.storageBroker.SelectAllDataSetSpecificationsAsync());

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
                    await ApplyModifyAuditAsync(dataSetSpecification);
                    await ValidateDataSetSpecificationOnModifyAsync(dataSetSpecificationWithAuditApplied);

                DataSetSpecification storageDataSet =
                    await this.storageBroker.SelectDataSetSpecificationByIdAsync(
                        dataSetSpecificationWithAuditApplied.Id);

                ValidateStorageDataSetSpecification(storageDataSet, dataSetSpecificationWithAuditApplied.Id);

                ValidateAgainstStorageDataSetSpecificationOnModify(
                    inputDataSetSpecification: dataSetSpecificationWithAuditApplied,
                    storageDataSetSpecification: storageDataSet);

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
                    await ApplyDeleteAuditAsync(storageDataSetSpecification);

                ValidateAgainstStorageDataSetSpecificationOnDelete(
                    dataSetSpecificationWithAuditApplied,
                    dataSetSpecificationWithAuditApplied.UpdatedBy);

                return await this.storageBroker.DeleteDataSetSpecificationAsync(dataSetSpecificationWithAuditApplied);
            });

        internal async ValueTask<DataSetSpecification> ApplyAddAuditAsync(
            DataSetSpecification dataSetSpecification)
                {
                    ValidateDataSetSpecificationIsNotNull(dataSetSpecification);

                    DateTimeOffset auditDate = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                    EntraUser auditUser = await this.securityBroker.GetCurrentUserAsync();

                    dataSetSpecification.CreatedBy = auditUser.EntraUserId;
                    dataSetSpecification.CreatedDate = auditDate;
                    dataSetSpecification.UpdatedBy = auditUser.EntraUserId;
                    dataSetSpecification.UpdatedDate = auditDate;

                    return dataSetSpecification;
                }

        internal async ValueTask<DataSetSpecification> ApplyModifyAuditAsync(
            DataSetSpecification dataSetSpecification)
                {
                    ValidateDataSetSpecificationIsNotNull(dataSetSpecification);

                    DateTimeOffset auditDate = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                    EntraUser auditUser = await this.securityBroker.GetCurrentUserAsync();

                    dataSetSpecification.UpdatedBy = auditUser.EntraUserId;
                    dataSetSpecification.UpdatedDate = auditDate;

                    return dataSetSpecification;
                }

        internal async ValueTask<DataSetSpecification> ApplyDeleteAuditAsync(
            DataSetSpecification dataSetSpecification)
                {
                    ValidateDataSetSpecificationIsNotNull(dataSetSpecification);

                    DateTimeOffset auditDate = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                    EntraUser auditUser = await this.securityBroker.GetCurrentUserAsync();

                    dataSetSpecification.UpdatedBy = auditUser.EntraUserId;
                    dataSetSpecification.UpdatedDate = auditDate;

                    return dataSetSpecification;
                }
    }
}