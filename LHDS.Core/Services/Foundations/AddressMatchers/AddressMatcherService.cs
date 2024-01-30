// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.AddressMatchers;

namespace LHDS.Core.Services.Foundations.AddressMatchers
{
    public class AddressMatcherService : IAddressMatcherService
    {
        private readonly ILoggingBroker loggingBroker;

        public AddressMatcherService(ILoggingBroker loggingBroker)
        {
            this.loggingBroker = loggingBroker;
        }

        public BestMatchEnum CheckForBestMatch(HashSet<AddressMatch> macthedAddresses) =>
            throw new NotImplementedException();
    }
}
