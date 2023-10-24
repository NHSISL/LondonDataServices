// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;

namespace LHDS.Core.Services.Foundations.AddressParsers
{
    public partial class AddressParserService : IAddressParserService
    {
        private readonly ILoggingBroker loggingBroker;

        public AddressParserService(
            ILoggingBroker loggingBroker)
        {
            this.loggingBroker = loggingBroker;
        }

        public Task<List<Address>> ProcessCSVAsync(byte[] data) =>
            TryCatch(async () =>
            {
                ValidateAddressParserOnProcessCSV(data);
                this.loggingBroker.LogInformation("Data validation complete.");
                string stringData = Encoding.UTF8.GetString(data);
                List<string> recods = stringData.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();
                List<Address> returnedAddresses = new List<Address>();

                foreach (string record in recods)
                {
                    if (record.StartsWith("28,"))
                    {
                        string[] index = record.Split(",");

                        Address address = new Address
                        {
                            Id = Guid.NewGuid(),
                            UPRN = index[3],
                            UPSN = index[4],
                            OrganisationName = index[5],
                            DepartmentName = index[6],
                            SubBuildingName = index[7],
                            BuildingName = index[8],
                            BuildingNumber = index[9],
                            DependentThoroughfare = index[10],
                            Thoroughfare = index[11],
                            DoubleDependentLocality = index[12],
                            DependentLocality = index[13],
                            PostTown = index[14],
                            PostCode = index[15],
                        };

                        returnedAddresses.Add(address);
                    }
                }

                return returnedAddresses;
            });
    }
}