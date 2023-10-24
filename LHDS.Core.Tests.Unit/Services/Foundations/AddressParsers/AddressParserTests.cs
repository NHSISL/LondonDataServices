// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Foundations.Addresses;
using Moq;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressParsers
{
    public partial class AddressParserTests
    {
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly AddressParserService addressParserService;

        public AddressParserTests()
        {
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.addressParserService = new AddressParserService(
                loggingBroker: this.loggingBrokerMock.Object);
        }
    }
}
