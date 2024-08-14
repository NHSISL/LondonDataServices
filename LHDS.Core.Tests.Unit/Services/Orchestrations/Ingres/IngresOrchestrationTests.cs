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
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.IngestionTrackings;
using LHDS.Core.Services.Orchestrations.Ingress;
using LHDS.Core.Services.Processings.Documents;
using LHDS.Core.Services.Processings.IngestionTrackings;
using LHDS.Core.Services.Processings.SpecificationObjects;
using Moq;
using Tynamix.ObjectFiller;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Ingres
{
    public partial class IngresOrchestrationTests
    {
        private readonly Mock<IIngestionTrackingProcessingService> ingestionTrackingProcessingServiceMock;
        private readonly Mock<ISpecificationObjectProcessingService> specificationObjectProcessingServiceMock;
        private readonly Mock<IDocumentProcessingService> documentProcessingServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IAuditBroker> auditBrokerMock;
        private readonly IIngressOrchestrationService ingressOrchestrationService;
        private readonly ITestOutputHelper output;

        public IngresOrchestrationTests(ITestOutputHelper output)
        {
            this.output = output;
            this.ingestionTrackingProcessingServiceMock = new Mock<IIngestionTrackingProcessingService>();
            this.specificationObjectProcessingServiceMock = new Mock<ISpecificationObjectProcessingService>();
            this.documentProcessingServiceMock = new Mock<IDocumentProcessingService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.auditBrokerMock = new Mock<IAuditBroker>();

            this.ingressOrchestrationService = new IngressOrchestrationService(
                ingestionTrackingProcessingService: this.ingestionTrackingProcessingServiceMock.Object,
                specificationObjectProcessingService: this.specificationObjectProcessingServiceMock.Object,
                documentProcessingService: this.documentProcessingServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                auditBroker: this.auditBrokerMock.Object);
        }

        private Expression<Func<Stream, bool>> SameStreamAs(Stream expectedStream)
        {
            return actualStream =>
                IsSameStream(expectedStream, actualStream);
        }

        private bool IsSameStream(Stream expectedStream, Stream actualStream)
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

        private static string CreateRandomDecryptedFilePath() =>
            $"/{GetRandomString()}/{DateTime.UtcNow.ToString("yyyyMMdd_HHmm")}/{GetRandomString()}.csv";

        private static string GetRandomString() =>
            new MnemonicString(wordCount: 1, wordMinLength: 1, wordMaxLength: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static List<string> GetRandomStringList()
        {
            return Enumerable.Range(start: 0, count: GetRandomNumber())
                .Select(number => GetRandomString())
                    .ToList();
        }

        private static IngestionTracking CreateRandomIngestionTracking() =>
            CreateIngestionTrackingFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static Filler<IngestionTracking> CreateIngestionTrackingFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<IngestionTracking>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(ingestionTracking => ingestionTracking.CreatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.UpdatedBy).Use(user)
                .OnProperty(ingestionTracking => ingestionTracking.IngestionTrackingAudits).IgnoreIt()
                .OnProperty(ingestionTracking => ingestionTracking.Supplier).IgnoreIt();

            return filler;
        }
    }
}
