// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Brokers.DateTimes;
using LHDS.ConfigImportExportTool.Brokers.Loggings;
using LHDS.ConfigImportExportTool.Brokers.Storages.Sql;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;
using LHDS.ConfigImportExportTool.Services.Foundations.DataSets;

namespace LHDS.ConfigImportExportTool.Services.Foundations.DataSets
{
    internal partial class DataSetService : IDataSetService
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

        public ValueTask<IQueryable<DataSet>> RetrieveAllDataSetsAsync() =>
            TryCatch(async() => await this.storageBroker.SelectAllDataSetsAsync());
    }
}