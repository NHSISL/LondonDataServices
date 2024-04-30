// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Processings.Addresses.Exceptions;
using LHDS.Core.Services.Orchestrations.AddressPersistances;
using LHDS.Core.Services.Processings.Addresses;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressPersistances
{
    public partial class AddressPersistanceOrchestrationServiceTests
    {
        private readonly Mock<IAddressProcessingService> addressProcessingServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly IAddressPersistanceOrchestrationService addressPersistanceOrchestrationService;

        public AddressPersistanceOrchestrationServiceTests()
        {
            this.addressProcessingServiceMock = new Mock<IAddressProcessingService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.compareLogic = new CompareLogic();

            this.addressPersistanceOrchestrationService = new AddressPersistanceOrchestrationService(
                addressProcessingService: addressProcessingServiceMock.Object,
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

        public static TheoryData AddressPersistenceDependencyValidationExceptions()
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
            };
        }

        public static TheoryData AddressPersistenceDependencyExceptions()
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
                    message: "Address processing service error occurred, contact support.",
                    innerException),
            };
        }
    }
}
