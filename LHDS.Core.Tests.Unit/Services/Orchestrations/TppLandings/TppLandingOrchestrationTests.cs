// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FluentAssertions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.DateTimes;
using LHDS.Core.Brokers.Hashing;
using LHDS.Core.Brokers.Identifiers;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits.Exceptions;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Models.Foundations.IngestionTrackings.Exceptions;
using LHDS.Core.Models.Orchestrations.EmisLandings;
using LHDS.Core.Services.Orchestrations.Tpp;
using LHDS.Core.Services.Orchestrations.TppLandings;
using LHDS.Core.Services.Processings.DataSetSpecifications;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.IngestionTrackingAudits;
using LHDS.Core.Services.Processings.IngestionTrackings;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.TppLandings
{
    public partial class TppLandingOrchestrationTests
    {
        private readonly ITestOutputHelper output;
        private readonly Mock<IDocumentProcessingService> documentProcessingServiceMock;
        private readonly Mock<IIngestionTrackingProcessingService> ingestionTrackingProcessingServiceMock;
        private readonly Mock<IIngestionTrackingAuditProcessingService> ingestionTrackingProcessingAuditServiceMock;
        private readonly Mock<IDataSetSpecificationProcessingService> dataSetSpecificationProcessingServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<IIdentifierBroker> identifierBrokerMock;
        private readonly Mock<IHashBroker> hashBrokerMock;
        private readonly LandingConfiguration landingConfiguration;
        private readonly BlobContainers blobContainers;
        private readonly ITppLandingOrchestrationService tppOrchestrationService;
        private readonly ICompareLogic compareLogic;

        public TppLandingOrchestrationTests(ITestOutputHelper output)
        {
            this.output = output;
            documentProcessingServiceMock = new Mock<IDocumentProcessingService>();
            ingestionTrackingProcessingServiceMock = new Mock<IIngestionTrackingProcessingService>();
            ingestionTrackingProcessingAuditServiceMock = new Mock<IIngestionTrackingAuditProcessingService>();
            dataSetSpecificationProcessingServiceMock = new Mock<IDataSetSpecificationProcessingService>();
            loggingBrokerMock = new Mock<ILoggingBroker>();
            dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            identifierBrokerMock = new Mock<IIdentifierBroker>();
            hashBrokerMock = new Mock<IHashBroker>();
            compareLogic = new CompareLogic();

            landingConfiguration = new LandingConfiguration
            {
                LandingSupplierId = Guid.NewGuid(),
                DecryptedFolder = "inbox/landing"
            };

            blobContainers = new BlobContainers
            {
                EmisLanding = "emislanding",
                Versioner = "versioner",
                Ingress = "ingress",
                OptOut = "optout",
                Pds = "pds",
                TppLanding = "tpplanding"
            };

            tppOrchestrationService = new TppLandingOrchestrationService(
                documentProcessingService: documentProcessingServiceMock.Object,
                ingestionTrackingProcessingService: ingestionTrackingProcessingServiceMock.Object,
                ingestionTrackingProcessingAuditService: ingestionTrackingProcessingAuditServiceMock.Object,
                dataSetSpecificationProcessingService: dataSetSpecificationProcessingServiceMock.Object,
                blobContainers,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object,
                identifierBroker: identifierBrokerMock.Object,
                hashBroker: hashBrokerMock.Object,
                landingConfiguration: landingConfiguration);
        }

        public byte[] CreateRandomData()
        {
            string randomMessage = GetRandomString();
            return Encoding.UTF8.GetBytes(randomMessage);
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString() =>
          new MnemonicString().GetValue();

        private static List<string> GetRandomStrings() =>
            Enumerable.Range(start: 1, count: GetRandomNumber())
                .Select(index => GetRandomString())
                .ToList();

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static DataSet CreateRandomDataSet(Guid supplierId) =>
            CreateDataSetFiller(supplierId).Create();

        private static Filler<DataSet> CreateDataSetFiller(Guid supplierId)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<DataSet>();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(dataSet => dataSet.SupplierId).Use(supplierId)
                .OnProperty(dataSet => dataSet.IsActive).Use(true)
                .OnProperty(dataSet => dataSet.ActiveFrom).Use(now.AddDays(-2))
                .OnProperty(dataSet => dataSet.ActiveTo).Use(now.AddDays(2))
                .OnProperty(dataSet => dataSet.CreatedBy).Use(user)
                .OnProperty(dataSet => dataSet.UpdatedBy).Use(user)
                .OnProperty(dataSet => dataSet.ActiveTo).Use(now.AddDays(GetRandomNumber()));

            return filler;
        }

        private static DataSetSpecification CreateRandomDataSetSpecification(DataSet dataSet) =>
            CreateDataSetSpecificationFiller(dataSet).Create();

        private static Filler<DataSetSpecification> CreateDataSetSpecificationFiller(DataSet dataSet)
        {
            string user = GetRandomString(255);
            var filler = new Filler<DataSetSpecification>();
            var now = DateTimeOffset.UtcNow;

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)

                .OnProperty(dataSetSpecification => dataSetSpecification.DataSetId).Use(dataSet.Id)
                .OnProperty(dataSetSpecification => dataSetSpecification.DataSet).Use(dataSet)
                .OnProperty(dataSetSpecification => dataSetSpecification.IsActive).Use(true)
                .OnProperty(dataSetSpecification => dataSetSpecification.ActiveFrom).Use(now.AddDays(-2))
                .OnProperty(dataSetSpecification => dataSetSpecification.ActiveTo).Use(now.AddDays(2))

                .OnProperty(dataSetSpecification =>
                    dataSetSpecification.OurSpecificationVersion).Use(GetRandomString(10))

                .OnProperty(dataSetSpecification =>
                    dataSetSpecification.SupplierSpecificationVersion).Use(GetRandomString(10))

                .OnProperty(dataSetSpecification => dataSetSpecification.PresededById).IgnoreIt()
                .OnProperty(dataSetSpecification => dataSetSpecification.SupersededById).IgnoreIt()
                .OnProperty(dataSetSpecification => dataSetSpecification.CreatedBy).Use(user)
                .OnProperty(dataSetSpecification => dataSetSpecification.CreatedBy).Use(user)
                .OnProperty(dataSetSpecification => dataSetSpecification.UpdatedBy).Use(user);

            return filler;
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
          actualException => actualException.SameExceptionAs(expectedException);

        public static TheoryData<Xeption> TppDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DocumentValidationException(
                    message: "Document validation errors occured, please try again",
                    innerException),

                new DocumentDependencyValidationException(
                    message: "Document dependency validation occurred, please try again.",
                    innerException),

                new IngestionTrackingValidationException(
                    message: "Ingestion tracking validation errors occurred, fix the errors and try again.",
                    innerException),

                new IngestionTrackingDependencyValidationException(
                    message: "Ingestion tracking dependency validation occurred, please try again.",
                    innerException),

                new IngestionTrackingAuditValidationException(
                    message: "Audit validation errors occurred, please try again.",
                    innerException),

                new IngestionTrackingAuditDependencyValidationException(
                    message: "Audit dependency validation occurred, please try again.",
                    innerException)
            };
        }

        public static TheoryData<Xeption> TppDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DocumentDependencyException(
                    message: "Document dependency error occurred, please contact support.",
                    innerException),

                new DocumentServiceException(
                    message: "Document service error occurred, please contact support.",
                    innerException),

                new IngestionTrackingDependencyException(
                    message: "Failed ingestion tracking storage error occurred, please contact support.",
                    innerException),

                new IngestionTrackingServiceException(
                    message: "Ingestion tracking service error occurred, please contact support.",
                    innerException),

                new IngestionTrackingAuditDependencyException(
                    message: "Audit dependency error occurred, please contact support.",
                    innerException),

                new IngestionTrackingAuditServiceException(
                    message: "Audit service error occurred, please contact support.",
                    innerException)
            };
        }

        private Expression<Func<IngestionTracking, bool>> SameIngestionTrackingAs(
            IngestionTracking expectedIngestionTracking)
        {
            return actualIngestionTracking =>
                CompareObjects(expectedIngestionTracking, actualIngestionTracking);
        }

        private bool CompareObjects(object expected, object actual)
        {
            try
            {
                actual.Should().BeEquivalentTo(expected);
            }
            catch (Exception exception)
            {
                output.WriteLine(exception.Message);
            }

            return this.compareLogic.Compare(expected, actual)
                    .AreEqual;
        }

        private List<string> GetRandomTppFileNames(string resourceGroup, object batch, int count)
        {
            return Enumerable.Range(start: 1, count: count)
                .Select(index => $"{resourceGroup}/{batch}/{GetRandomString()}.csv")
                .ToList();
        }

        private static IngestionTracking CreateRandomIngestionTracking(DateTimeOffset dateTimeOffset) =>
            CreateIngestionTrackingFiller(dateTimeOffset).Create();

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(ingestionTracking => ingestionTracking.Supplier).IgnoreIt()
                .OnProperty(ingestionTracking => ingestionTracking.IngestionTrackingAudits).IgnoreIt()
                .OnProperty(ingestionTracking => ingestionTracking.SubscriberAgreement).IgnoreIt();

            return filler;
        }

        private static List<IngestionTracking> CreateRandomIngestionTrackings(
            DateTimeOffset dateTimeOffset,
            List<string> fileNames,
            Guid supplierId)
        {
            List<IngestionTracking> items = new List<IngestionTracking>();

            foreach (var fileName in fileNames)
            {
                items.Add(CreateIngestionTrackingFiller(
                    dateTimeOffset,
                    fileName: fileName,
                    supplierId).Create());
            }

            return items;
        }

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller(
            DateTimeOffset dateTimeOffset,
            string fileName,
            Guid supplierId)
        {
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnProperty(ingestionTracking => ingestionTracking.FileName).Use(fileName)
                .OnProperty(ingestionTracking => ingestionTracking.SupplierId).Use(supplierId)
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(ingestionTracking => ingestionTracking.Supplier).IgnoreIt()
                .OnProperty(ingestionTracking => ingestionTracking.IngestionTrackingAudits).IgnoreIt()
                .OnProperty(ingestionTracking => ingestionTracking.SubscriberAgreement).IgnoreIt();

            return filler;
        }
    }
}