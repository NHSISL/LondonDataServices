// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Brokers.CsvMappers;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddressParsers.Exceptions;

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

        public ValueTask<List<ResolvedAddress>> ProcessCsvAsync(byte[] data, string filename) =>
            TryCatch(async () =>
            {
                ValidateResolvedAddressParserOnProcessCSV(data, filename);
                string stringData = Encoding.UTF8.GetString(data);
                bool hasHeaderRecord = true;
                List<ResolvedAddress> returnedResolvedAddresses = new List<ResolvedAddress>();
                var exceptions = new List<Exception>();

                List<string[]> records =
                    await this.csvMapperBroker.MapCsvToListArrayAsync(data: stringData, hasHeaderRecord);

                for (int i = 0; i < records.Count; i++)
                {
                    try
                    {
                        string[] record = records[i];
                        ResolvedAddress address = new ResolvedAddress
                        {
                            Id = this.identifierBroker.GetIdentifier(),
                            UniqueReference = Guid.Parse(record[0]),
                            PostCode = record[1],
                            UnstructuredPostalAddress = record[2],
                        };

                        returnedResolvedAddresses.Add(address);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(new InvalidCsvItemResolvedAddressParserException(
                            $"Error processing record at row {i + 1}: {ex.Message}"));
                    }
                }

                if (exceptions.Any())
                {
                    AggregateException aggregateException = new AggregateException(
                        $"Unable to process {exceptions.Count} records from {filename}",
                        exceptions);

                    throw new FailedToParseResolvedAddressParserException(
                        message: $"Unable to fully parse resolved addresses from file {filename}",
                        innerException: aggregateException);
                }

                return returnedResolvedAddresses;
            });

        public ValueTask<List<ResolvedAddress>> ProcessCsvAsync(string data, string filename) =>
           TryCatch(async () =>
           {
               ValidateResolvedAddressParserOnProcessCSV(data, filename);
               bool hasHeaderRecord = true;
               List<ResolvedAddress> returnedResolvedAddresses = new List<ResolvedAddress>();
               List<string[]> records = await this.csvMapperBroker.MapCsvToListArrayAsync(data, hasHeaderRecord);
               var exceptions = new List<Exception>();

               for (int i = 0; i < records.Count; i++)
               {
                   try
                   {
                       string[] record = records[i];
                       ResolvedAddress address = new ResolvedAddress
                       {
                           Id = this.identifierBroker.GetIdentifier(),
                           UniqueReference = Guid.Parse(record[0]),
                           PostCode = record[1],
                           UnstructuredPostalAddress = record[2],
                       };

                       returnedResolvedAddresses.Add(address);
                   }
                   catch (Exception ex)
                   {
                       exceptions.Add(new InvalidCsvItemResolvedAddressParserException(
                           $"Error processing record at row {i + 1}: {ex.Message}"));
                   }
               }

               if (exceptions.Any())
               {
                   AggregateException aggregateException = new AggregateException(
                       $"Unable to process {exceptions.Count} records from {filename}",
                       exceptions);

                   throw new FailedToParseResolvedAddressParserException(
                       message: $"Unable to fully parse resolved addresses from file {filename}",
                       innerException: aggregateException);
               }

               return returnedResolvedAddresses;
           });
    }
}