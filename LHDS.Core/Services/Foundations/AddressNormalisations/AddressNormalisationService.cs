// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Brokers.AddressNormalisations;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.AddressNormalisations;

namespace LHDS.Core.Services.Foundations.AddressNormalisations
{
    public class AddressNormalisationService : IAddressNormalisationService
    {
        private readonly IAddressNormalisationBroker addressNormalisationBroker;
        private readonly ILoggingBroker loggingBroker;

        public AddressNormalisationService(
            IAddressNormalisationBroker addressNormalisationBroker,
            ILoggingBroker loggingBroker)
        {
            this.addressNormalisationBroker = addressNormalisationBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<AddressNormalisation> GetNormalisedAddress(string address)
        {
            throw new System.NotImplementedException();
        }
    }
}
