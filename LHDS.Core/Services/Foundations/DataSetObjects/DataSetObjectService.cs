using System;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Storages.Sql;
using LHDS.Core.Models.Foundations.DataSetObjects;

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

        public ValueTask<DataSetObject> AddDataSetObjectAsync(DataSetObject dataSetObject) =>
            TryCatch(async () =>
            {
                ValidateDataSetObjectOnAdd(dataSetObject);

                return await this.storageBroker.InsertDataSetObjectAsync(dataSetObject);
            });

        public IQueryable<DataSetObject> RetrieveAllDataSetObjects() =>
            TryCatch(() => this.storageBroker.SelectAllDataSetObjects());

        public ValueTask<DataSetObject> RetrieveDataSetObjectByIdAsync(Guid dataSetObjectId) =>
            TryCatch(async () =>
            {
                ValidateDataSetObjectId(dataSetObjectId);

                DataSetObject maybeDataSetObject = await this.storageBroker
                    .SelectDataSetObjectByIdAsync(dataSetObjectId);

                return maybeDataSetObject;
            });
    }
}