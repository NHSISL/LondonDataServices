// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NHSISL.CsvHelperClient.Clients;

namespace LHDS.ConfigImportExportTool.Brokers.CsvHelpers
{
    public class CsvHelperBroker : ICsvHelperBroker
    {
        private readonly ICsvClient csvClient;

        public CsvHelperBroker()
        {
            csvClient = new CsvClient();
        }

        public async ValueTask<List<T>> MapCsvToObjectAsync<T>(
            string data,
            bool hasHeaderRecord,
            Dictionary<string, int> fieldMappings = null)
        {
            using Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(data ?? string.Empty));
            var results = new List<T>();

            await foreach (var item in csvClient
                .MapCsvToObjectAsync<T>(stream, hasHeaderRecord, fieldMappings))
            {
                results.Add(item);
            }

            return results;
        }

        public async ValueTask<string> MapObjectToCsvAsync<T>(
            List<T> @object,
            bool addHeaderRecord,
            Dictionary<string, int> fieldMappings = null,
            bool? shouldAddTrailingComma = false)
        {
            using MemoryStream stream = new MemoryStream();

            await csvClient
                .MapObjectToCsvAsync(@object, stream, addHeaderRecord, fieldMappings, shouldAddTrailingComma);

            return Encoding.UTF8.GetString(stream.ToArray());
        }
    }
}
