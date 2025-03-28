// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using NHSISL.CsvHelperClient.Clients;

namespace LHDS.Core.Brokers.CsvHelpers
{
    public class CsvHelperBroker : ICsvHelperBroker
    {
        private readonly ICsvClient csvClient;

        public CsvHelperBroker()
        {
            this.csvClient = new CsvClient();
        }

        public async ValueTask<List<T>> MapCsvToObjectAsync<T>(
            string data,
            bool hasHeaderRecord,
            Dictionary<string, int>? fieldMappings = null,
            bool headerValidated = true)
        {
            return await this.csvClient
                .MapCsvToObjectAsync<T>(data, hasHeaderRecord, fieldMappings, headerValidated);
        }

        public async ValueTask<string> MapObjectToCsvAsync<T>(
            List<T> @object,
            bool addHeaderRecord,
            Dictionary<string, int>? fieldMappings = null,
            bool? shouldAddTrailingComma = false)
        {
            return await this.csvClient
                .MapObjectToCsvAsync(@object, addHeaderRecord, fieldMappings, shouldAddTrailingComma);
        }
    }
}
