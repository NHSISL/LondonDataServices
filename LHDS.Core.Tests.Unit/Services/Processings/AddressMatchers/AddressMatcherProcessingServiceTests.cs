// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq.Expressions;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Services.Processings.AddressMatchers;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

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
                " A , A    A , A",
                "A, A A, A ",
                "  A  ,  A   A  ,  A  "
            };
        }

        private static Random random = new Random();

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static string GetRandomSeparator()
        {
            string[] separators = { " ", "  ", "   ", ",", ", ", " ,", " , " };
            return separators[random.Next(separators.Length)];
        }

        public static string GetRandomSpacedString(int wordCount)
        {
            string result = string.Empty;
            result += new string(' ', GetRandomNumber());

            for (int i = 0; i < wordCount; i++)
            {
                result += GetRandomString();
                if (i < wordCount - 1)
                {
                    result += GetRandomSeparator();
                }
            }

            result += new string(' ', GetRandomNumber());

            return result;
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
           actualException => actualException.SameExceptionAs(expectedException);
    }
}
