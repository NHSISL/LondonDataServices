// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.CsvHelpers;
using LHDS.Core.Brokers.Hashing;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Decisions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Decisions;
using LHDS.Core.Models.Foundations.Decisions.Exceptions;
using LHDS.Core.Models.Foundations.DecisionTypes;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.Patients;
using LHDS.Core.Services.Foundations.Decisions;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Orchestrations.Decisions;
using Moq;
using NHSISL.CsvHelperClient.Models.Clients.CsvHelpers.Exceptions;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;


namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Decisions
{
    public partial class DecisionOrchestrationServiceTests
    {
        private readonly ITestOutputHelper output;
        private readonly ICompareLogic compareLogic;
        private readonly Mock<IDecisionService> decisionServiceMock;
        private readonly Mock<IDocumentService> documentServiceMock;
        private readonly Mock<ICsvHelperBroker> csvHelperBrokerMock;
        private readonly Mock<IHashBroker> hashBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly BlobContainers blobContainers;
        private readonly DecisionConfiguration decisionConfiguration;
        private readonly IDecisionOrchestrationService decisionOrchestrationService;

        public DecisionOrchestrationServiceTests(ITestOutputHelper output)
        {
            this.output = output;
            this.compareLogic = new CompareLogic();
            this.decisionServiceMock = new Mock<IDecisionService>();
            this.documentServiceMock = new Mock<IDocumentService>();
            this.csvHelperBrokerMock = new Mock<ICsvHelperBroker>();
            this.hashBrokerMock = new Mock<IHashBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.blobContainers = new BlobContainers
            {
                Ingress = "ingress"
            };

            this.decisionConfiguration = new DecisionConfiguration
            {
                HashPepper = "hashPepper",
                FolderName = "localdataoptout",
                FilePrefix = "ldoo"
            };

            this.decisionOrchestrationService = new DecisionOrchestrationService(
                decisionService: this.decisionServiceMock.Object,
                documentService: this.documentServiceMock.Object,
                csvHelperBroker: this.csvHelperBrokerMock.Object,
                hashBroker: this.hashBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                blobContainers: this.blobContainers,
                decisionConfiguration: this.decisionConfiguration);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private Expression<Func<Stream, bool>> SameStreamAs(Stream expectedStream)
        {
            return actualStream =>
                IsSameStream(expectedStream, actualStream);
        }

        private bool IsSameStream(Stream expectedStream, Stream actualStream)
        {
            byte[] expectedBytes = ReadAllBytesFromStream(expectedStream);
            byte[] actualBytes = ReadAllBytesFromStream(actualStream);

            string expectedString = System.Text.Encoding.UTF8.GetString(expectedBytes);
            string actualString = System.Text.Encoding.UTF8.GetString(actualBytes);

            try
            {
                actualString.Should().BeEquivalentTo(expectedString);
            }
            catch (Exception exception)
            {
                output.WriteLine(exception.Message);
            }

            return new CompareLogic().Compare(expectedBytes, actualBytes).AreEqual;
        }

        private static string GetRandomStringWithLengthOf(int length)
        {
            string result = new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

            return result.Length > length ? result.Substring(0, length) : result;
        }

        public static TheoryData<int> MinutesBeforeOrAfter()
        {
            int randomNumber = GetRandomNumber();
            int randomNegativeNumber = GetRandomNegativeNumber();

            return new TheoryData<int>
            {
                randomNumber,
                randomNegativeNumber
            };
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        public static TheoryData<Xeption> DecisionDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DecisionValidationException(
                    message: "Decision validation errors occurred, please try again.",
                    innerException),

                new DocumentValidationException(
                    message: "Document validation errors occured, please try again",
                    innerException),

                new DocumentDependencyValidationException(
                    message: "Document dependency validation occurred, please try again.",
                    innerException),

                new CsvHelperClientValidationException(innerException)
            };
        }

        public static TheoryData<Xeption> DecisionDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DecisionServiceException(
                    message: "Decision service error occurred, please contact support.",
                    innerException),

                new DocumentDependencyException(
                    message: "Document service error occured, please contact support.",
                    innerException),

                new DocumentServiceException(
                    message: "Document service error occurred, please contact support.",
                    innerException),

                new CsvHelperClientDependencyException(innerException),
                new CsvHelperClientServiceException(innerException)
            };
        }

        private Dictionary<string, int> GetFieldMappings() =>
            new Dictionary<string, int>
            {
                { nameof(DecisionCsv.DecisionId), 0 },
                { nameof(DecisionCsv.NhsNumber), 1 },
                { nameof(DecisionCsv.PatientInstructionCategory), 2 },
                { nameof(DecisionCsv.PatientInstructionState), 3 },
                { nameof(DecisionCsv.InstructionDate), 4 }
            };

        private static List<Decision> CreateRandomDecisions()
        {
            return CreateDecisionFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                .ToList();
        }

        private static Filler<Decision> CreateDecisionFiller(DateTimeOffset dateTimeOffset, string userId = "")
        {
            userId = string.IsNullOrEmpty(userId) ? Guid.NewGuid().ToString() : userId;

            DecisionType decisionType = new()
            {
                Id = Guid.NewGuid()
            };

            Patient patient = new()
            {
                Id = Guid.NewGuid()
            };

            var filler = new Filler<Decision>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(decision => decision.DecisionChoice).Use(GetRandomStringWithLengthOf(255))
                .OnProperty(decision => decision.CreatedBy).Use(userId)
                .OnProperty(decision => decision.UpdatedBy).Use(userId);

            return filler;
        }

        static byte[] ReadAllBytesFromStream(Stream stream)
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
    }
}
