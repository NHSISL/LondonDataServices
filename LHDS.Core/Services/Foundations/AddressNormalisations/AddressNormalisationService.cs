// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Brokers.AddressNormalisations;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.AddressNormalisation;

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

        public async ValueTask<AddressNormalisation> GetNormalisedAddress(string address)
        {
            (string PostalAddress, string JsonPostalAddress) = 
                await this.addressNormalisationBroker.GetNormalisedAddress(address);

            AddressNormalisation normalisedAddress = new AddressNormalisation
                {
                    PostalAddress = PostalAddress,
                    JsonPostalAddress = JsonPostalAddress
                };

            return normalisedAddress;
        }
    }
}
