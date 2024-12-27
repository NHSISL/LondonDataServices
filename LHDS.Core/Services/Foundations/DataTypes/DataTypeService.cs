// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.DataTypes;

namespace LHDS.Core.Services.Foundations.DataTypes
{
    public partial class DataTypeService : IDataTypeService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public DataTypeService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<DataType> AddDataTypeAsync(DataType dataType) =>
            TryCatch(async () =>
            {
                await ValidateDataTypeOnAddAsync(dataType);

                return await this.storageBroker.InsertDataTypeAsync(dataType);
            });

        public IQueryable<DataType> RetrieveAllDataTypes() =>
            TryCatch(() => this.storageBroker.SelectAllDataTypes());

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
                ValidateDataTypeOnModify(dataType);

                DataType maybeDataType =
                    await this.storageBroker.SelectDataTypeByIdAsync(dataType.Id);

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
    }
}