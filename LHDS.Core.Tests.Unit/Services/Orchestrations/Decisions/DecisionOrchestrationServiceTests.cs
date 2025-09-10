// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.DecisionPolls;
using LHDS.Core.Models.Foundations.DecisionPolls.Exceptions;
using LHDS.Core.Models.Foundations.Decisions;
using LHDS.Core.Models.Foundations.Decisions.Exceptions;
using LHDS.Core.Models.Foundations.DecisionTypes;
using LHDS.Core.Models.Foundations.Documents.Exceptions;
using LHDS.Core.Models.Foundations.Patients;
using LHDS.Core.Services.Foundations.DecisionPolls;
using LHDS.Core.Services.Foundations.Decisions;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Orchestrations.Decisions;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Decisions
{
    public partial class DecisionOrchestrationServiceTests
    {
        private readonly Mock<IDecisionPollService> decisionPollServiceMock;
        private readonly Mock<IDecisionService> decisionServiceMock;
        private readonly Mock<IDocumentService> documentServiceMock;
        private readonly IDecisionOrchestrationService decisionOrchestrationService;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly BlobContainers blobContainers;
        private readonly ICompareLogic compareLogic;

        public DecisionOrchestrationServiceTests()
        {
            this.compareLogic = new CompareLogic();
            this.decisionPollServiceMock = new Mock<IDecisionPollService>();
            this.decisionServiceMock = new Mock<IDecisionService>();
            this.documentServiceMock = new Mock<IDocumentService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.blobContainers = new BlobContainers
            {
                Decisions = "decisions"
            };

            this.decisionOrchestrationService = new DecisionOrchestrationService(
                decisionPollService: this.decisionPollServiceMock.Object,
                decisionService: this.decisionServiceMock.Object,
                documentService: this.documentServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                blobContainers: this.blobContainers);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

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

                new DecisionPollValidationException(
                    message: "DecisionPoll validation errors occurred, please try again.",
                    innerException)
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

                new DecisionPollDependencyException(
                    message: "DecisionPoll dependency error occurred, please contact support.",
                    innerException),

                new DecisionPollServiceException(
                message: "DecisionPoll dependency error occurred, please contact support.",
                innerException)
            };
        }

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
                .OnProperty(decision => decision.UpdatedBy).Use(userId)
                .OnProperty(decision => decision.DecisionType).Use(decisionType)
                .OnProperty(decision => decision.Patient).Use(patient);

            return filler;
        }

        private static IQueryable<DecisionPoll> CreateRandomDecisionPolls()
        {
            return CreateDecisionPollFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                .AsQueryable();
        }

        private static Filler<DecisionPoll> CreateDecisionPollFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<DecisionPoll>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(decisionPoll => decisionPoll.CreatedBy).Use(user)
                .OnProperty(decisionPoll => decisionPoll.UpdatedBy).Use(user);

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
