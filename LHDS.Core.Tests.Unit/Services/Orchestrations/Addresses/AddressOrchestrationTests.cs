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
using LHDS.Core.Brokers.Files;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.Addresses.Exceptions;
using LHDS.Core.Models.Foundations.Assigns.Exceptions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Orchestrations.Addresses;
using LHDS.Core.Services.Orchestrations.Addresses;
using LHDS.Core.Services.Processings.Addresses;
using LHDS.Core.Services.Processings.Assigns;
using Moq;
using NHSISL.CsvHelperClient.Models.Clients.CsvHelpers.Exceptions;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Addresses
{
    public partial class AddressOrchestrationServiceTests
    {
        private readonly Mock<IAddressProcessingService> addressProcessingServiceMock;
        private readonly Mock<IAssignProcessingService> assignProcessingServiceMock;
        private readonly Mock<IAuditBroker> auditBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<IIdentifierBroker> identifierBrokerMock;
        private readonly Mock<ICsvHelperBroker> csvHelperBrokerMock;
        private readonly Mock<IFileBroker> fileBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly IAddressOrchestrationService addressOrchestrationService;
        private readonly ITestOutputHelper output;

        public AddressOrchestrationServiceTests(ITestOutputHelper output)
        {
            this.addressProcessingServiceMock = new Mock<IAddressProcessingService>();
            this.assignProcessingServiceMock = new Mock<IAssignProcessingService>();
            this.auditBrokerMock = new Mock<IAuditBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.csvHelperBrokerMock = new Mock<ICsvHelperBroker>();
            this.fileBrokerMock = new Mock<IFileBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.identifierBrokerMock = new Mock<IIdentifierBroker>();
            this.compareLogic = new CompareLogic();
            this.output = output;

            this.addressOrchestrationService = new AddressOrchestrationService(
                addressProcessingService: addressProcessingServiceMock.Object,
                assignProcessingService: assignProcessingServiceMock.Object,
                fileBroker: fileBrokerMock.Object,
                csvHelperBroker: csvHelperBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object,
                auditBroker: auditBrokerMock.Object,
                loggingBroker: loggingBrokerMock.Object);
        }

        private Expression<Func<Address, bool>> SameAddressAs(Address expectedAddress)
        {
            return actualAddress =>
                this.compareLogic.Compare(expectedAddress, actualAddress)
                    .AreEqual;
        }

        private Expression<Func<LPIAddress, bool>> SameLPIAddressAs(LPIAddress expectedAddress)
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

        private static List<StreetDescriptor> CreateRandomStreetDescriptors(int count = 0)
        {
            if (count == 0)
            {
                count = GetRandomNumber();
            }

            return CreateStreetDescriptorFiller()
                .Create(count)
                    .ToList();
        }

        private static StreetDescriptor CreateRandomStreetDescriptor() =>
            CreateStreetDescriptorFiller().Create();

        private static Filler<StreetDescriptor> CreateStreetDescriptorFiller() =>
            new Filler<StreetDescriptor>();

        private static List<LPIAddress> CreateRandomLPIAddresses(int count = 0)
        {
            if (count == 0)
            {
                count = GetRandomNumber();
            }

            return CreateLPIAddressFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count)
                    .ToList();
        }

        private static LPIAddress CreateRandomLPIAddress(DateTimeOffset dateTimeOffset) =>
            CreateLPIAddressFiller(dateTimeOffset).Create();

        private static Filler<LPIAddress> CreateLPIAddressFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<LPIAddress>();

            filler.Setup()
                .OnType<DateTimeOffset?>().Use(dateTimeOffset);

            return filler;
        }

        private static List<BLPUAddress> CreateRandomBLPUAddresses(int count = 0)
        {
            if (count == 0)
            {
                count = GetRandomNumber();
            }

            return CreateBLPUAddressFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count)
                    .ToList();
        }

        private static BLPUAddress CreateRandomBLPUAddress(DateTimeOffset dateTimeOffset) =>
            CreateBLPUAddressFiller(dateTimeOffset).Create();

        private static Filler<BLPUAddress> CreateBLPUAddressFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<BLPUAddress>();
            filler.Setup().OnType<DateTimeOffset?>().Use(dateTimeOffset);

            return filler;
        }

        private static List<string> CreateRandomStringList() =>
            Enumerable.Range(1, GetRandomNumber()).Select(x => GetRandomString()).ToList();

        public static TheoryData<Xeption> AddressOrchestrationDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new AddressValidationException(
                    message: "Address parser validation error occured, please try again",
                    innerException),

                new AddressDependencyValidationException(
                    message: "Address parser dependency validation error occurred, please try again.",
                    innerException),

                new AssignValidationException(
                    message: "Assign validation error occured, please try again",
                    innerException),

                new AssignDependencyValidationException(
                    message: "Assign dependency validation error occurred, please try again.",
                    innerException),

                new CsvHelperClientValidationException(innerException),
            };
        }

        public static TheoryData<Xeption> AddressDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new AddressDependencyException(
                    message: "Address dependency error occurred, please contact support.",
                    innerException),

                new AddressServiceException(
                    message: "Address service error occurred, please contact support.",
                    innerException),

                new AssignDependencyException(
                    message: "Assign dependency error occurred, please contact support.",
                    innerException),

                new AssignServiceException(
                    message: "Assign service error occurred, please contact support.",
                    innerException),

                new CsvHelperClientDependencyException(innerException),
                new CsvHelperClientServiceException(innerException)
            };
        }
    }
}
