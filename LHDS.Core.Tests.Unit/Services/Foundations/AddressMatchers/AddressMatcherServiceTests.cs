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

        public static TheoryData<List<KeyValuePair<string, string>>, HashSet<AddressMatch>, int, bool> AddressToMatch()
        {
            var theoryData = new TheoryData<List<KeyValuePair<string, string>>, HashSet<AddressMatch>, int, bool>
            {
                {
                    new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("house_number", "10"),
                        new KeyValuePair<string, string>("road", "downing str"),
                        new KeyValuePair<string, string>("city_district", "westminster"),
                        new KeyValuePair<string, string>("city", "london"),
                        new KeyValuePair<string, string>("postcode", "sw1a2aa"),
                        new KeyValuePair<string, string>("country", "uk")
                    },
                    new HashSet<AddressMatch> {
                        new AddressMatch
                        {
                            PostalAddress = "10 downing str westminster london sw1a2aa uk",
                            JsonPostalAddress = "[{\"Key\":\"house_number\",\"Value\":\"10\"}," +
                                "{\"Key\":\"road\",\"Value\":\"downing str\"}," +
                                "{\"Key\":\"city_district\",\"Value\":\"westminster\"}," +
                                "{\"Key\":\"city\",\"Value\":\"london\"}," +
                                "{\"Key\":\"postcode\",\"Value\":\"sw1a2aa\"}," +
                                "{\"Key\":\"country\",\"Value\":\"uk\"}]",
                            NormalisedAddressComponents = new List<KeyValuePair<string, string>>
                            {
                                new KeyValuePair<string, string>("house_number", "10"),
                                new KeyValuePair<string, string>("road", "downing str"),
                                new KeyValuePair<string, string>("city_district", "westminster"),
                                new KeyValuePair<string, string>("city", "london"),
                                new KeyValuePair<string, string>("postcode", "sw1a2aa"),
                                new KeyValuePair<string, string>("country", "uk")
                            },
                            UPRN = GetRandomString(),
                            UPSN = GetRandomString(),
                        }
                    },
                    6,
                    true
                },
                {
                    new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("house_number", "10"),
                        new KeyValuePair<string, string>("road", "downing str"),
                        new KeyValuePair<string, string>("city_district", "westminster"),
                        new KeyValuePair<string, string>("city", "london"),
                        new KeyValuePair<string, string>("postcode", "sw1a2aa"),
                        new KeyValuePair<string, string>("country", "uk")
                    },
                    new HashSet<AddressMatch> {
                        new AddressMatch
                        {
                            PostalAddress = "downing str westminster london sw1a2aa uk",
                            JsonPostalAddress = "[{\"Key\":\"road\",\"Value\":\"downing str\"}," +
                                "{\"Key\":\"city_district\",\"Value\":\"westminster\"}," +
                                "{\"Key\":\"city\",\"Value\":\"london\"}," +
                                "{\"Key\":\"postcode\",\"Value\":\"sw1a2aa\"}," +
                                "{\"Key\":\"country\",\"Value\":\"uk\"}]",
                            NormalisedAddressComponents = new List<KeyValuePair<string, string>>
                            {
                                new KeyValuePair<string, string>("road", "downing str"),
                                new KeyValuePair<string, string>("city_district", "westminster"),
                                new KeyValuePair<string, string>("city", "london"),
                                new KeyValuePair<string, string>("postcode", "sw1a2aa"),
                                new KeyValuePair<string, string>("country", "uk")
                            },
                            UPRN = GetRandomString(),
                            UPSN = GetRandomString(),
                        }
                    },
                    5,
                    false
                },
                {
                    new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("house_number", "10"),
                        new KeyValuePair<string, string>("road", "downing str"),
                        new KeyValuePair<string, string>("city_district", "westminster"),
                        new KeyValuePair<string, string>("city", "london"),
                        new KeyValuePair<string, string>("postcode", "sw1a2aa"),
                        new KeyValuePair<string, string>("country", "uk")
                    },
                    new HashSet<AddressMatch> {
                        new AddressMatch
                        {
                            PostalAddress = "10 westminster london sw1a2aa uk",
                            JsonPostalAddress = "[{\"Key\":\"house_number\",\"Value\":\"10\"}," +
                                "{\"Key\":\"city_district\",\"Value\":\"westminster\"}," +
                                "{\"Key\":\"city\",\"Value\":\"london\"}," +
                                "{\"Key\":\"postcode\",\"Value\":\"sw1a2aa\"}," +
                                "{\"Key\":\"country\",\"Value\":\"uk\"}]",
                            NormalisedAddressComponents = new List<KeyValuePair<string, string>>
                            {
                                new KeyValuePair<string, string>("house_number", "10"),
                                new KeyValuePair<string, string>("city_district", "westminster"),
                                new KeyValuePair<string, string>("city", "london"),
                                new KeyValuePair<string, string>("postcode", "sw1a2aa"),
                                new KeyValuePair<string, string>("country", "uk")
                            },
                            UPRN = GetRandomString(),
                            UPSN = GetRandomString(),
                        }
                    },
                    5,
                    true
                },
                {
                    new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("house_number", "10"),
                        new KeyValuePair<string, string>("road", "downing str"),
                        new KeyValuePair<string, string>("city_district", "westminster"),
                        new KeyValuePair<string, string>("city", "london"),
                        new KeyValuePair<string, string>("postcode", "sw1a2aa"),
                        new KeyValuePair<string, string>("country", "uk")
                    },
                    new HashSet<AddressMatch> {
                        new AddressMatch
                        {
                            PostalAddress = "10 sw1a2aa",
                            JsonPostalAddress = "[{\"Key\":\"house_number\",\"Value\":\"10\"}," +
                                "{\"Key\":\"postcode\",\"Value\":\"sw1a2aa\"}]",
                            NormalisedAddressComponents = new List<KeyValuePair<string, string>>
                            {
                                new KeyValuePair<string, string>("house_number", "10"),
                                new KeyValuePair<string, string>("postcode", "sw1a2aa"),
                            },
                            UPRN = GetRandomString(),
                            UPSN = GetRandomString(),
                        }
                    },
                    2,
                    true
                },
                {
                    new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("house_number", "10"),
                        new KeyValuePair<string, string>("road", "downing str"),
                        new KeyValuePair<string, string>("city_district", "westminster"),
                        new KeyValuePair<string, string>("city", "london"),
                        new KeyValuePair<string, string>("postcode", "sw1a2aa"),
                        new KeyValuePair<string, string>("country", "uk")
                    },
                    new HashSet<AddressMatch> {
                        new AddressMatch
                        {
                            PostalAddress = "1 sw1a2aa",
                            JsonPostalAddress = "[{\"Key\":\"house_number\",\"Value\":\"1\"}," +
                                "{\"Key\":\"postcode\",\"Value\":\"sw1a2aa\"}]",
                            NormalisedAddressComponents = new List<KeyValuePair<string, string>>
                            {
                                new KeyValuePair<string, string>("house_number", "1"),
                                new KeyValuePair<string, string>("postcode", "sw1a2aa"),
                            },
                            UPRN = GetRandomString(),
                            UPSN = GetRandomString(),
                        }
                    },
                    1,
                    false
                },
                {
                    new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("house_number", "10"),
                        new KeyValuePair<string, string>("road", "downing str"),
                        new KeyValuePair<string, string>("city_district", "westminster"),
                        new KeyValuePair<string, string>("city", "london"),
                        new KeyValuePair<string, string>("postcode", "sw1a2aa"),
                        new KeyValuePair<string, string>("country", "uk")
                    },
                    new HashSet<AddressMatch> {
                        new AddressMatch
                        {
                            PostalAddress = "10 sw1a2ab",
                            JsonPostalAddress = "[{\"Key\":\"house_number\",\"Value\":\"1\"}," +
                                "{\"Key\":\"postcode\",\"Value\":\"sw1a2aa\"}]",
                            NormalisedAddressComponents = new List<KeyValuePair<string, string>>
                            {
                                new KeyValuePair<string, string>("house_number", "10"),
                                new KeyValuePair<string, string>("postcode", "sw1a2ab"),
                            },
                            UPRN = GetRandomString(),
                            UPSN = GetRandomString(),
                        }
                    },
                    1,
                    false
                },
            };

            return theoryData;
        }

        public static TheoryData<HashSet<AddressMatch>, BestMatchEnum> CheckForBestMatch()
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

