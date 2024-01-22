// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Foundations.CsvMappers;

namespace LHDS.Core.Services.Processings.CsvMappers
{
    public partial class CsvMapperProcessingService : ICsvMapperProcessingService
    {
        private readonly ICsvMapperService csvMapperService;
        private readonly ILoggingBroker loggingBroker;

        public CsvMapperProcessingService(
            ICsvMapperService csvMapperService,
            ILoggingBroker loggingBroker)
        {
            this.csvMapperService = csvMapperService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<List<T>> MapCsvToObjectAsync<T>(string data, bool hasHeaderRecord) =>
            TryCatch(async () =>
            {
                ValidateMapCsvToObjectArguments(data, hasHeaderRecord);

                return await this.csvMapperService.MapCsvToObjectAsync<T>(data, hasHeaderRecord);
            });

        public ValueTask<string> MapObjectToCsvAsync<T>(
            List<T> @object,
            bool addHeaderRecord,
            bool shouldAddTrailingComma) =>
            TryCatch(async () =>
            {
                ValidateMapObjectToCsvArguments(@object, addHeaderRecord);

                return await this.csvMapperService
                    .MapObjectToCsvAsync(@object, addHeaderRecord, shouldAddTrailingComma);
            });
    }
}
