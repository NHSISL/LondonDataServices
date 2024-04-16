// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using LHDS.Core.Models.Foundations.AddressParsers.Exceptions;
using LHDS.Core.Services.Foundations.AddressNormalisations;
using LHDS.Core.Services.Foundations.AddressParsers;
using LHDS.Core.Services.Orchestrations.AddressExtractions;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExtractionOrchestrationServiceTests
    {
        private readonly Mock<IAddressParserService> addressParserServiceMock;
        private readonly Mock<IAddressNormalisationService> addressNormalisationServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly IAddressExtractionOrchestrationService addressExtractionOrchestrationService;
        private readonly ITestOutputHelper output;

        public AddressExtractionOrchestrationServiceTests(ITestOutputHelper output)
        {
            this.addressParserServiceMock = new Mock<IAddressParserService>();
            this.addressNormalisationServiceMock = new Mock<IAddressNormalisationService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.compareLogic = new CompareLogic();
            this.output = output;

            this.addressExtractionOrchestrationService = new AddressExtractionOrchestrationService(
                addressParserService: addressParserServiceMock.Object,
                addressNormalisationService: addressNormalisationServiceMock.Object,
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

        private static IQueryable<Address> CreateRandomAddresses()
        {
            return CreateAddressFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static Address CreateRandomAddress(DateTimeOffset dateTimeOffset) =>
            CreateAddressFiller(dateTimeOffset).Create();

        private static Filler<Address> CreateAddressFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            string normalised = null;
            var filler = new Filler<Address>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(address => address.JsonPostalAddress).IgnoreIt()
                .OnProperty(address => address.PostalAddress).IgnoreIt()
                .OnProperty(address => address.CreatedBy).Use(user)
                .OnProperty(address => address.CreatedBy).Use(user)
                .OnProperty(address => address.UpdatedBy).Use(user);

            return filler;
        }

        public static TheoryData AddressExtractionOrchestrationDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new AddressParserValidationException(
                    message: "Address parser validation error occured, please try again",
                    innerException),

                new AddressParserDependencyValidationException(
                    message: "Address parser dependency validation error occurred, please try again.",
                    innerException),
            };
        }

        public static TheoryData AddressExtractionDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new AddressParserDependencyException(
                    message: "Address parser dependency error occurred, contact support.",
                    innerException),

                new AddressParserServiceException(
                    message: "Address parser service error occurred, contact support.",
                    innerException),
            };
        }
    }
}
