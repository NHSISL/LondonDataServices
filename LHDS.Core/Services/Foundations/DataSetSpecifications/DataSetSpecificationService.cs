// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.DataSetSpecifications;

namespace LHDS.Core.Services.Foundations.DataSetSpecifications
{
    public partial class DataSetSpecificationService : IDataSetSpecificationService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public DataSetSpecificationService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<DataSetSpecification> AddDataSetSpecificationAsync(DataSetSpecification dataSetSpecification) =>
            TryCatch(async () =>
            {
                await ValidateDataSetSpecificationOnAddAsync(dataSetSpecification);

                return await this.storageBroker.InsertDataSetSpecificationAsync(dataSetSpecification);
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

        public ValueTask<DataSetSpecification> ModifyDataSetSpecificationAsync(DataSetSpecification dataSetSpecification) =>
            TryCatch(async () =>
            {
                await ValidateDataSetSpecificationOnModifyAsync(dataSetSpecification);

                DataSetSpecification maybeDataSetSpecification =
                    await this.storageBroker.SelectDataSetSpecificationByIdAsync(dataSetSpecification.Id);

                ValidateStorageDataSetSpecification(maybeDataSetSpecification, dataSetSpecification.Id);
                ValidateAgainstStorageDataSetSpecificationOnModify(inputDataSetSpecification: dataSetSpecification, storageDataSetSpecification: maybeDataSetSpecification);

                return await this.storageBroker.UpdateDataSetSpecificationAsync(dataSetSpecification);
            });

        public ValueTask<DataSetSpecification> RemoveDataSetSpecificationByIdAsync(Guid dataSetSpecificationId) =>
            TryCatch(async () =>
            {
                ValidateDataSetSpecificationId(dataSetSpecificationId);

                DataSetSpecification maybeDataSetSpecification = await this.storageBroker
                    .SelectDataSetSpecificationByIdAsync(dataSetSpecificationId);

                ValidateStorageDataSetSpecification(maybeDataSetSpecification, dataSetSpecificationId);

                return await this.storageBroker.DeleteDataSetSpecificationAsync(maybeDataSetSpecification);
            });
    }
}