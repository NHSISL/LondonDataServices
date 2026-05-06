// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
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

        public async IAsyncEnumerable<T> MapCsvToObjectAsync<T>(
            Stream data,
            bool hasHeaderRecord,
            Dictionary<string, int> fieldMappings = null,
            bool? headerValidated = true,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var item in this.csvClient
                .MapCsvToObjectAsync<T>(data, hasHeaderRecord, fieldMappings, headerValidated, cancellationToken))
            {
                yield return item;
            }
        }

        public async ValueTask MapObjectToCsvAsync<T>(
            List<T> @object,
            Stream outputStream,
            bool addHeaderRecord,
            Dictionary<string, int> fieldMappings,
            bool? shouldAddTrailingComma,
            CancellationToken cancellationToken)
        {
            await this.csvClient
                .MapObjectToCsvAsync(
                @object,
                outputStream,
                addHeaderRecord,
                fieldMappings,
                shouldAddTrailingComma,
                cancellationToken);
        }
    }
}
