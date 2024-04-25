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
using LHDS.Core.Models.Foundations.Addresses;

namespace LHDS.Core.Services.Foundations.AddressParsers
{
    public partial class AddressParserService : IAddressParserService
    {
        private readonly ICsvMapperBroker csvMapperBroker;
        private readonly IIdentifierBroker identifierBroker;
        private readonly ILoggingBroker loggingBroker;

        public AddressParserService(
            ICsvMapperBroker csvMapperBroker,
            IIdentifierBroker identifierBroker,
            ILoggingBroker loggingBroker)
        {
            this.csvMapperBroker = csvMapperBroker;
            this.identifierBroker = identifierBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<List<Address>> ProcessCsvAsync(byte[] data, string filename) =>
            TryCatch(async () =>
            {
                ValidateAddressParserOnProcessCSV(data, filename);
                string stringData = Encoding.UTF8.GetString(data);
                List<string> records = stringData.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

                List<string> filteredRecords = records.Where(record =>
                   record.StartsWith("28,") || record.StartsWith("\"28\",")).ToList();

                string stringRecords = string.Join(Environment.NewLine, filteredRecords);

                Dictionary<string, int> fieldMappings = new Dictionary<string, int>
                {
                    { "UPRN", 3 },
                    { "UPSN", 4 },
                    { "OrganisationName", 5 },
                    { "DepartmentName", 6 },
                    { "SubBuildingName", 7 },
                    { "BuildingName", 8 },
                    { "BuildingNumber", 9 },
                    { "DependentThoroughfare", 10 },
                    { "Thoroughfare", 11 },
                    { "DoubleDependentLocality", 12 },
                    { "DependentLocality", 13 },
                    { "PostTown", 14 },
                    { "PostCode", 15 }
                };

                List<Address> results = await this.csvMapperBroker.MapCsvToObjectAsync<Address>(
                    stringRecords,
                    fieldMappings,
                    hasHeaderRecord: false);

                return results;
            });

        public ValueTask<List<Address>> ProcessCsvAsync(string data, string filename) =>
            TryCatch(async () =>
            {
                ValidateAddressParserOnProcessCSV(data, filename);
                List<string> records = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

                List<string> filteredRecords = records.Where(record =>
                   record.StartsWith("28,") || record.StartsWith("\"28\",")).ToList();

                string stringRecords = string.Join(Environment.NewLine, filteredRecords);

                Dictionary<string, int> fieldMappings = new Dictionary<string, int>
                {
                    { "UPRN", 3 },
                    { "UPSN", 4 },
                    { "OrganisationName", 5 },
                    { "DepartmentName", 6 },
                    { "SubBuildingName", 7 },
                    { "BuildingName", 8 },
                    { "BuildingNumber", 9 },
                    { "DependentThoroughfare", 10 },
                    { "Thoroughfare", 11 },
                    { "DoubleDependentLocality", 12 },
                    { "DependentLocality", 13 },
                    { "PostTown", 14 },
                    { "PostCode", 15 }
                };

                List<Address> results = await this.csvMapperBroker.MapCsvToObjectAsync<Address>(
                    stringRecords,
                    fieldMappings,
                    hasHeaderRecord: false);

                return results;
            });
    }
}