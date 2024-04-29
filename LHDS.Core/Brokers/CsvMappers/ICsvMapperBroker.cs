// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using CsvHelper;

namespace LHDS.Core.Brokers.CsvMappers
{
    public interface ICsvMapperBroker
    {
        CsvReader CreateCsvReader(StringReader reader, bool hasHeaderRecord);
        CsvWriter CreateCsvWriter(StringWriter writer, bool hasHeaderRecord);
        StringWriter CreateStringWriter();
    }
}
