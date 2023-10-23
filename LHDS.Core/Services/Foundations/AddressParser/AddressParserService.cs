// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Services.Foundations.AddressParsers;
using Org.BouncyCastle.Utilities;

namespace LHDS.Core.Services.Foundations.Addresses
{
    public partial class AddressParserService : IAddressParserService
    {
        private readonly ILoggingBroker loggingBroker;

        public AddressParserService(
            ILoggingBroker loggingBroker)
        {
            this.loggingBroker = loggingBroker;
        }

        public List<Address> ProcessCSV(byte[] data)
        {
            string stringData = data.ToString();
            List<string> recods = stringData.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();
            List<Address> expectedAddresses = new List<Address>();

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
                        Thoroughfare = index[12],
                        DoubleDependentLocality = index[13],
                        DependentLocality = index[14],
                        PostTown = index[15],
                        PostCode = index[16],
                    };

                    expectedAddresses.Add(address);
                }
            }

            return expectedAddresses;
        }
    }
}