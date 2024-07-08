// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions;
using LHDS.Core.Models.Foundations.AddressParsers.Exceptions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Services.Orchestrations.AddressExtractions;
using LHDS.Core.Services.Processings.Addresses;
using LHDS.Core.Services.Processings.AddressNormalisations;
using Moq;
using NHSISL.CsvHelperClient.Models.Clients.CsvHelpers.Exceptions;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExtractionOrchestrationServiceTests
    {
        private readonly Mock<IAddressNormalisationProcessingService> addressNormalisationProcessingServiceMock;
        private readonly Mock<IAddressProcessingService> addressProcessingServiceMock;
        private readonly Mock<IAuditBroker> auditBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<IIdentifierBroker> identifierBrokerMock;
        private readonly Mock<ICsvHelperBroker> csvHelperBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly IAddressExtractionOrchestrationService addressExtractionOrchestrationService;
        private readonly ITestOutputHelper output;

        public AddressExtractionOrchestrationServiceTests(ITestOutputHelper output)
        {
            this.addressNormalisationProcessingServiceMock = new Mock<IAddressNormalisationProcessingService>();
            this.addressProcessingServiceMock = new Mock<IAddressProcessingService>();
            this.auditBrokerMock = new Mock<IAuditBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.csvHelperBrokerMock = new Mock<ICsvHelperBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.identifierBrokerMock = new Mock<IIdentifierBroker>();
            this.compareLogic = new CompareLogic();
            this.output = output;

            this.addressExtractionOrchestrationService = new AddressExtractionOrchestrationService(
                addressNormalisationProcessingService: addressNormalisationProcessingServiceMock.Object,
                addressProcessingService: addressProcessingServiceMock.Object,
                auditBroker: auditBrokerMock.Object,
                loggingBroker: loggingBrokerMock.Object,
                csvHelperBroker: csvHelperBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object,
                identifierBroker: identifierBrokerMock.Object);
        }

        private Expression<Func<Address, bool>> SameAddressAs(Address expectedAddress)
        {
            return actualAddress =>
                this.compareLogic.Compare(expectedAddress, actualAddress)
                    .AreEqual;
        }

        private Expression<Func<List<Address>, bool>> SameAddressesAs(List<Address> expectedAddresses)
        {
            return actualAddresses =>
                this.compareLogic.Compare(expectedAddresses, actualAddresses)
                    .AreEqual;
        }

        private static string GetRandomString() =>
           new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
           new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
          actualException => actualException.SameExceptionAs(expectedException);

        private static IQueryable<Address> CreateRandomAddresses(int count = 0)
        {
            if (count == 0)
            {
                count = GetRandomNumber();
            }

            return CreateAddressFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count)
                    .AsQueryable();
        }

        private static Address CreateRandomAddress(DateTimeOffset dateTimeOffset) =>
            CreateAddressFiller(dateTimeOffset).Create();

        private static Filler<Address> CreateAddressFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<Address>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(address => address.CreatedBy).Use(user)
                .OnProperty(address => address.CreatedBy).Use(user)
                .OnProperty(address => address.UpdatedBy).Use(user);

            return filler;
        }

        private static IQueryable<ResolvedAddress> CreateRandomResolvedAddresses()
        {
            return CreateResolvedAddressFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
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

        private static Address CreateRandomAddress() =>
           CreateRandomAddressFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static Filler<Address> CreateRandomAddressFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<Address>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(address => address.CreatedBy).Use(user)
                .OnProperty(address => address.UpdatedBy).Use(user);

            return filler;
        }

        public static TheoryData<Xeption> AddressExtractionOrchestrationDependencyValidationExceptions()
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

                new AddressNormalisationValidationException(
                    message: "Address normalisation validation error occured, please try again",
                    innerException),

                new AddressNormalisationDependencyValidationException(
                    message: "Address normalisation dependency validation error occurred, please try again.",
                    innerException),

                new CsvHelperClientValidationException(innerException),
            };
        }

        public static TheoryData<Xeption> AddressExtractionDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new AddressParserDependencyException(
                    message: "Address parser dependency error occurred, please contact support.",
                    innerException),

                new AddressParserServiceException(
                    message: "Address parser service error occurred, please contact support.",
                    innerException),

                new AddressNormalisationDependencyException(
                    message: "Address normalisation dependency error occurred, please contact support.",
                    innerException),

                new AddressNormalisationServiceException(
                    message: "Address normalisation service error occurred, please contact support.",
                    innerException),

                new CsvHelperClientDependencyException(innerException),
                new CsvHelperClientServiceException(innerException)
            };
        }
    }
}
