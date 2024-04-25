// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.Core.Brokers.CsvMappers
{
    public interface ICsvMapperBroker
    {
        ValueTask<List<string[]>> MapCsvToListArrayAsync(string data, bool hasHeaderRecord);
        ValueTask<List<T>> MapCsvToObjectAsync<T>(string data, bool hasHeaderRecord);

        ValueTask<List<T>> MapCsvToObjectAsync<T>(
            string data,
            Dictionary<string, int> fieldMappings,
            bool hasHeaderRecord);

        ValueTask<string> MapObjectToCsvAsync<T>(
            List<T> @object,
            bool addHeaderRecord,
            bool shouldAddTrailingComma);
    }
}
