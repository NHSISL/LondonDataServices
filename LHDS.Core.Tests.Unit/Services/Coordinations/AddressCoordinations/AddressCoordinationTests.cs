// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Orchestrations.AddressExtractions.Exceptions;
using LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions;
using LHDS.Core.Services.Coordinations.AddressCoordinations;
using LHDS.Core.Services.Orchestrations.AddressExtractions;
using LHDS.Core.Services.Orchestrations.AddressNormalisations;
using LHDS.Core.Services.Orchestrations.AddressPersistances;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.AddressCoordinations
{
    public partial class AddressCoordinationServiceTests
    {
        private readonly Mock<IAddressExtractionOrchestrationService> addressExtractionOrchestrationServiceMock;
        private readonly Mock<IAddressNormalisationOrchestrationService> addressNormalisationOrchestrationServiceMock;
        private readonly Mock<IAddressPersistanceOrchestrationService> addressPersistanceOrchestrationServiceMock;
        private readonly Mock<IResolvedAddressOrchestrationService> resolvedAddressOrchestrationServiceMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly IAddressCoordinationService addressCoordinationService;

        public AddressCoordinationServiceTests()
        {
            this.addressExtractionOrchestrationServiceMock = new Mock<IAddressExtractionOrchestrationService>();
            this.addressNormalisationOrchestrationServiceMock = new Mock<IAddressNormalisationOrchestrationService>();
            this.addressPersistanceOrchestrationServiceMock = new Mock<IAddressPersistanceOrchestrationService>();
            this.resolvedAddressOrchestrationServiceMock = new Mock<IResolvedAddressOrchestrationService>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.addressCoordinationService = new AddressCoordinationService(
                addressExtractionOrchestrationService: addressExtractionOrchestrationServiceMock.Object,
                addressNormalisationOrchestrationService: addressNormalisationOrchestrationServiceMock.Object,
                addressPersistanceOrchestrationService: addressPersistanceOrchestrationServiceMock.Object,
                resolvedAddressOrchestrationService: resolvedAddressOrchestrationServiceMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object,
                loggingBroker: loggingBrokerMock.Object);
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

        public static TheoryData AddressCoordinationDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new AddressExtractionValidationOrchestrationException(
                    message: "Address extraction orchestration validation error occured, please try again",
                    innerException),

                new AddressExtractionOrchestrationDependencyValidationException(
                    message: "Address extraction orchestration dependency validation error occurred, please try again.",
                    innerException),

                new AddressPersistanceOrchestrationValidationException(
                    message: "Address persistance orchestration validation error occured, please try again",
                    innerException),

                new AddressPersistanceOrchestrationDependencyValidationException(
                    message: "Address persistance orchestration dependency validation error occurred, " +
                    "please try again.",
                    innerException),
            };
        }

        public static TheoryData AddressCoordinationDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new AddressExtractionOrchestrationDependencyException(
                    message: "Address extraction orchestration dependency validation error occurred, please try again.",
                    innerException),

                new AddressExtractionOrchestrationServiceException(
                    message: "Address extraction orchestration error occured, please try again",
                    innerException),

                new AddressPersistanceOrchestrationDependencyException(
                    message: "Address persistance orchestration dependency error occurred, please try again.",
                    innerException),

                new AddressPersistanceOrchestrationServiceException(
                    message: "Address persistance orchestration service error occured, please try again",
                    innerException),
            };
        }
    }
}
