// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Processings.AddressMatchers;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
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
        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        public static TheoryData<string, string> CleanedAddressTestData()
        {
            return new TheoryData<string, string>
                {
                    {"123 Christo Street, London, W1A 1AA, United Kingdom", "W1A 1AA"},
                    {"Manchester, M2 3YZ, 456 Park Avenue, UK", "M2 3YZ"},
                    {"789 David Road, Birmingham, B12 9XY, England", "B12 9XY"},
                    {"G1 1AB, 101 Oak Lane, Glasgow, Scotland", "G1 1AB"},
                    {"CF10 2XY, 202 Birch Street, Cardiff, Wales", "CF10 2XY"},
                    {"BT1 2YZ, 303 Pine Road, Belfast, Northern Ireland", "BT1 2YZ"},
                    {"123 Main Street, London, W1A 1AA, W2 2BB, United Kingdom", "W1A 1AA"},
                    {"M1 1AA, M2 2BB, M3 3CC, Manchester, UK", "M1 1AA"},
                };
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
          actualException => actualException.SameExceptionAs(expectedException);
    }
}

