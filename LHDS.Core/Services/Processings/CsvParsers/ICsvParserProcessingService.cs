// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.Core.Services.Processings.CsvParsers
{
    public interface ICsvParserProcessingService
    {
        ValueTask<List<T>> MapCsvToObjectAsync<T>(
            string data,
            bool hasHeaderRecord,
            Dictionary<string, int>? fieldMappings);

        ValueTask<string> MapObjectToCsvAsync<T>(
            List<T> @object,
            bool addHeaderRecord = true,
            Dictionary<string, int>? fieldMappings = null,
            bool? shouldAddTrailingComma = false);
    }
}