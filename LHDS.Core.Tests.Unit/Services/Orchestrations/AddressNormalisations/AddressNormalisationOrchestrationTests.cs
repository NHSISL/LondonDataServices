// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions;
using LHDS.Core.Services.Orchestrations.AddressNormalisations;
using LHDS.Core.Services.Processings.AddressNormalisations;
using LHDS.Core.Services.Processings.AddressParsers;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressNormalisations
{
    public partial class AddressNormalisationOrchestrationServiceTests
    {
        private readonly Mock<IAddressParserProcessingService> addressParserProcessingServiceMock;
        private readonly Mock<IAddressNormalisationProcessingService> addressNormalisationProcessingServiceMock;
        private readonly Mock<IAuditBroker> auditBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<IIdentifierBroker> identifierBrokerMock;
        private readonly IAddressNormalisationOrchestrationService addressNormalisationOrchestrationService;
        private readonly ICompareLogic compareLogic;
        private readonly ITestOutputHelper output;

        public AddressNormalisationOrchestrationServiceTests(ITestOutputHelper output)
        {
            this.addressParserProcessingServiceMock = new Mock<IAddressParserProcessingService>();
            this.addressNormalisationProcessingServiceMock = new Mock<IAddressNormalisationProcessingService>();
            this.auditBrokerMock = new Mock<IAuditBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.identifierBrokerMock = new Mock<IIdentifierBroker>();
            this.compareLogic = new CompareLogic();
            this.output = output;

            this.addressNormalisationOrchestrationService = new AddressNormalisationOrchestrationService(
                addressParserProcessingService: addressParserProcessingServiceMock.Object,
                addressNormalisationProcessingService: addressNormalisationProcessingServiceMock.Object,
                auditBroker: auditBrokerMock.Object,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object,
                identifierBroker: identifierBrokerMock.Object);
        }

        private static string GetRandomString() =>
           new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
           new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
          actualException => actualException.SameExceptionAs(expectedException);

        private static IQueryable<Address> CreateRandomAddresses()
        {
            return CreateAddressFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static Address CreateRandomAddress(DateTimeOffset dateTimeOffset) =>
            CreateAddressFiller(dateTimeOffset).Create();

        static List<KeyValuePair<string, string>> GenerateKeyValuePairList(int count)
        {
            List<KeyValuePair<string, string>> keyValuePairList = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < count; i++)
            {
                keyValuePairList.Add(new KeyValuePair<string, string>(GetRandomString(), GetRandomString()));
            }
            return keyValuePairList;
        }

        private string GenerateStringAddress(Address address)
        {
            List<string> addressList = new List<string> {
            address.OrganisationName,
            address.DepartmentName,
            address.SubBuildingName,
            address.BuildingName,
            address.BuildingNumber,
            address.DependentThoroughfare,
            address.Thoroughfare,
            address.DoubleDependentLocality,
            address.DependentLocality,
            address.PostTown,
            address.PostCode};

            return string.Join("", addressList.Where(s => !string.IsNullOrEmpty(s)));
        }

        private static Filler<Address> CreateAddressFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<Address>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(address => address.CreatedBy).Use(user)
                .OnProperty(address => address.UpdatedBy).Use(user);

            return filler;
        }

        public static TheoryData DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new AddressNormalisationValidationException(
                    message: "Address Normalisation validation errors occured, please try again",
                    innerException),

                new AddressNormalisationDependencyValidationException(
                    message: "Address Normalisation dependency validation occurred, please try again.",
                    innerException)
            };
        }
        public static TheoryData DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new AddressNormalisationDependencyException(
                    message: "Address normalisation dependency error occurred, contact support.",
                    innerException),

                new AddressNormalisationServiceException(
                    message: "Address normalisation service error occurred, contact support.",
                    innerException)
            };
        }
    }
}
