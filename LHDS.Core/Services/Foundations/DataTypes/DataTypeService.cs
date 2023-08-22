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
                ValidateDataTypeOnAdd(dataType);

                return await this.storageBroker.InsertDataTypeAsync(dataType);
            });

        public IQueryable<DataType> RetrieveAllDataTypes() =>
            this.storageBroker.SelectAllDataTypes();
    }
}