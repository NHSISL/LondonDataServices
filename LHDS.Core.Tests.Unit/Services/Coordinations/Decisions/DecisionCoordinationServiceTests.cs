// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LHDS.Core.Brokers.Loggings;
using LHDS.Core.Models.Foundations.DecisionPolls;
using LHDS.Core.Models.Foundations.Decisions;
using LHDS.Core.Models.Foundations.DecisionTypes;
using LHDS.Core.Models.Foundations.Patients;
using LHDS.Core.Models.Orchestrations.Decisions.Exceptions;
using LHDS.Core.Services.Coordinations.Decisions;
using LHDS.Core.Services.Orchestrations.Decisions;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.Decisions
{
    public partial class DecisionCoordinationServiceTests
    {
        private readonly Mock<IDecisionOrchestrationService> decisionOrchestrationServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IDecisionCoordinationService decisionCoordinationService;

        public DecisionCoordinationServiceTests()
        {
            this.decisionOrchestrationServiceMock = new Mock<IDecisionOrchestrationService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.decisionCoordinationService = new DecisionCoordinationService(
                decisionOrchestrationService: this.decisionOrchestrationServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
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

        public static TheoryData<Xeption> DecisionCoordinationDependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DecisionOrchestrationValidationException(
                    message: "Decision orchestration validation errors occurred, please try again.",
                    innerException),

                new DecisionOrchestrationDependencyValidationException(
                message: "Decision orchestration dependency validation errors occurred, please try again.",
                innerException)
            };
        }

        public static TheoryData<Xeption> DecisionCoordinationDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DecisionOrchestrationDependencyException(
                    message: "Decision orchestration dependency error occurred, please contact support.",
                    innerException),

                new DecisionOrchestrationServiceException(
                    message: "Decision orchestration service error occured, please contact support.",
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
    }
}
