// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Processings.AddressMatchers;
using Moq;
using Tynamix.ObjectFiller;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressMatchers
{
    public partial class AddressMatcherProcessingServiceTests
    {
        private readonly Mock<ILoggingBroker> loggingBrokerMock = new Mock<ILoggingBroker>();
        private readonly IAddressMatcherProcessingService addressMatcherProcessingService;

        public AddressMatcherProcessingServiceTests()
        {
            addressMatcherProcessingService = new AddressMatcherProcessingService(
                loggingBroker: loggingBrokerMock.Object);
        }

        public static TheoryData UncleanedAddressString()
        {
            return new TheoryData<string>
            {
                "A,A A,A",
                "A, A A, A",
                "A, A  A, A",
                " A , A A , A",
                "A, A A, A "
            };
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();


    }
}
