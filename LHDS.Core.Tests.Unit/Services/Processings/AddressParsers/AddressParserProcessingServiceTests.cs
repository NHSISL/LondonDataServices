// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq.Expressions;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Foundations.AddressParsers;
using LHDS.Core.Services.Processings.AddressParsers;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressParsers
{
    public partial class AddressParserProcessingServiceTests
    {
        private readonly Mock<IAddressParserService> addressParserServiceMock =
            new Mock<IAddressParserService>();

        private readonly IAddressParserProcessingService addressParserProcessingService;
        private readonly Mock<ILoggingBroker> loggingBrokerMock = new Mock<ILoggingBroker>();

        public AddressParserProcessingServiceTests()
        {
            this.addressParserProcessingService = new AddressParserProcessingService(
                this.addressParserServiceMock.Object,
                this.loggingBrokerMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);
    }
}
