// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using LHDS.Core.Clients;
using LHDS.Core.Models.Foundations.Decisions;
using LHDS.Core.Models.Foundations.DecisionTypes;
using LHDS.Core.Models.Foundations.Patients;
using LHDS.Core.Models.Orchestrations.Decisions.Exceptions;
using LHDS.Core.Services.Orchestrations.Decisions;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Clients.IDecide
{
    public partial class IDecideClientTests
    {
        private readonly Mock<IDecisionOrchestrationService> decisionOrchestrationServiceMock;
        private readonly IIDecideClient iDecideClient;

        public IDecideClientTests()
        {
            this.decisionOrchestrationServiceMock = new Mock<IDecisionOrchestrationService>();

            this.iDecideClient = new IDecideClient(
                decisionOrchestrationService: this.decisionOrchestrationServiceMock.Object);
        }

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



        public static TheoryData<Xeption> IDecideClientDependencyValidationExceptions()
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
                    message:
                    "Decision orchestration dependency validation error occurred, fix the errors and try again.",
                    innerException)
            };
        }

        public static TheoryData<Xeption> IDecideClientDependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Xeption>
            {
                new DecisionOrchestrationDependencyException(
                    message: "Decision orchestration dependency error occurred, fix the errors and try again.",
                    innerException)
            };
        }

        public static TheoryData<Xeption> IDecideClientServiceExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Exception();

            return new TheoryData<Xeption>
            {
                new DecisionOrchestrationServiceException(
                    message: "Decision orchestration service error occurred, please contact support.",
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
                .OnProperty(decision => decision.UpdatedBy).Use(userId);

            return filler;
        }
    }
}
