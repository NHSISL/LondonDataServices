// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Orchestrations.Pds.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Pds
{
    public partial class PdsOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(PdsDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnRetrieveAndUpdateIfDependencyValidationOccursAndLogItAsync(
           Xeption dependancyValidationException)
        {
            var expectedDependencyException =
                new PdsOrchestrationDependencyValidationException(
                    dependancyValidationException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
              service.RetrieveMessageIdsFromInboxAsync())
                    .Throws(dependancyValidationException);

            //when
            ValueTask<List<PdsAudit>> actualPdsAudits =
                this.pdsOrchestrationService.RetreiveMessagesFromMeshAndUpdateStorage();

            PdsOrchestrationDependencyValidationException actualException =
              await Assert.ThrowsAsync<PdsOrchestrationDependencyValidationException>(
                  actualPdsAudits.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.meshServiceMock.Verify(broker =>
                broker.RetrieveMessageIdsFromInboxAsync(),
                   Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
