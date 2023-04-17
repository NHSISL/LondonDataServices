// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.Core.Tests.Unit.Services.Processings.CsvMappers
{
    internal class CsvMapperProcessingService : ICsvMapperProcessingService
    {
        public ValueTask<List<T>> MapCsvDataToObjectAsync<T>(byte[] data) =>
            throw new NotImplementedException();

        public ValueTask<byte[]> MapObjectToCsvDataAsync<T>(List<T> @object) =>
            throw new NotImplementedException();
    }
}
