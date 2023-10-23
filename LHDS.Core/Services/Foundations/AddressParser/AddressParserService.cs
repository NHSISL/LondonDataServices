// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Services.Foundations.AddressParsers;

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

        public List<Address> ProcessCSV(byte[] data) =>
            throw new NotImplementedException();
    }
}