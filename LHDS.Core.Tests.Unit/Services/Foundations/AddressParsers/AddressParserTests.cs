// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq.Expressions;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Foundations.AddressParsers;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

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

        private static string GetRandomString() =>
          new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
           new IntRange(min: 2, max: 10).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);
    }
}
