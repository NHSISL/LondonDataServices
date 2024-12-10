// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Brokers.CsvHelpers;
using LHDS.ConfigImportExportTool.Brokers.Loggings;

namespace LHDS.ConfigImportExportTool.Services.Foundations.CsvHelpers
{
    internal partial class CsvHelperService : ICsvHelperService
    {
        private readonly ICsvHelperBroker csvHelperBroker;
        private readonly ILoggingBroker loggingBroker;

        public CsvHelperService(
            ICsvHelperBroker csvHelperBroker,
            ILoggingBroker loggingBroker)
        {
            this.csvHelperBroker = csvHelperBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<List<T>> MapCsvToObjectAsync<T>(
        string data,
        bool hasHeaderRecord,
        Dictionary<string, int> fieldMappings = null) =>
            TryCatch(async () =>
            {
                ValidateMapCsvToObjectArguments(data);

                return await this.csvHelperBroker.MapCsvToObjectAsync<T>(data, hasHeaderRecord, fieldMappings);
            });

        public ValueTask<string> MapObjectToCsvAsync<T>(
            List<T> @object,
            bool addHeaderRecord,
            Dictionary<string, int> fieldMappings = null,
            bool? shouldAddTrailingComma = false) =>
                TryCatch(async () =>
                {
                    ValidateObjectListIsNotNull<T>(@object);

                    return await this.csvHelperBroker.MapObjectToCsvAsync<T>(
                        @object,
                        addHeaderRecord,
                        fieldMappings,
                        shouldAddTrailingComma);
                });
    }
}
