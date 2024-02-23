// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.SubscriberCredentials
{
    public partial class SubscriberCredentialOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldModifyOrAddSubscriberCredentialAndLogAsync()
        {
            // Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            SubscriberCredential randomSubscriberCredential = CreateRandomCreateSubscriberCredential(
                randomDateTimeOffset);

            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            SubscriberCredential outputSubscriberCredential = inputSubscriberCredential;
            SubscriberCredential expectedSubscriberCredential = outputSubscriberCredential.DeepClone();

            this.secureDataProcessingServiceMock.Setup(service =>
                service.AddOrModifySecureDataAsync(inputSubscriberCredential))
                    .ReturnsAsync(outputSubscriberCredential);

            // When
            SubscriberCredential actualSubscriberCredential = await this.subscriberCredentialOrchestration
                .ModifyOrAddSubscriberCredentialAsync(inputSubscriberCredential);

            // Then
            actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);

            this.secureDataProcessingServiceMock.Verify(service =>
                service.AddOrModifySecureDataAsync(inputSubscriberCredential),
                    Times.Once);

            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

