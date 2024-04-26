// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Foundations.CsvMappers;

namespace LHDS.Core.Brokers.CsvMappers
{
    public partial class CsvMapperService : ICsvMapperService
    {
        private readonly ICsvMapperBroker csvMapperBroker;
        private readonly ILoggingBroker loggingBroker;

        public CsvMapperService(ICsvMapperBroker csvMapperBroker, ILoggingBroker loggingBroker)
        {
            this.csvMapperBroker = csvMapperBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<List<T>> MapCsvToObjectAsync<T>(string data,
            bool hasHeaderRecord,
            Dictionary<string, int>? fieldMappings = null) =>
            TryCatch(async () =>
            {
                ValidateMapCsvToObjectArguments(data, hasHeaderRecord);

                using (var reader = new StringReader(data))
                using (var csv = this.csvMapperBroker.CreateCsvReader(reader, hasHeaderRecord))
                {
                    if (fieldMappings != null)
                    {
                        csv.Context.RegisterClassMap(new CustomMap<T>(fieldMappings));
                    }

                    var records = csv.GetRecords<T>().ToList();

                    return await ValueTask.FromResult(records);
                }
            });

        public ValueTask<string> MapObjectToCsvAsync<T>(
            List<T> @object,
            bool hasHeaderRecord,
            Dictionary<string, int>? fieldMappings = null,
            bool? shouldAddTrailingComma = false) =>
        TryCatch(async () =>
        {
            ValidateMapObjectToCsvArguments(@object, hasHeaderRecord);

            using (var writer = new StringWriter())
            using (var csvWriter = this.csvMapperBroker.CreateCsvWriter(writer, hasHeaderRecord))
            {
                if (hasHeaderRecord)
                {
                    csvWriter.WriteHeader<T>();
                    csvWriter.NextRecord();
                }

                foreach (var item in @object)
                {
                    csvWriter.WriteRecord(item);

                    if (shouldAddTrailingComma.HasValue && shouldAddTrailingComma.Value == true)
                    {
                        csvWriter.WriteField("");
                    }

                    csvWriter.NextRecord();
                }

                return await ValueTask.FromResult(writer.ToString());
            }
        });
    }
}
