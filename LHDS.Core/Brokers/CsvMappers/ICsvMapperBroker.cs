// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace LHDS.Core.Brokers.CsvMappers
{
    public interface ICsvMapperBroker
    {
        ValueTask<List<T>> MapCsvDataToObjectAsync<T>(string data, bool hasHeaderRecord);
        ValueTask<string> MapObjectToCsvDataAsync<T>(List<T> @object, bool addHeaderRecord);
    }
}
