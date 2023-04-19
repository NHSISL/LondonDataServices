// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.CsvMappers;
using LHDS.Core.Brokers.Loggings;

namespace LHDS.Core.Services.Processings.CsvMappers
{
    public class CsvMapperProcessingService : ICsvMapperProcessingService
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

        public async ValueTask<List<T>> MapCsvToObjectAsync<T>(string data, bool hasHeaderRecord) =>
            await this.csvMapperService.MapCsvToObjectAsync<T>(data, hasHeaderRecord);

        public ValueTask<string> MapObjectToCsvAsync<T>(List<T> @object, bool addHeaderRecord) =>
            throw new NotImplementedException();
    }
}
