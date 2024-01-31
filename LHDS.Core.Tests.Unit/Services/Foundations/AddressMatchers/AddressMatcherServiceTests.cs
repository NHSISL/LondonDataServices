// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.AddressMatchers;
using LHDS.Core.Services.Foundations.AddressMatchers;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressMatchers
{
    public partial class AddressMatcherServiceTests
    {
        private readonly Mock<ILoggingBroker> loggingBrokerMock = new Mock<ILoggingBroker>();
        private readonly IAddressMatcherService addressMatcherService;

        public AddressMatcherServiceTests()
        {
            addressMatcherService = new AddressMatcherService(
                loggingBroker: loggingBrokerMock.Object);

            this.addressMatcherService = new AddressMatcherService(loggingBroker: loggingBrokerMock.Object);
        }

        public static TheoryData<List<KeyValuePair<string, string>>> HouseNumbersWithCharacters()
        {
            return new TheoryData<List<KeyValuePair<string, string>>>
            {
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("house_number", "10"),
                    new KeyValuePair<string, string>("road", "Downing Street"),
                    new KeyValuePair<string, string>("suburb", "Westminster"),
                    new KeyValuePair<string, string>("city", "London"),
                    new KeyValuePair<string, string>("postcode", "SW1A2AA"),
                    new KeyValuePair<string, string>("country", "United Kingdom")
                },
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("house_number", "10 A"),
                    new KeyValuePair<string, string>("road", "Downing Street"),
                    new KeyValuePair<string, string>("suburb", "Westminster"),
                    new KeyValuePair<string, string>("city", "London"),
                    new KeyValuePair<string, string>("postcode", "SW1A2AA"),
                    new KeyValuePair<string, string>("country", "United Kingdom")
                },
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("house_number", "10 ABC"),
                    new KeyValuePair<string, string>("road", "Downing Street"),
                    new KeyValuePair<string, string>("suburb", "Westminster"),
                    new KeyValuePair<string, string>("city", "London"),
                    new KeyValuePair<string, string>("postcode", "SW1A2AA"),
                    new KeyValuePair<string, string>("country", "United Kingdom")
                },
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("house_number", "10A"),
                    new KeyValuePair<string, string>("road", "Downing Street"),
                    new KeyValuePair<string, string>("suburb", "Westminster"),
                    new KeyValuePair<string, string>("city", "London"),
                    new KeyValuePair<string, string>("postcode", "SW1A2AA"),
                    new KeyValuePair<string, string>("country", "United Kingdom")
                },
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("house_number", "10ABC"),
                    new KeyValuePair<string, string>("road", "Downing Street"),
                    new KeyValuePair<string, string>("suburb", "Westminster"),
                    new KeyValuePair<string, string>("city", "London"),
                    new KeyValuePair<string, string>("postcode", "SW1A2AA"),
                    new KeyValuePair<string, string>("country", "United Kingdom")
                }
            };
        }

        public static TheoryData CheckForBestMatch()
        {
            return new TheoryData<HashSet<AddressMatch>, BestMatchEnum>
            {
                {
                    new HashSet<AddressMatch>
                    {
                        new AddressMatch
                        {
                            MatchedComponents = 0,
                            MatchingCoreComponents = false,
                        }
                    },
                    BestMatchEnum.None
                },
                {
                    new HashSet<AddressMatch>
                    {
                        new AddressMatch
                        {
                            MatchedComponents = 1,
                            MatchingCoreComponents = true,
                        }
                    },
                    BestMatchEnum.Single
                },
                {
                    new HashSet<AddressMatch>
                    {
                        new AddressMatch
                        {
                            MatchedComponents = 1,
                            MatchingCoreComponents = true,
                        },
                        new AddressMatch
                        {
                            MatchedComponents = 1,
                            MatchingCoreComponents = true,
                        },
                    },
                    BestMatchEnum.Multiple
                }
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

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
           actualException => actualException.SameExceptionAs(expectedException);
    }
}

