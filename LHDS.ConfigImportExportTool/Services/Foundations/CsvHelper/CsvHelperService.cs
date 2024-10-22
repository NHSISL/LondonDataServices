// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.ConfigImportExportTool.Brokers.CsvHelpers;

namespace LHDS.ConfigImportExportTool.Services.Foundations.CsvHelpers
{
    internal partial class CsvHelperService<T> : ICsvHelperService
    {
        private readonly ICsvHelperBroker csvHelperBroker;

        public CsvHelperService(ICsvHelperBroker csvHelperBroker)
        {
            this.csvHelperBroker = csvHelperBroker;
        }

        public ValueTask<List<T>> MapCsvToObjectAsync<T>(
        string data,
        bool hasHeaderRecord,
        Dictionary<string, int>? fieldMappings = null) =>
            TryCatch(async () =>
            {
                return await this.csvHelperBroker.MapCsvToObjectAsync<T>(data, hasHeaderRecord, fieldMappings);
            });

        public async ValueTask<string> MapObjectToCsvAsync<T>(
            List<T> @object,
            bool addHeaderRecord,
            Dictionary<string, int>? fieldMappings = null,
            bool? shouldAddTrailingComma = false) =>
                throw new NotImplementedException();
    }
}
