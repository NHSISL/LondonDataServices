// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.Audits;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Brokers.Securities;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AssignAddresses;
using LHDS.Core.Models.Foundations.Audits;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using LHDS.Core.Models.Processings.ResolvedAddresses.Exceptions;
using LHDS.Core.Services.Orchestrations.ResolvedAddresses;
using LHDS.Core.Services.Processings.Addresses;
using LHDS.Core.Services.Processings.Assigns;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.ResolvedAddresses;
using Moq;
using NHSISL.CsvHelperClient.Models.Clients.CsvHelpers.Exceptions;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationTests
    {
        private readonly Mock<IDocumentProcessingService> documentProcessingServiceMock;
        private readonly Mock<IResolvedAddressProcessingService> resolvedAddressProcessingServiceMock;
        private readonly Mock<IAssignProcessingService> assignProcessingServiceMock;
        private readonly Mock<IAddressProcessingService> addressProcessingServiceMock;
        private readonly Mock<IAuditBroker> auditBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<ICsvHelperBroker> csvHelperBrokerMock;
        private readonly Mock<IIdentifierBroker> identifierBrokerMock;
        private readonly Mock<ISecurityBroker> securityBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly BlobContainers blobContainers;
        private readonly IResolvedAddressOrchestrationService resolvedAddressOrchestrationService;
        private readonly ITestOutputHelper output;

        public ResolvedAddressOrchestrationTests(ITestOutputHelper output)
        {
            this.documentProcessingServiceMock = new Mock<IDocumentProcessingService>();
            this.resolvedAddressProcessingServiceMock = new Mock<IResolvedAddressProcessingService>();
            this.assignProcessingServiceMock = new Mock<IAssignProcessingService>();
            this.addressProcessingServiceMock = new Mock<IAddressProcessingService>();
            this.auditBrokerMock = new Mock<IAuditBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.csvHelperBrokerMock = new Mock<ICsvHelperBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.identifierBrokerMock = new Mock<IIdentifierBroker>();
            this.securityBrokerMock = new Mock<ISecurityBroker>();
            this.compareLogic = new CompareLogic();
            this.output = output;

            this.blobContainers = new BlobContainers
            {
                Addresses = "addresses"
            };

            this.resolvedAddressOrchestrationService = new ResolvedAddressOrchestrationService(
                documentProcessingService: this.documentProcessingServiceMock.Object,
                resolvedAddressProcessingService: this.resolvedAddressProcessingServiceMock.Object,
                assignProcessingService: this.assignProcessingServiceMock.Object,
                addressProcessingService: this.addressProcessingServiceMock.Object,
                auditBroker: this.auditBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                csvHelperBroker: this.csvHelperBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                identifierBroker: this.identifierBrokerMock.Object,
                securityBroker: this.securityBrokerMock.Object,
                blobContainers: blobContainers);
        }

        private Expression<Func<Stream, bool>> SameStreamAs(Stream expectedStream)
        {
            return actualStream =>
                IsSameStream(expectedStream, actualStream);
        }

        private static bool IsSameStream(Stream expectedStream, Stream actualStream)
        {
            byte[] expectedBytes = ReadAllBytesFromStream(expectedStream);
            byte[] actualBytes = ReadAllBytesFromStream(actualStream);

            return new CompareLogic().Compare(expectedBytes, actualBytes).AreEqual;
        }

        private Expression<Func<ResolvedAddress, bool>> SameResolvedAddressAs(
           ResolvedAddress expectedResolvedAddress)
        {
            return actualResolvedAddress =>
                this.compareLogic.Compare(expectedResolvedAddress, actualResolvedAddress)
                    .AreEqual;
        }

        private Expression<Func<AssignAddress, bool>> SameAssignAddressAs(
           AssignAddress expectedAssignAddress)
        {
            return actualAssignAddress =>
                this.compareLogic.Compare(expectedAssignAddress, actualAssignAddress)
                    .AreEqual;
        }

        private Expression<Func<List<ResolvedAddress>, bool>> SameResolvedAddressListAs(
           List<ResolvedAddress> expectedResolvedAddressList)
        {
            return actualResolvedAddressList =>
                this.compareLogic.Compare(expectedResolvedAddressList, actualResolvedAddressList).AreEqual;
        }

        private static byte[] ReadAllBytesFromStream(Stream stream)
        {
            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomStringWithLengthOf(int length)
        {
            string result = new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

            return result.Length > length ? result.Substring(0, length) : result;
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static List<ResolvedAddress> CreateRandomResolvedAddresses()
        {
            return CreateResolvedAddressFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .ToList();
        }

        private EntraUser CreateRandomEntraUser(string entraUserId = "")
        {
            var userId = string.IsNullOrWhiteSpace(entraUserId) ? GetRandomStringWithLengthOf(255) : entraUserId;

            return new EntraUser(
                entraUserId: userId,
                givenName: GetRandomString(),
                surname: GetRandomString(),
                displayName: GetRandomString(),
                email: GetRandomString(),
                jobTitle: GetRandomString(),
                roles: new List<string> { GetRandomString() },

                claims: new List<System.Security.Claims.Claim>
                {
                    new System.Security.Claims.Claim(type: GetRandomString(), value: GetRandomString())
                });
        }

        private Expression<Func<List<ResolvedAddressReturn>, bool>> SameResolvedAddressReturnsAs(
            List<ResolvedAddressReturn> expectedResolvedAddressReturns)
        {
            return actualResolvedAddressReturns =>
                this.compareLogic.Compare(expectedResolvedAddressReturns, actualResolvedAddressReturns)
                    .AreEqual;
        }

        private Expression<Func<Document, bool>> SameDocumentAs(Document expectedDocument)
        {
            return actualDocument =>
                this.compareLogic.Compare(expectedDocument, actualDocument)
                    .AreEqual;
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

        private static List<ResolvedAddress> CreateRandomUnmatchedAddresses(int count)
        {
            var fillers = Enumerable.Range(1, count)
                .Select(_ => CreateUnmatchedAddressFiller())
                    .ToList();

            var result = fillers.Select(filler => filler.Create()).ToList();

            return result.ToList();
        }

        private static Filler<ResolvedAddress> CreateUnmatchedAddressFiller()
        {
            string user = Guid.NewGuid().ToString();
            DateTimeOffset dateTimeOffset = DateTimeOffset.Now;

            var filler = new Filler<ResolvedAddress>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(resolvedAddress => resolvedAddress.IsProcessed).Use(false)
                .OnProperty(resolvedAddress => resolvedAddress.IsProcessing).Use(false)
                .OnProperty(resolvedAddress => resolvedAddress.IsExported).Use(false)
                .OnProperty(resolvedAddress => resolvedAddress.RetryCount).Use(0)
                .OnProperty(resolvedAddress => resolvedAddress.CreatedBy).Use(user)
                .OnProperty(resolvedAddress => resolvedAddress.UpdatedBy).Use(user);

            return filler;
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

        private static AssignAddress CreateRandomAssignAddress(DateTimeOffset dateTimeOffset) =>
            CreateAssignAddressFiller(dateTimeOffset).Create();

        private static Filler<AssignAddress> CreateAssignAddressFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<AssignAddress>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset);

            return filler;
        }

        private static List<ResolvedAddressReturn> MapToResolvedAddressReturn(List<ResolvedAddress> resolvedAddresses)
        {
            List<ResolvedAddressReturn> returnAddresses = resolvedAddresses.Select(resolvedAddress =>
                    new ResolvedAddressReturn
                    {
                        UniqueReference = resolvedAddress.UniqueReference,
                        UPRN = resolvedAddress.UPRN,
                        UPSN = resolvedAddress.UPSN,
                        OrganisationName = resolvedAddress.OrganisationName,
                        DepartmentName = resolvedAddress.DepartmentName,
                        SubBuildingName = resolvedAddress.SubBuildingName,
                        BuildingName = resolvedAddress.BuildingName,
                        BuildingNumber = resolvedAddress.BuildingNumber,
                        DependentThoroughfare = resolvedAddress.DependentThoroughfare,
                        Thoroughfare = resolvedAddress.Thoroughfare,
                        DoubleDependentLocality = resolvedAddress.DoubleDependentLocality,
                        DependentLocality = resolvedAddress.DependentLocality,
                        PostTown = resolvedAddress.PostTown,
                        PostCode = resolvedAddress.PostCode,
                    }).ToList();

            return returnAddresses;
        }

        public static TheoryData<Xeption> DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DocumentProcessingValidationException(
                    message: "Document processing validation error occured, please try again",
                    innerException),

                new DocumentProcessingDependencyValidationException(
                    message: "Document processing dependency validation error occurred, please try again.",
                    innerException),

                new ResolvedAddressProcessingValidationException(
                    message: "Resolved address processing validation error occured, please try again",
                    innerException),

                new ResolvedAddressProcessingDependencyValidationException(
                    message: "Resolved address processing dependency validation error occurred, please try again.",
                    innerException),

                new CsvHelperClientValidationException(innerException),
            };
        }

        public static TheoryData<Xeption> DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DocumentProcessingDependencyException(
                    message: "Document processing dependency error occurred, please contact support.",
                    innerException),

                new DocumentProcessingServiceException(
                    message: "Document processing service error occurred, please contact support.",
                    innerException),

                new ResolvedAddressProcessingDependencyException(
                    message: "Resolved address processing dependency error occurred, please contact support.",
                    innerException),

                new ResolvedAddressProcessingServiceException(
                    message: "Resolved address processing service error occurred, please contact support.",
                    innerException),

                new CsvHelperClientDependencyException(innerException),
                new CsvHelperClientServiceException(innerException)
            };
        }

    }
}
