// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.Core.Services.Processings.CsvMappers
{
    public interface ICsvMapperProcessingService
    {
        ValueTask<List<T>> MapCsvToObjectAsync<T>(string data, bool hasHeaderRecord);
        ValueTask<string> MapObjectToCsvAsync<T>(List<T> @object, bool addHeaderRecord, bool shouldAddTrailingComma);
    }
}
