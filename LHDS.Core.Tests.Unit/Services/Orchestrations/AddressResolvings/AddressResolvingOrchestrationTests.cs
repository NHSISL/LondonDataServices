// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Serializations;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Processings.Addresses.Exceptions;
using LHDS.Core.Models.Processings.AddressMatchers.Exceptions;
using LHDS.Core.Models.Processings.ResolvedAddresses.Exceptions;
using LHDS.Core.Services.Orchestrations.AddressResolvings;
using LHDS.Core.Services.Processings.Addresses;
using LHDS.Core.Services.Processings.AddressMatchers;
using LHDS.Core.Services.Processings.ResolvedAddresses;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressResolvings
{
    public partial class AddressResolvingOrchestrationServiceTests
    {
        private readonly Mock<IAddressProcessingService> addressProcessingServiceMock;
        private readonly Mock<IAddressMatcherProcessingService> addressMatcherProcessingServiceMock;
        private readonly Mock<IResolvedAddressProcessingService> resolvedAddressProcessingServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ISerializationBroker> serializationBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly IAddressResolvingOrchestrationService addressResolvingOrchestrationService;

        public AddressResolvingOrchestrationServiceTests()
        {
            this.addressProcessingServiceMock = new Mock<IAddressProcessingService>();
            this.addressMatcherProcessingServiceMock = new Mock<IAddressMatcherProcessingService>();
            this.resolvedAddressProcessingServiceMock = new Mock<IResolvedAddressProcessingService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.serializationBrokerMock = new Mock<ISerializationBroker>();
            this.compareLogic = new CompareLogic();

            this.addressResolvingOrchestrationService = new AddressResolvingOrchestrationService(
                addressProcessingService: addressProcessingServiceMock.Object,
                addressMatcherProcessingService: addressMatcherProcessingServiceMock.Object,
                resolvedAddressProcessingService: resolvedAddressProcessingServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object,
                serializationBroker: serializationBrokerMock.Object);
        }

        private static string GetRandomString() =>
           new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
          actualException => actualException.SameExceptionAs(expectedException);

        private static List<Address> CreateRandomAddressList()
        {
            return CreateAddressFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .ToList();
        }

        private Expression<Func<ResolvedAddress, bool>> SameResolvedAddressAs(
            ResolvedAddress exprectedResolvedAddress)
        {
            return actualResolvedAddress =>
                this.compareLogic.Compare(exprectedResolvedAddress, actualResolvedAddress)
                    .AreEqual;
        }
        private static Address CreateRandomAddress() =>
            CreateAddressFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static Address CreateRandomAddress(DateTimeOffset dateTimeOffset) =>
            CreateAddressFiller(dateTimeOffset).Create();

        private static Filler<Address> CreateAddressFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            List<KeyValuePair<string, string>> compomnents = GenerateRandomKeyValuePairAddress();
            var filler = new Filler<Address>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(address => address.PostalAddress).Use(() => ConvertToString(compomnents))
                .OnProperty(address => address.JsonPostalAddress).Use(() => ConvertToJSONString(compomnents))
                .OnProperty(address => address.CreatedBy).Use(user)
                .OnProperty(address => address.UpdatedBy).Use(user);

            return filler;
        }

        public static AddressNormalisation CreateRandomAddressNormalisation()
        {
            List<KeyValuePair<string, string>> components = GenerateRandomKeyValuePairAddress();

            return new AddressNormalisation
            {
                PostalAddress = ConvertToString(components),
                JsonPostalAddress = ConvertToJSONString(components),
                AddressComponents = components
            };
        }

        static string GetRandomUKPostcode()
        {
            Random random = new Random();

            // Example postcode format: AA1 1AA
            char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            int randomLetterIndex1 = random.Next(0, letters.Length);
            int randomLetterIndex2 = random.Next(0, letters.Length);

            int randomDigit1 = random.Next(0, 10);
            int randomDigit2 = random.Next(0, 10);

            return $"{letters[randomLetterIndex1]}{letters[randomLetterIndex2]}{randomDigit1} {randomDigit2}{letters[randomLetterIndex1]}{letters[randomLetterIndex2]}";
        }

        private static List<KeyValuePair<string, string>> GetRandomAddressComponents()
        {
            int numberOfComponents = GetRandomNumber();

            var components = new List<KeyValuePair<string, string>>();

            for (int i = 0; i < numberOfComponents; i++)
            {
                string key = GetRandomString();
                string value = GetRandomString();
                components.Add(new KeyValuePair<string, string>(key, value));
            }

            return components;
        }

        private static ResolvedAddress CreateRandomResolvedAddress() =>
            CreateResolvedAddressFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static ResolvedAddress CreateRandomResolvedAddress(DateTimeOffset dateTimeOffset) =>
            CreateResolvedAddressFiller(dateTimeOffset).Create();

        private static Filler<ResolvedAddress> CreateResolvedAddressFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<ResolvedAddress>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(resolvedAddress => resolvedAddress.CreatedBy).Use(user)
                .OnProperty(resolvedAddress => resolvedAddress.UpdatedBy).Use(user);

            return filler;
        }

        static List<KeyValuePair<string, string>> GenerateKeyValuePairList(int count)
        {
            List<KeyValuePair<string, string>> keyValuePairList = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < count; i++)
            {
                keyValuePairList.Add(new KeyValuePair<string, string>(GetRandomString(), GetRandomString()));
            }
            return keyValuePairList;
        }

        static List<KeyValuePair<string, string>> GenerateRandomKeyValuePairAddress()
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

        static string ConvertToJSONString(List<KeyValuePair<string, string>> keyValuePairs)
        {
            var jsonString = JsonSerializer.Serialize(keyValuePairs);
            return jsonString;
        }

        public static string ConvertToString(List<KeyValuePair<string, string>> keyValuePairs)
        {
            return string.Join(" ", keyValuePairs.Select(kvp => kvp.Value)).Trim();
        }

        public static List<KeyValuePair<string, string>> ConvertStringToKeyValue(string jsonString)
        {
            var components = JsonSerializer.Deserialize<List<KeyValuePair<string, string>>>(jsonString);
            return components;
        }

        public static TheoryData AddressResolvingDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new AddressProcessingValidationException(
                    message: "Address processing validation errors occurred, please try again.",
                    innerException),

                new AddressProcessingDependencyValidationException(
                    message: "Address processing dependency validation occurred, please try again.",
                    innerException),

                new AddressMatcherProcessingValidationException(
                    message: "Address matcher processing validation errors occured, please try again",
                    innerException),

                new AddressMatcherProcessingDependencyValidationException(
                    message: "Address matcher processing dependency validation errors occured, please try again",
                    innerException),

                new ResolvedAddressProcessingValidationException(
                    message: "Resolved Address validation errors occurred, please try again.",
                    innerException),

                new ResolvedAddressProcessingDependencyValidationException(
                    message: "Resolved Address dependency validation occurred, please try again.",
                    innerException)
            };
        }

        public static TheoryData AddressResolvingDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new AddressProcessingDependencyException(
                    message: "Address processing dependency error occurred, contact support.",
                    innerException),

                new AddressProcessingServiceException(
                    message: "Address processing service error occurred, contact support.",
                    innerException),

                new AddressMatcherProcessingDependencyException(
                    message: "Address matcher processing dependency error occurred, contact support.",
                    innerException),

                new AddressMatcherProcessingServiceException(
                    message: "Address matcher processing service error occurred, contact support.",
                    innerException),

                 new ResolvedAddressProcessingDependencyException(
                    message: "Resolved address dependency error occurred, contact support.",
                    innerException),

                 new ResolvedAddressProcessingServiceException(
                    message: "Resolved address processing service error occurred, contact support.",
                    innerException),
            };
        }
    }
}
