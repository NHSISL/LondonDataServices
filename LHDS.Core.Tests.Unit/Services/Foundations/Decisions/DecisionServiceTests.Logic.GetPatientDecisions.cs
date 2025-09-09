// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Decisions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Decisions
{
    public partial class DecisionServiceTests
    {
        [Fact]
        public async Task ShouldGetPatientDecisionsAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DateTimeOffset inputDateTimeOffset = randomDateTimeOffset;
            List<Decision> randomDecisions = CreateRandomDecisions();
            List<Decision> brokerDecisions = randomDecisions;
            List<Decision> expectedDecisions = brokerDecisions.DeepClone();

            this.decisionBrokerMock.Setup(broker =>
                broker.GetPatientDecisions(inputDateTimeOffset))
                    .ReturnsAsync(brokerDecisions);

            // when
            List<Decision> actualDecisions =
                await this.decisionService.GetPatientDecisions(inputDateTimeOffset);

            // then
            actualDecisions.Should().BeEquivalentTo(expectedDecisions);

            this.decisionBrokerMock.Verify(broker =>
                broker.GetPatientDecisions(inputDateTimeOffset),
                    Times.Once);

            this.decisionBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
