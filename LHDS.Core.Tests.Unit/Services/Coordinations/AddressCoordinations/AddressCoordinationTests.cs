// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Coordinations.AddressCoordinations;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Orchestrations.AddressExtractions.Exceptions;
using LHDS.Core.Models.Orchestrations.AddressPersistances.Exceptions;
using LHDS.Core.Services.Coordinations.AddressCoordinations;
using LHDS.Core.Services.Orchestrations.AddressExtractions;
using LHDS.Core.Services.Orchestrations.AddressPersistances;
using LHDS.Core.Services.Orchestrations.ResolvedAddresses;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.AddressCoordinations
{
    public partial class AddressCoordinationServiceTests
    {
        private readonly Mock<IAddressExtractionOrchestrationService> addressExtractionOrchestrationServiceMock;
        private readonly Mock<IAddressPersistanceOrchestrationService> addressPersistanceOrchestrationServiceMock;
        private readonly Mock<IResolvedAddressOrchestrationService> resolvedAddressOrchestrationServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly AddressConfiguration addressConfiguration;
        private readonly BlobContainers blobContainers;
        private readonly ICompareLogic compareLogic;
        private readonly IAddressCoordinationService addressCoordinationService;

        public AddressCoordinationServiceTests()
        {
            this.addressExtractionOrchestrationServiceMock = new Mock<IAddressExtractionOrchestrationService>();
            this.addressPersistanceOrchestrationServiceMock = new Mock<IAddressPersistanceOrchestrationService>();
            this.resolvedAddressOrchestrationServiceMock = new Mock<IResolvedAddressOrchestrationService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.compareLogic = new CompareLogic();

            this.addressConfiguration = new AddressConfiguration
            {
                InFolder = "In",
                ErrorFolder = "Error"
            };

            this.blobContainers = new BlobContainers
            {
                Addresses = "addresses"
            };

            this.addressCoordinationService = new AddressCoordinationService(
                addressExtractionOrchestrationService: addressExtractionOrchestrationServiceMock.Object,
                addressPersistanceOrchestrationService: addressPersistanceOrchestrationServiceMock.Object,
                resolvedAddressOrchestrationService: resolvedAddressOrchestrationServiceMock.Object,
                loggingBroker: loggingBrokerMock.Object,
                addressConfiguration: addressConfiguration,
                blobContainers: blobContainers);
        }

        private static string GetRandomString() =>
           new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static string GetRandomWordString() =>
           new MnemonicString(wordCount: 1).GetValue();

        private static string CreateRandomFileName()
        {
            string fileName = GetRandomWordString();

            for (int i = 0; i < 6; i++)
            {
                fileName += "/" + GetRandomWordString();
            }

            return fileName;
        }

        private static string CreateErrorFileName(string fileName, string errorFolder)
        {
            string[] splitFileName = fileName.Split('/');
            splitFileName[2] = errorFolder;
            string errorFileName = String.Join("/", splitFileName);

            return errorFileName;
        }

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

        private static List<ResolvedAddress> CreateRandomResolvedAddresses()
        {
            return CreateResolvedAddressFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .ToList();
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

        public static TheoryData<Xeption> AddressCoordinationDependencyValidationExceptions()
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

                new AddressPersistenceOrchestrationValidationException(
                    message: "Address persistance orchestration validation error occured, please try again",
                    innerException),

                new AddressPersistenceOrchestrationDependencyValidationException(
                    message: "Address persistance orchestration dependency validation error occurred, " +
                    "please try again.",
                    innerException),
            };
        }

        public static TheoryData<Xeption> AddressCoordinationDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new AddressExtractionOrchestrationDependencyException(
                    message: "Address extraction orchestration dependency error occurred, please try again.",
                    innerException),

                new AddressExtractionOrchestrationServiceException(
                    message: "Address extraction orchestration error occured, please try again",
                    innerException),

                new AddressPersistenceOrchestrationDependencyException(
                    message: "Address persistance orchestration dependency error occurred, please try again.",
                    innerException),

                new AddressPersistenceOrchestrationServiceException(
                    message: "Address persistance orchestration service error occured, please try again",
                    innerException),
            };
        }
    }
}
