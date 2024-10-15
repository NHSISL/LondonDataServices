// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.ConfigImportExportTool.Services.Foundations.CsvHelpers;
using NHSISL.CsvHelperClient.Clients;

namespace LHDS.ConfigImportExportTool.Brokers.CsvHelpers
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
                throw new NotImplementedException();

        public async ValueTask<string> MapObjectToCsvAsync<T>(
            List<T> @object,
            bool addHeaderRecord,
            Dictionary<string, int>? fieldMappings = null,
            bool? shouldAddTrailingComma = false) =>
                throw new NotImplementedException();
    }
}
