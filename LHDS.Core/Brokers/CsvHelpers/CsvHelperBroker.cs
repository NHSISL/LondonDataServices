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
        private readonly ICsvHelperClient csvHelperClient;

        public CsvHelperBroker(ICsvHelperClient csvHelperClient)
        {
            this.csvHelperClient = csvHelperClient;
        }

        public async ValueTask<List<T>> MapCsvToObjectAsync<T>(
            string data,
            bool hasHeaderRecord,
            Dictionary<string, int>? fieldMappings = null)
        {
            return await this.csvHelperClient
                .MapCsvToObjectAsync<T>(data, hasHeaderRecord, fieldMappings);
        }

        public async ValueTask<string> MapObjectToCsvAsync<T>(
            List<T> @object,
            bool addHeaderRecord,
            Dictionary<string, int>? fieldMappings = null,
            bool? shouldAddTrailingComma = false)
        {
            return await this.csvHelperClient
                .MapObjectToCsvAsync(@object, addHeaderRecord, fieldMappings, shouldAddTrailingComma);
        }
    }
}
