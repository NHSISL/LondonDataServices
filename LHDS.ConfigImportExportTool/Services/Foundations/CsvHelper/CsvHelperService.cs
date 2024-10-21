// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Brokers.CsvHelpers;
using LHDS.ConfigImportExportTool.Services.Foundations.CsvHelpers;

namespace LHDS.ConfigImportExportTool.Foundations.CsvHelpers
{
    public class CsvHelperService : ICsvHelperService
    {
        private readonly ICsvHelperBroker csvHelperBroker;

        public CsvHelperService(ICsvHelperBroker csvHelperBroker)
        {
            csvHelperBroker = new CsvHelperBroker();
        }

        public async ValueTask<List<T>> MapCsvToObjectAsync<T>(
            string data,
            bool hasHeaderRecord,
            Dictionary<string, int>? fieldMappings = null) =>
                await this.csvHelperBroker.MapCsvToObjectAsync<T>(data, hasHeaderRecord, fieldMappings);

        public async ValueTask<string> MapObjectToCsvAsync<T>(
            List<T> @object,
            bool addHeaderRecord,
            Dictionary<string, int>? fieldMappings = null,
            bool? shouldAddTrailingComma = false) =>
                throw new NotImplementedException();
    }
}
