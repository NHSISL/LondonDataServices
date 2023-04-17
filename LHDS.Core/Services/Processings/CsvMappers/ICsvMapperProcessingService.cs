// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.Core.Tests.Unit.Services.Processings.CsvMappers
{
    public interface ICsvMapperProcessingService
    {
        ValueTask<List<T>> MapCsvDataToObjectAsync<T>(byte[] data);
        ValueTask<byte[]> MapObjectToCsvDataAsync<T>(List<T> @object);
    }
}
