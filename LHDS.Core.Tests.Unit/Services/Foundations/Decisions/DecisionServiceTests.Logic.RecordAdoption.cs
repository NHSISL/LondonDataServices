// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Decisions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Decisions
{
    public partial class DecisionServiceTests
    {
        [Fact]
        public async Task ShouldRecordAdoptionAsync()
        {
            // given
            List<Decision> randomDecisionsAdopted = CreateRandomDecisions();
            List<Decision> inputDecisionsAdopted = randomDecisionsAdopted;

            this.decisionBrokerMock.Setup(broker =>
                broker.RecordAdoption(inputDecisionsAdopted));

            // when
            await this.decisionService.RecordAdoption(inputDecisionsAdopted);

            // then
            this.decisionBrokerMock.Verify(broker =>
                broker.RecordAdoption(inputDecisionsAdopted),
                    Times.Once);

            this.decisionBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
