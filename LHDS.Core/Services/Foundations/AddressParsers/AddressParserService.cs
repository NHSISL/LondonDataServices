// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;

namespace LHDS.Core.Services.Foundations.AddressParsers
{
    public class AddressParserService : IAddressParserService
    {
        private readonly ILoggingBroker loggingBroker;

        public AddressParserService(ILoggingBroker loggingBroker)
        {
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<List<Address>> ProcessCsvAsync(byte[] data) =>
            throw new NotImplementedException();
    }
}
