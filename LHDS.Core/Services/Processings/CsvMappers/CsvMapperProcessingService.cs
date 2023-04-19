// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.Core.Services.Processings.CsvMappers
{
    internal class CsvMapperProcessingService : ICsvMapperProcessingService
    {
        public ValueTask<List<T>> MapCsvToObjectAsync<T>(string data, bool hasHeaderRecord) =>
            throw new NotImplementedException();

        public async ValueTask<string> MapObjectToCsvAsync<T>(List<T> @object, bool addHeaderRecord) =>
            throw new NotImplementedException();
    }
}
