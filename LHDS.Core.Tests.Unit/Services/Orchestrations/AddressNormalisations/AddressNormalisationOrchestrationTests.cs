// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions;
using LHDS.Core.Services.Foundations.AddressParsers;
using LHDS.Core.Services.Orchestrations.AddressNormalisations;
using LHDS.Core.Services.Processings.AddressLoadingAudits;
using LHDS.Core.Services.Processings.AddressNormalisations;
using LHDS.Core.Services.Processings.AddressParsers;
using Microsoft.EntityFrameworkCore;
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
        private readonly Mock<IAddressLoadingAuditProcessingService> addressLoadingAuditProcessingServiceMock;
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
            this.addressLoadingAuditProcessingServiceMock = new Mock<IAddressLoadingAuditProcessingService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.identifierBrokerMock = new Mock<IIdentifierBroker>();
            this.compareLogic = new CompareLogic();
            this.output = output;

            this.addressNormalisationOrchestrationService = new AddressNormalisationOrchestrationService(
                addressParserProcessingService: addressParserProcessingServiceMock.Object,
                addressNormalisationProcessingService: addressNormalisationProcessingServiceMock.Object,
                addressLoadingAuditProcessingService: addressLoadingAuditProcessingServiceMock.Object,
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

        private static AddressLoadingAudit CreateRandomAddressLoadingAudit(DateTimeOffset dateTimeOffset) =>
           CreateAddressLoadingAuditFiller(dateTimeOffset).Create();

        private static Filler<AddressLoadingAudit> CreateAddressLoadingAuditFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<AddressLoadingAudit>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(addressLoadingAudit => addressLoadingAudit.CreatedBy).Use(user)
                .OnProperty(addressLoadingAudit => addressLoadingAudit.UpdatedBy).Use(user);

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


    }
}
