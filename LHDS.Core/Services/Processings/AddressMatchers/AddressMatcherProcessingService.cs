// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using LHDS.Core.Brokers.Loggings;

namespace LHDS.Core.Services.Processings.AddressMatchers
{
    internal class AddressMatcherProcessingService : IAddressMatcher
    {
        private readonly ILoggingBroker loggingBroker;

        public AddressMatcherProcessingService(ILoggingBroker loggingBroker)
        {
            this.loggingBroker = loggingBroker;
        }

        public string CleanAddress(string address) =>
            throw new System.NotImplementedException();

        public string ExtractPostCode(string address) =>
            throw new System.NotImplementedException();
    }
}
