// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Orchestrations.AddressExtractions.Exceptions;
using LHDS.Core.Models.Orchestrations.SubscriberCredentials.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Models.Processings.SubscriberCredentials.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.SubscriberCredentials
{
    public partial class SubscriberCredentialOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyOrAddIfSubscriberCredentialIsNullAndLogItAsync()
        {
            // given
            SubscriberCredential nullSubscriberCredential = null;

            var invalidArgumentSubscriberCredentialProcessingException =
                new InvalidArgumentSubscriberCredentialProcessingException(
                    message: "Invalid argument subscriber credential orchestration exception, " +
                        "please correct the errors and try again.");

            var expectedSubscriberCredentialValidationOrchestrationException =
                new SubscriberCredentialValidationOrchestrationException(
                    message: "Subscriber credential orchestration validation error occurred, please try again.",
                    innerException: invalidArgumentSubscriberCredentialProcessingException);

            // when
            ValueTask<SubscriberCredential> addOrModifySubscriberCredentialTask =
                this.subscriberCredentialOrchestration.ModifyOrAddSubscriberCredentialAsync(nullSubscriberCredential);

            SubscriberCredentialValidationOrchestrationException actualException =
                await Assert.ThrowsAsync<SubscriberCredentialValidationOrchestrationException>(
                    addOrModifySubscriberCredentialTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedSubscriberCredentialValidationOrchestrationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSubscriberCredentialValidationOrchestrationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
