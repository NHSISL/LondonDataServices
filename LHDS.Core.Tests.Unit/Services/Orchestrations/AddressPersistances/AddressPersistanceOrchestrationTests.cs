// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressMatchers;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions;
using LHDS.Core.Models.Processings.Addresses.Exceptions;
using LHDS.Core.Models.Processings.AddressMatchers.Exceptions;
using LHDS.Core.Services.Orchestrations.AddressPersistances;
using LHDS.Core.Services.Processings.Addresses;
using LHDS.Core.Services.Processings.AddressMatchers;
using LHDS.Core.Services.Processings.ResolvedAddresses;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressPersistances
{
    public partial class AddressPersistanceOrchestrationServiceTests
    {
        private readonly Mock<IAddressProcessingService> addressProcessingServiceMock;
        private readonly Mock<IAddressMatcherProcessingService> addressMatcherProcessingServiceMock;
        private readonly Mock<IResolvedAddressProcessingService> resolvedAddressProcessingServiceMock;
        private readonly Mock<IAuditBroker> auditBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly IAddressPersistanceOrchestrationService addressPersistanceOrchestrationService;

        public AddressPersistanceOrchestrationServiceTests()
        {
            this.addressProcessingServiceMock = new Mock<IAddressProcessingService>();
            this.addressMatcherProcessingServiceMock = new Mock<IAddressMatcherProcessingService>();
            this.resolvedAddressProcessingServiceMock = new Mock<IResolvedAddressProcessingService>();
            this.auditBrokerMock = new Mock<IAuditBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.compareLogic = new CompareLogic();

            this.addressPersistanceOrchestrationService = new AddressPersistanceOrchestrationService(
                addressProcessingService: addressProcessingServiceMock.Object,
                addressMatcherProcessingService: addressMatcherProcessingServiceMock.Object,
                resolvedAddressProcessingService: resolvedAddressProcessingServiceMock.Object,
                auditBroker: auditBrokerMock.Object,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object);
        }

        private static string GetRandomString() =>
           new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
          actualException => actualException.SameExceptionAs(expectedException);

        private Expression<Func<Address, bool>> SameAddressAs(
            Address exprectedAddress)
        {
            return actualAddress =>
                this.compareLogic.Compare(exprectedAddress, actualAddress)
                    .AreEqual;
        }

        private static IQueryable<Address> CreateRandomAddresses(int addressCount)
        {
            return CreateAddressFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: addressCount)
                    .AsQueryable();
        }

        static string ConvertToJSONString(List<KeyValuePair<string, string>> keyValuePairs)
        {
            var dictionary = keyValuePairs.ToDictionary(pair => pair.Key, pair => pair.Value);
            var jsonString = JsonSerializer.Serialize(dictionary);

            return jsonString;
        }

        public static string ConvertToString(List<KeyValuePair<string, string>> keyValuePairs)
        {
            return string.Join(" ", keyValuePairs.Select(kvp => kvp.Value)).Trim();
        }

        private static List<Address> CreateRandomAddressList(int addressCount)
        {
            return CreateAddressFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: addressCount)
                    .ToList();
        }

        private static Filler<Address> CreateAddressFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            List<KeyValuePair<string, string>> compomnents = GenerateRandomKeyValuePairAddress();
            var filler = new Filler<Address>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(address => address.CreatedBy).Use(user)
                .OnProperty(address => address.UpdatedBy).Use(user);

            return filler;
        }

        private static List<KeyValuePair<string, string>> GenerateKeyValuePairRepresentingAddress(Address address)
        {
            var keyValuePairs = new List<KeyValuePair<string, string>>();
            PropertyInfo[] properties = typeof(Address).GetProperties();

            foreach (var property in properties)
            {
                object? value = property.GetValue(address);
                string stringValue = value?.ToString() ?? string.Empty;
                keyValuePairs.Add(new KeyValuePair<string, string>(property.Name, stringValue));
            }

            return keyValuePairs;
        }

        private static List<KeyValuePair<string, string>> GenerateRandomKeyValuePairAddress()
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("house_number", GetRandomString()),
                new KeyValuePair<string, string>("road", GetRandomString()),
                new KeyValuePair<string, string>("city_district", GetRandomString()),
                new KeyValuePair<string, string>("city", GetRandomString()),
                new KeyValuePair<string, string>("postcode", GetRandomString()),
                new KeyValuePair<string, string>("country", GetRandomString()),
            };
        }

        private static List<KeyValuePair<string, string>> GenerateKeyValuePairAddressFromJson(string? jsonPostalAddress)
        {
            var keyValuePairs = new List<KeyValuePair<string, string>>();

            if (string.IsNullOrWhiteSpace(jsonPostalAddress))
            {
                return keyValuePairs;
            }

            var dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonPostalAddress);

            foreach (var item in dictionary)
            {
                var key = item.Key;
                var value = item.Value;
                keyValuePairs.Add(new KeyValuePair<string, string>(key, value));
            }

            return keyValuePairs;
        }

        private static Address CreateRandomAddress(DateTimeOffset dateTimeOffset) =>
            CreateAddressFiller(dateTimeOffset).Create();

        private static ResolvedAddress CreateRandomResolvedAddress(DateTimeOffset dateTimeOffset) =>
            CreateResolvedAddressFiller(dateTimeOffset).Create();

        private static Filler<ResolvedAddress> CreateResolvedAddressFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            List<KeyValuePair<string, string>> compomnents = GenerateRandomKeyValuePairAddress();
            var filler = new Filler<ResolvedAddress>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(address => address.PostalAddress).Use(() => ConvertToString(compomnents))
                .OnProperty(address => address.JsonPostalAddress).Use(() => ConvertToJSONString(compomnents))
                .OnProperty(resolvedAddress => resolvedAddress.CreatedBy).Use(user)
                .OnProperty(resolvedAddress => resolvedAddress.UpdatedBy).Use(user);

            return filler;
        }
        public static ResolvedAddress UpdateResolvedAddress(
            ResolvedAddress inputResolvedAddress,
            AddressMatch matchedAddress)
        {
            inputResolvedAddress.IsMatched = matchedAddress.IsMatched;
            MatchAlgorithmEnum matchAlgorithmEnum = MatchAlgorithmEnum.Human;
            Enum.TryParse(((int)matchedAddress.BestMatch).ToString(), ignoreCase: true, out matchAlgorithmEnum);
            inputResolvedAddress.MatchAlgorithmEnum = matchAlgorithmEnum;

            if (matchedAddress.OriginalAddressComponents.Count() > 0)
            {
                inputResolvedAddress.MatchAlgorithmEnum = MatchAlgorithmEnum.Exact;

                inputResolvedAddress.MatchedPostalAddress = matchedAddress.NormalisedAddressComponents
                    .FirstOrDefault(pair => pair.Key == "PostalAddress").Value;

                inputResolvedAddress.MatchedJsonPostalAddress = matchedAddress.NormalisedAddressComponents
                    .FirstOrDefault(pair => pair.Key == "JsonPostalAddress").Value;

                inputResolvedAddress.MatchedUPRN = matchedAddress.NormalisedAddressComponents
                    .FirstOrDefault(pair => pair.Key == "UPRN").Value;

                inputResolvedAddress.MatchedUPSN = matchedAddress.NormalisedAddressComponents
                    .FirstOrDefault(pair => pair.Key == "UPSN").Value;

                inputResolvedAddress.MatchedOrganisationName = matchedAddress.NormalisedAddressComponents
                    .FirstOrDefault(pair => pair.Key == "OrganisationName").Value;

                inputResolvedAddress.MatchedOrganisationName = matchedAddress.NormalisedAddressComponents
                    .FirstOrDefault(pair => pair.Key == "DepartmentName").Value;

                inputResolvedAddress.MatchedOrganisationName = matchedAddress.NormalisedAddressComponents
                    .FirstOrDefault(pair => pair.Key == "SubBuildingName").Value;

                inputResolvedAddress.MatchedBuildingName = matchedAddress.NormalisedAddressComponents
                    .FirstOrDefault(pair => pair.Key == "MatchedBuildingName").Value;

                inputResolvedAddress.MatchedBuildingNumber = matchedAddress.NormalisedAddressComponents
                    .FirstOrDefault(pair => pair.Key == "MatchedBuildingNumber").Value;

                inputResolvedAddress.MatchedDependentThoroughfare = matchedAddress.NormalisedAddressComponents
                    .FirstOrDefault(pair => pair.Key == "MatchedDependentThoroughfare").Value;

                inputResolvedAddress.MatchedThoroughfare = matchedAddress.NormalisedAddressComponents
                    .FirstOrDefault(pair => pair.Key == "MatchedThoroughfare").Value;

                inputResolvedAddress.MatchedDoubleDependentLocality = matchedAddress.NormalisedAddressComponents
                    .FirstOrDefault(pair => pair.Key == "MatchedDoubleDependentLocality").Value;

                inputResolvedAddress.MatchedDependentLocality = matchedAddress.NormalisedAddressComponents
                    .FirstOrDefault(pair => pair.Key == "MatchedDependentLocality").Value;

                inputResolvedAddress.MatchedPostTown = matchedAddress.NormalisedAddressComponents
                    .FirstOrDefault(pair => pair.Key == "MatchedPostTown").Value;

                inputResolvedAddress.MatchedPostCode = matchedAddress.NormalisedAddressComponents
                    .FirstOrDefault(pair => pair.Key == "MatchedPostCode").Value;
            }
            return inputResolvedAddress;
        }

        private Expression<Func<HashSet<AddressMatch>, bool>> SameAddressToMatchAs(
           HashSet<AddressMatch> expectedAddressToMatch)
        {
            return actualAddressToMatch =>
                this.compareLogic.Compare(expectedAddressToMatch, actualAddressToMatch)
                    .AreEqual;
        }

        private Expression<Func<List<KeyValuePair<string, string>>, bool>> SameResolvedAddressAs(
            List<KeyValuePair<string, string>> expectedResolvedAddress)
        {
            return actualResolvedAddress =>
                this.compareLogic.Compare(expectedResolvedAddress, actualResolvedAddress)
                    .AreEqual;
        }

        public static TheoryData<Xeption> AddressPersistenceDependencyValidationExceptions()
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
                    message: "Address matcher processing validation errors occurred, please try again.",
                    innerException),

                 new ResolvedAddressValidationException(
                    message: "Resolved address processing validation errors occurred, please try again.",
                    innerException),

                new ResolvedAddressDependencyValidationException(
                    message: "Resolved address processing dependency validation occurred, please try again.",
                    innerException),
            };
        }

        public static TheoryData<Xeption> AddressPersistenceDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new AddressProcessingDependencyException(
                    message: "Address processing dependency error occurred, please try again.",
                    innerException),

                new AddressProcessingServiceException(
                    message: "Address processing service error occurred, please contact support.",
                    innerException),

                 new AddressMatcherProcessingServiceException(
                    message: "Address matcher processing service error occurred, please contact support.",
                    innerException),

                 new ResolvedAddressDependencyException(
                    message: "Resolved address processing dependency error occurred, please try again.",
                    innerException),

                new ResolvedAddressServiceException(
                    message: "Resolved address processing service error occurred, please contact support.",
                    innerException),
            };
        }
    }
}
