// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.SecureData;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Models.Processings.SubscriberCredentials.Exceptions;
using LHDS.Core.Services.Processings.SecureDatas;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.SecureDatas
{
    public partial class SecureDataProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionRemoveIfSubscriberCredentialIsNullAsync()
        {
            // given
            SubscriberCredential nullSubscriberCredential = null;

            var nullSubscriberCredentialException =
                new NullSubscriberCredentialException(message: "Subscriber credential is null.");

            var expectedSubscriberCredentialValidationException =
                new SubscriberCredentialValidationException(
                    message: "Subscriber credential validation errors occurred, please try again.",
                    innerException: nullSubscriberCredentialException);

            // when
            ValueTask<SubscriberCredential> removeSubscriberCredentialTask =
                this.secureDataProcessingService.RemoveSecureDataAsync(nullSubscriberCredential);

            SubscriberCredentialValidationException actualSubscriberCredentialValidationException =
                await Assert.ThrowsAsync<SubscriberCredentialValidationException>(() =>
                    removeSubscriberCredentialTask.AsTask());

            // then
            actualSubscriberCredentialValidationException.Should().BeEquivalentTo(
                expectedSubscriberCredentialValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSubscriberCredentialValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.secureDataServiceMock.VerifyNoOtherCalls();
        }
    }
}