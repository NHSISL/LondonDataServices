// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq.Expressions;
using System.Text;
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

        public static string GetRandomCleanAddressString(int wordCount)
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < wordCount; i++)
            {
                stringBuilder.Append(GetRandomString());

                if (i < wordCount - 1 && random.NextDouble() < 0.5)
                {
                    stringBuilder.Append(',');
                }

                if (i < wordCount - 1)
                {
                    stringBuilder.Append(' ');
                }
            }

            return stringBuilder.ToString().Trim();
        }

        public static string GetAddSpacesToString(string input)
        {
            StringBuilder modifiedString = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                char currentChar = input[i];

                if (currentChar == ' ' || currentChar == ',')
                {
                    if (random.NextDouble() < 0.5)
                    {
                        modifiedString.Append(' ', GetRandomNumber());
                    }

                    modifiedString.Append(currentChar);

                    if (random.NextDouble() < 0.5)
                    {
                        modifiedString.Append(' ', GetRandomNumber());
                    }
                }
                else
                {
                    modifiedString.Append(currentChar);
                }
            }

            return modifiedString.ToString();
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
           actualException => actualException.SameExceptionAs(expectedException);
    }
}
