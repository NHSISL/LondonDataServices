// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Processings.Documents.Exceptions;
using LHDS.Core.Models.Processings.ResolvedAddresses.Exceptions;
using LHDS.Core.Services.Orchestrations.ResolvedAddresses;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.ResolvedAddresses;
using Moq;
using NHSISL.CsvHelperClient.Models.Clients.CsvHelpers.Exceptions;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.ResolvedAddresses
{
    public partial class ResolvedAddressOrchestrationTests
    {
        private readonly Mock<IDocumentProcessingService> documentProcessingServiceMock;
        private readonly Mock<IResolvedAddressProcessingService> resolvedAddressProcessingServiceMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<ICsvHelperBroker> csvHelperBrokerMock;
        private readonly Mock<IIdentifierBroker> identifierBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly BlobContainers blobContainers;
        private readonly IResolvedAddressOrchestrationService resolvedAddressOrchestrationService;

        public ResolvedAddressOrchestrationTests()
        {
            this.documentProcessingServiceMock = new Mock<IDocumentProcessingService>();
            this.resolvedAddressProcessingServiceMock = new Mock<IResolvedAddressProcessingService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.csvHelperBrokerMock = new Mock<ICsvHelperBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.identifierBrokerMock = new Mock<IIdentifierBroker>();
            this.compareLogic = new CompareLogic();

            this.blobContainers = new BlobContainers
            {
                Addresses = "addresses"
            };

            this.resolvedAddressOrchestrationService = new ResolvedAddressOrchestrationService(
                documentProcessingService: this.documentProcessingServiceMock.Object,
                resolvedAddressProcessingService: this.resolvedAddressProcessingServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                csvHelperBroker: this.csvHelperBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                identifierBroker: this.identifierBrokerMock.Object,
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
