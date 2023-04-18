// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace LHDS.Core.Brokers.CsvMappers
{
    public class CsvMapperBroker : ICsvMapperBroker
    {
        public async ValueTask<List<T>> MapCsvToObjectAsync<T>(string data, bool hasHeaderRecord)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = hasHeaderRecord,
            };

            using (var reader = new StreamReader(data))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<T>().ToList();
                return await ValueTask.FromResult(records);
            }
        }

        public async ValueTask<string> MapObjectToCsvAsync<T>(List<T> @object, bool addHeaderRecord)
        {
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                if (addHeaderRecord)
                {
                    csvWriter.WriteHeader<T>();
                    csvWriter.NextRecord();
                }

                csvWriter.WriteRecords(@object);

                byte[] csvData = memoryStream.ToArray();
                return await ValueTask.FromResult(Encoding.UTF8.GetString(csvData));
            }
        }
    }
}
