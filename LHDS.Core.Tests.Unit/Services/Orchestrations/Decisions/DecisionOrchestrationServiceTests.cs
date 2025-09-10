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
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.DecisionPolls;
using LHDS.Core.Models.Foundations.Decisions;
using LHDS.Core.Models.Foundations.DecisionTypes;
using LHDS.Core.Models.Foundations.Patients;
using LHDS.Core.Services.Foundations.DecisionPolls;
using LHDS.Core.Services.Foundations.Decisions;
using LHDS.Core.Services.Foundations.Documents;
using LHDS.Core.Services.Orchestrations.Decisions;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using Xunit.Abstractions;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Decisions
{
    public partial class DecisionOrchestrationServiceTests
    {
        private readonly Mock<IDecisionPollService> decisionPollServiceMock;
        private readonly Mock<IDecisionService> decisionServiceMock;
        private readonly Mock<IDocumentService> documentServiceMock;
        private readonly IDecisionOrchestrationService decisionOrchestrationService;
        private readonly BlobContainers blobContainers;
        private readonly ITestOutputHelper output;
        private readonly ICompareLogic compareLogic;

        public DecisionOrchestrationServiceTests(ITestOutputHelper output)
        {
            this.output = output;
            this.compareLogic = new CompareLogic();
            this.decisionPollServiceMock = new Mock<IDecisionPollService>();
            this.decisionServiceMock = new Mock<IDecisionService>();
            this.documentServiceMock = new Mock<IDocumentService>();

            this.blobContainers = new BlobContainers
            {
                Decisions = "decisions"
            };

            this.decisionOrchestrationService = new DecisionOrchestrationService(
                decisionPollService: this.decisionPollServiceMock.Object,
                decisionService: this.decisionServiceMock.Object,
                documentService: this.documentServiceMock.Object,
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

        private Expression<Func<DecisionPoll, bool>> SameDecisionPollAs(
            DecisionPoll expectedDecisionPoll)
        {
            return actualDecisionPoll =>
                IsSameDecisionPoll(expectedDecisionPoll, actualDecisionPoll);
        }

        private bool IsSameDecisionPoll(DecisionPoll expected, DecisionPoll actual)
        {
            try
            {
                actual.Should().BeEquivalentTo(expected, options => options
                    .Excluding(poll => poll.Id));
            }
            catch (Exception exception)
            {
                output.WriteLine(exception.Message);
            }

            return new CompareLogic(new ComparisonConfig
            {
                MembersToIgnore = new List<string> { "Id" }
            }).Compare(expected, actual).AreEqual;
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

        private static DecisionPoll CreateRandomDecisionPoll(
            DateTimeOffset dateTimeOffset,
            string userId) =>
            CreateDecisionPollFiller(dateTimeOffset, userId).Create();

        private static Filler<DecisionPoll> CreateDecisionPollFiller(
            DateTimeOffset dateTimeOffset,
            string userId)
        {
            var filler = new Filler<DecisionPoll>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(decisionPoll => decisionPoll.CreatedBy).Use(userId)
                .OnProperty(decisionPoll => decisionPoll.UpdatedBy).Use(userId);

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
