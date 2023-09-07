using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.SpecificationObjects;

namespace LHDS.Core.Services.Foundations.DataSetObjects
{
    public partial class DataSetObjectService : IDataSetObjectService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public DataSetObjectService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<SpecificationObject> AddDataSetObjectAsync(SpecificationObject dataSetObject) =>
            TryCatch(async () =>
            {
                ValidateDataSetObjectOnAdd(dataSetObject);

                return await this.storageBroker.InsertDataSetObjectAsync(dataSetObject);
            });

        public IQueryable<SpecificationObject> RetrieveAllDataSetObjects() =>
            TryCatch(() => this.storageBroker.SelectAllDataSetObjects());

        public ValueTask<SpecificationObject> RetrieveDataSetObjectByIdAsync(Guid dataSetObjectId) =>
            TryCatch(async () =>
            {
                ValidateDataSetObjectId(dataSetObjectId);

                SpecificationObject maybeDataSetObject = await this.storageBroker
                    .SelectDataSetObjectByIdAsync(dataSetObjectId);

                ValidateStorageDataSetObject(maybeDataSetObject, dataSetObjectId);

                return maybeDataSetObject;
            });

        public ValueTask<SpecificationObject> ModifyDataSetObjectAsync(SpecificationObject dataSetObject) =>
            TryCatch(async () =>
            {
                ValidateDataSetObjectOnModify(dataSetObject);

                SpecificationObject maybeDataSetObject =
                    await this.storageBroker.SelectDataSetObjectByIdAsync(dataSetObject.Id);

                ValidateStorageDataSetObject(maybeDataSetObject, dataSetObject.Id);
                ValidateAgainstStorageDataSetObjectOnModify(inputDataSetObject: dataSetObject, storageDataSetObject: maybeDataSetObject);

                return await this.storageBroker.UpdateDataSetObjectAsync(dataSetObject);
            });

        public ValueTask<SpecificationObject> RemoveDataSetObjectByIdAsync(Guid dataSetObjectId) =>
            TryCatch(async () =>
            {
                ValidateDataSetObjectId(dataSetObjectId);

                SpecificationObject maybeDataSetObject = await this.storageBroker
                    .SelectDataSetObjectByIdAsync(dataSetObjectId);

                ValidateStorageDataSetObject(maybeDataSetObject, dataSetObjectId);

                return await this.storageBroker.DeleteDataSetObjectAsync(maybeDataSetObject);
            });
    }
}