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
using LHDS.Core.Models.Foundations.DataTypes;

namespace LHDS.Core.Services.Foundations.DataTypes
{
    public partial class DataTypeService : IDataTypeService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ISecurityBroker securityBroker;
        private readonly ILoggingBroker loggingBroker;

        public DataTypeService(
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

        public ValueTask<DataType> AddDataTypeAsync(DataType dataType) =>
            TryCatch(async () =>
            {
                DataType dataTypeWithAddAuditApplied = await ApplyAddAuditAsync(dataType);
                await ValidateDataTypeOnAddAsync(dataTypeWithAddAuditApplied);

                return await this.storageBroker.InsertDataTypeAsync(dataType);
            });

        public ValueTask<IQueryable<DataType>> RetrieveAllDataTypesAsync() =>
            TryCatch(async() => await this.storageBroker.SelectAllDataTypesAsync());

        public ValueTask<DataType> RetrieveDataTypeByIdAsync(Guid dataTypeId) =>
            TryCatch(async () =>
            {
                ValidateDataTypeId(dataTypeId);

                DataType maybeDataType = await this.storageBroker
                    .SelectDataTypeByIdAsync(dataTypeId);

                ValidateStorageDataType(maybeDataType, dataTypeId);

                return maybeDataType;
            });

        public ValueTask<DataType> ModifyDataTypeAsync(DataType dataType) =>
            TryCatch(async () =>
            {
                DataType dataTypeWithModifyAuditApplied = await ApplyModifyAuditAsync(dataType);
                await ValidateDataTypeOnModifyAsync(dataTypeWithModifyAuditApplied);
                DataType maybeDataType = await this.storageBroker.SelectDataTypeByIdAsync(dataType.Id);
                ValidateStorageDataType(maybeDataType, dataType.Id);
                ValidateAgainstStorageDataTypeOnModify(inputDataType: dataType, storageDataType: maybeDataType);

                return await this.storageBroker.UpdateDataTypeAsync(dataType);
            });

        public ValueTask<DataType> RemoveDataTypeByIdAsync(Guid dataTypeId) =>
            TryCatch(async () =>
            {
                ValidateDataTypeId(dataTypeId);

                DataType maybeDataType = await this.storageBroker
                    .SelectDataTypeByIdAsync(dataTypeId);

                ValidateStorageDataType(maybeDataType, dataTypeId);

                return await this.storageBroker.DeleteDataTypeAsync(maybeDataType);
            });

        virtual internal async ValueTask<DataType> ApplyAddAuditAsync(DataType dataType)
        {
            ValidateDataTypeIsNotNull(dataType);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            dataType.CreatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            dataType.CreatedDate = auditDateTimeOffset;
            dataType.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            dataType.UpdatedDate = auditDateTimeOffset;

            return dataType;
        }

        virtual internal async ValueTask<DataType> ApplyModifyAuditAsync(DataType dataType)
        {
            ValidateDataTypeIsNotNull(dataType);
            var auditDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            var auditUser = await this.securityBroker.GetCurrentUserAsync();
            dataType.UpdatedBy = auditUser?.EntraUserId.ToString() ?? string.Empty;
            dataType.UpdatedDate = auditDateTimeOffset;

            return dataType;
        }
    }
}