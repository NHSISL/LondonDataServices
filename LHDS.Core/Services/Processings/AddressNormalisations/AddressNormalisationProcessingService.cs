// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Services.Foundations.AddressNormalisations;

namespace LHDS.Core.Services.Processings.AddressNormalisations
{
    public partial class AddressNormalisationProcessingService : IAddressNormalisationProcessingService
    {
        private readonly IAddressNormalisationService addressNormalisationService;
        private readonly ILoggingBroker loggingBroker;

        public AddressNormalisationProcessingService(
            IAddressNormalisationService addressNormalisationService,
            ILoggingBroker loggingBroker)
        {
            this.addressNormalisationService = addressNormalisationService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<AddressNormalisation> GetNormalisedAddress(string address) =>
            TryCatch(async () =>
            {
                ValidateAddressNormalisationArgs(address);

                AddressNormalisation normalisedAddress =
                    await this.addressNormalisationService.GetNormalisedAddress(address);

                return normalisedAddress;
            });
    }
}
