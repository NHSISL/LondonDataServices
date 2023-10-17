// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Foundations.AddressNormalisations;
using LHDS.Core.Services.Processings.AddressNormalisations;
using Moq;
using Tynamix.ObjectFiller;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressNormalisations
{
    public partial class AddressNormalisationProcessingServiceTests
    {
        private readonly Mock<IAddressNormalisationService> addressNormalisationServiceMock =
            new Mock<IAddressNormalisationService>();

        private readonly Mock<ILoggingBroker> loggingBrokerMock = new Mock<ILoggingBroker>();
        private readonly IAddressNormalisationProcessingService addressNormalisationProcessingService;

        public AddressNormalisationProcessingServiceTests()
        {
            this.addressNormalisationProcessingService = new AddressNormalisationProcessingService(
                this.addressNormalisationServiceMock.Object,
                this.loggingBrokerMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();
    }
}
