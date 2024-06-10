// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.AddressMatchers;
using LHDS.Core.Services.Foundations.AddressMatchers;
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
        private readonly Mock<IAddressMatcherService> addressMatcherServiceMock = new Mock<IAddressMatcherService>();
        private readonly IAddressMatcherProcessingService addressMatcherProcessingService;

        public AddressMatcherProcessingServiceTests()
        {
            addressMatcherProcessingService = new AddressMatcherProcessingService(
                addressMatcherService: addressMatcherServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }

        public static TheoryData<string> UncleanedAddressString()
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

        public static TheoryData<List<KeyValuePair<string, string>>, HashSet<AddressMatch>> AddressToMatch()
        {
            var theoryData = new TheoryData<List<KeyValuePair<string, string>>, HashSet<AddressMatch>>
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
                    }
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
                    }
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
                    }
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
                    }
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
                    }
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
                    }
                },
            };

            return theoryData;
        }

        public static HashSet<AddressMatch> CreateNoBestMatch()
        {
            return new HashSet<AddressMatch>
            {
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
                    MatchingCoreComponents = false,
                    MatchedComponents = 6,
                },
                new AddressMatch
                {
                    PostalAddress = "10 downing str london sw1a2aa",
                    JsonPostalAddress = "[{\"Key\":\"house_number\",\"Value\":\"10\"}," +
                        "{\"Key\":\"road\",\"Value\":\"downing str\"}," +
                        "{\"Key\":\"city\",\"Value\":\"london\"}," +
                        "{\"Key\":\"postcode\",\"Value\":\"sw1a2aa\"}]",
                    NormalisedAddressComponents = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("house_number", "10"),
                        new KeyValuePair<string, string>("road", "downing str"),
                        new KeyValuePair<string, string>("city", "london"),
                        new KeyValuePair<string, string>("postcode", "sw1a2aa"),
                    },
                    UPRN = GetRandomString(),
                    UPSN = GetRandomString(),
                    MatchingCoreComponents = false,
                    MatchedComponents = 4,
                },
                new AddressMatch
                {
                    PostalAddress = "10 sw1a2aa",
                    JsonPostalAddress = "[{\"Key\":\"house_number\",\"Value\":\"10\"}," +
                        "{\"Key\":\"road\",\"Value\":\"downing str\"}," +
                        "{\"Key\":\"postcode\",\"Value\":\"sw1a2aa\"}]",
                    NormalisedAddressComponents = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("house_number", "10"),
                        new KeyValuePair<string, string>("postcode", "sw1a2aa"),
                    },
                    UPRN = GetRandomString(),
                    UPSN = GetRandomString(),
                    MatchingCoreComponents = false,
                    MatchedComponents = 2,
                }
           };
        }

        public HashSet<AddressMatch> CreateSingleBestMatch()
        {
            return new HashSet<AddressMatch>
            {
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
                    MatchingCoreComponents = true,
                    MatchedComponents = 6,
                },
                new AddressMatch
                {
                    PostalAddress = "10 downing str london sw1a2aa",
                    JsonPostalAddress = "[{\"Key\":\"house_number\",\"Value\":\"10\"}," +
                        "{\"Key\":\"road\",\"Value\":\"downing str\"}," +
                        "{\"Key\":\"city\",\"Value\":\"london\"}," +
                        "{\"Key\":\"postcode\",\"Value\":\"sw1a2aa\"}]",
                    NormalisedAddressComponents = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("house_number", "10"),
                        new KeyValuePair<string, string>("road", "downing str"),
                        new KeyValuePair<string, string>("city", "london"),
                        new KeyValuePair<string, string>("postcode", "sw1a2aa"),
                    },
                    UPRN = GetRandomString(),
                    UPSN = GetRandomString(),
                    MatchingCoreComponents = true,
                    MatchedComponents = 4,
                },
                new AddressMatch
                {
                    PostalAddress = "10 sw1a2aa",
                    JsonPostalAddress = "[{\"Key\":\"house_number\",\"Value\":\"10\"}," +
                        "{\"Key\":\"road\",\"Value\":\"downing str\"}," +
                        "{\"Key\":\"postcode\",\"Value\":\"sw1a2aa\"}]",
                    NormalisedAddressComponents = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("house_number", "10"),
                        new KeyValuePair<string, string>("postcode", "sw1a2aa"),
                    },
                    UPRN = GetRandomString(),
                    UPSN = GetRandomString(),
                    MatchingCoreComponents = true,
                    MatchedComponents = 2,
                }
            };
        }

        public static HashSet<AddressMatch> CreateMultipleBestMatch()
        {
            return new HashSet<AddressMatch>
            {
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
                    MatchingCoreComponents = false,
                    MatchedComponents = 6,
                },
                new AddressMatch
                {
                    PostalAddress = "10 downing str london sw1a2aa",
                    JsonPostalAddress = "[{\"Key\":\"house_number\",\"Value\":\"10\"}," +
                        "{\"Key\":\"road\",\"Value\":\"downing str\"}," +
                        "{\"Key\":\"city\",\"Value\":\"london\"}," +
                        "{\"Key\":\"postcode\",\"Value\":\"sw1a2aa\"}]",
                    NormalisedAddressComponents = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("house_number", "10"),
                        new KeyValuePair<string, string>("road", "downing str"),
                        new KeyValuePair<string, string>("city", "london"),
                        new KeyValuePair<string, string>("postcode", "sw1a2aa"),
                    },
                    UPRN = GetRandomString(),
                    UPSN = GetRandomString(),
                    MatchingCoreComponents = false,
                    MatchedComponents = 4,
                },
                new AddressMatch
                {
                    PostalAddress = "10 sw1a2aa",
                    JsonPostalAddress = "[{\"Key\":\"house_number\",\"Value\":\"10\"}," +
                        "{\"Key\":\"road\",\"Value\":\"downing str\"}," +
                        "{\"Key\":\"postcode\",\"Value\":\"sw1a2aa\"}]",
                    NormalisedAddressComponents = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("house_number", "10"),
                        new KeyValuePair<string, string>("postcode", "sw1a2aa"),
                    },
                    UPRN = GetRandomString(),
                    UPSN = GetRandomString(),
                    MatchingCoreComponents = false,
                    MatchedComponents = 2,
                }
           };
        }

        public static List<KeyValuePair<string, string>> CreateKeyValuePairList()
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("house_number", GetRandomNumber().ToString()),
                new KeyValuePair<string, string>("road", GetRandomString()),
                new KeyValuePair<string, string>("city_district", GetRandomString()),
                new KeyValuePair<string, string>("city", GetRandomString()),
                new KeyValuePair<string, string>("postcode", GetRandomString()),
                new KeyValuePair<string, string>("country", GetRandomString())
            };
        }

        public static List<KeyValuePair<string, string>> CreateRandomKeyValuePairList()
        {
            int number = GetRandomNumber();
            List<KeyValuePair<string, string>> randomList = new List<KeyValuePair<string, string>>();

            for (int i = 0; i < number; i++)
            {
                randomList.Add(new KeyValuePair<string, string>(GetRandomString(), GetRandomString()));
            }

            return randomList;
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
                    {"123 Christo Street, London, IP39WF, United Kingdom", "IP39WF"},
                    {"123 Christo Street, London, IP3 9WF, United Kingdom", "IP3 9WF"},
                    {"123 Main Street, London, CR2 0HG, CR2 0HG, United Kingdom" , "CR2 0HG"},
                    {"123 Christo Street, London, W1A 1AA, United Kingdom, Additional Info, Longer Street Name, More Details, Extended Address Information, Extra Information, Additional Details, Longer City Name, Extended Street Name, More Information, Extra Details", "W1A 1AA"}
                };
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
           actualException => actualException.SameExceptionAs(expectedException);
    }
}

