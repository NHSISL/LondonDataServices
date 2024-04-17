// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Brokers.CsvMappers;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.ResolvedAddresses;

namespace LHDS.Core.Services.Foundations.ResolvedAddressParsers
{
    public partial class ResolvedAddressParserService : IResolvedAddressParserService
    {
        private readonly ICsvMapperBroker csvMapperBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly ILoggingBroker loggingBroker;

        public ResolvedAddressParserService(
            ICsvMapperBroker csvMapperBroker,
            IIdentifierBroker identifierBroker,
            ILoggingBroker loggingBroker)
        {
            this.csvMapperBroker = csvMapperBroker;
            this.identifierBroker = identifierBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<List<ResolvedAddress>> ProcessCsvAsync(byte[] data) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddressParserOnProcessCSV(data);
                string stringData = Encoding.UTF8.GetString(data);

                return await this.ProcessCsvAsync(stringData);
            });

        public ValueTask<List<ResolvedAddress>> ProcessCsvAsync(string data) =>
           TryCatch(async () =>
           {
               ValidateResolvedAddressParserOnProcessCSV(data);
               bool hasHeaderRecord = true;
               List<ResolvedAddress> returnedResolvedAddresses = new List<ResolvedAddress>();
               List<string[]> records = await this.csvMapperBroker.MapCsvToListArrayAsync(data, hasHeaderRecord);

               foreach (string[] record in records)
               {
                   ResolvedAddress address = new ResolvedAddress
                   {
                       Id = this.identifierBroker.GetIdentifier(),
                       UniqueReference = Guid.Parse(record[0]),
                       PostCode = record[1],
                       UnstructuredPostalAddress = record[2],
                   };

                   returnedResolvedAddresses.Add(address);
               }

               return returnedResolvedAddresses;
           });
    }
}