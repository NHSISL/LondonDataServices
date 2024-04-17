// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using LHDS.Core.Models.Foundations.AddressExtractionAudits.Exceptions;
using LHDS.Core.Models.Foundations.AddressParsers.Exceptions;
using LHDS.Core.Services.Foundations.AddressExtractionAudits;
using LHDS.Core.Services.Foundations.AddressParsers;
using LHDS.Core.Services.Orchestrations.AddressExtractions;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExctractionOrchestrationServiceTests
    {
        private readonly Mock<IAddressParserService> addressParserServiceMock;
        private readonly Mock<IAddressParserService> addressParserServiceMock;
        private readonly Mock<IAddressParserService> addressParserServiceMock;
        private readonly Mock<IAddressExtractionAuditService> addressExtractionAuditServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<IIdentifierBroker> identifierBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly IAddressExtractionOrchestrationService addressExtractionOrchestrationService;
        private readonly ITestOutputHelper output;

        public AddressExctractionOrchestrationServiceTests(ITestOutputHelper output)
        {
            this.addressParserServiceMock = new Mock<IAddressParserService>();
            this.addressExtractionAuditServiceMock = new Mock<IAddressExtractionAuditService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.identifierBrokerMock = new Mock<IIdentifierBroker>();
            this.compareLogic = new CompareLogic();
            this.output = output;

            this.addressExtractionOrchestrationService = new AddressExtractionOrchestrationService(
                addressParserService: addressParserServiceMock.Object,
                addressExtractionAuditService: addressExtractionAuditServiceMock.Object,
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

        private Expression<Func<AddressExtractionAudit, bool>> SameAddressExtractionAuditAs(
            AddressExtractionAudit expectedAddressExtractionAudit)
        {
            return actualAddressExtractionAudit =>
                this.compareLogic.Compare(expectedAddressExtractionAudit, actualAddressExtractionAudit)
                    .AreEqual;
        }

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
            var filler = new Filler<Address>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
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
                    message: "Address normalisation processing validation error occured, please try again",
                    innerException),

                new AddressParserDependencyValidationException(
                    message: "Address normalisation processing dependency validation error occurred, please try again.",
                    innerException),

                new AddressExtractionAuditValidationException(
                    message: "Audit validation error occurred, please try again.",
                    innerException),

                new AddressExtractionAuditDependencyValidationException(
                    message: "Audit dependency validation error occurred, please try again.",
                    innerException)
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

                new AddressExtractionAuditDependencyException(
                    message: "Audit dependency error occurred, contact support.",
                    innerException),

                new AddressExtractionAuditServiceException(
                    message: "Audit service error occurred, contact support.",
                    innerException)
            };
        }
    }
}
