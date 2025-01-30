// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Models.Processings.SubscriberCredentials.Exceptions;
using LHDS.Core.Services.Processings.SecureDatas;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.SecureDatas
{
    public partial class SecureDataProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionRetrieveIfSubscriberCredentialIsNullAsync()
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
            ValueTask<SubscriberCredential> secureDataRetrieveTask =
                this.secureDataProcessingService.RetrieveSecretsByKeyVaultKeyNameAsync(nullSubscriberCredential);

            SubscriberCredentialValidationException actualSubscriberCredentialValidationException =
                await Assert.ThrowsAsync<SubscriberCredentialValidationException>(secureDataRetrieveTask.AsTask);

            // then
            actualSubscriberCredentialValidationException.Should().BeEquivalentTo(
                expectedSubscriberCredentialValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(expectedSubscriberCredentialValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.secureDataServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionAddOrRetrieveIfSubscriberCredentialIsInvalidAsync(
            string invalidText)
        {
            // given
            var invalidSubscriberCredential = new SubscriberCredential
            {
                Id = Guid.Empty,
                SupplierSharingAgreementShortName = invalidText,
                FtpUserName = invalidText,
                FtpPublicKey = invalidText,
                GpgPublicKey = invalidText,
            };

            var invalidSubscriberCredentialException =
                new InvalidSubscriberCredentialException(
                    message: "Invalid subscriber credential errors occurred. Please correct the errors and try again.");

            invalidSubscriberCredentialException.AddData(
                key: nameof(SubscriberCredential.Id),
                values: "Id is required");

            var expectedSubscriberCredentialValidationException =
               new SubscriberCredentialValidationException(
                   message: "Subscriber credential validation errors occurred, please try again.",
                   innerException: invalidSubscriberCredentialException);

            // when
            ValueTask<SubscriberCredential> secureDataRetrieveTask =
                this.secureDataProcessingService.RetrieveSecretsByKeyVaultKeyNameAsync(invalidSubscriberCredential);

            SubscriberCredentialValidationException actualSubscriberCredentialValidationException =
                await Assert.ThrowsAsync<SubscriberCredentialValidationException>(secureDataRetrieveTask.AsTask);

            // then
            actualSubscriberCredentialValidationException.Should()
                .BeEquivalentTo(expectedSubscriberCredentialValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberCredentialValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.secureDataServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionRetrieveIfSubscriberCredentialPropertyIsInvlaidAsync()
        {
            // given
            dynamic randomCredential = CreateRandomDynamicSharingAgreementCredential();
            List<string> invalidProperties = GetRandomProperties();

            SubscriberCredential inputSubscriberCredential =
                CreateSubscriberCredentialFromDynamic(credential: randomCredential);

            var secureDataProcessingServiceMock = new Mock<SecureDataProcessingService>(
                secureDataServiceMock.Object,
                loggingBrokerMock.Object,
                identifierBrokerMock.Object)
            { CallBase = true };

            secureDataProcessingServiceMock.Setup(x => x.GetPropertyList()).Returns(invalidProperties);

            var invalidArgumentSubscriberCredentialProcessingException =
                new InvalidArgumentSubscriberCredentialProcessingException(
                    message: "Invalid argument subscriber credential processing error occurred, please contact support.");

            foreach (string keyType in invalidProperties)
            {
                invalidArgumentSubscriberCredentialProcessingException.AddData(
                    key: keyType,
                    values: "Invalid property");
            }

            var expectedSubscriberCredentialValidationException =
                new SubscriberCredentialValidationException(
                    message: "Subscriber credential validation errors occurred, please try again.",
                    innerException: invalidArgumentSubscriberCredentialProcessingException);

            // when
            ValueTask<SubscriberCredential> secureDataRetrieveTask =
                secureDataProcessingServiceMock.Object.RetrieveSecretsByKeyVaultKeyNameAsync(inputSubscriberCredential);

            SubscriberCredentialValidationException actualSubscriberCredentialValidationException =
                await Assert.ThrowsAsync<SubscriberCredentialValidationException>(secureDataRetrieveTask.AsTask);

            // then
            actualSubscriberCredentialValidationException.Should().BeEquivalentTo(
                expectedSubscriberCredentialValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(expectedSubscriberCredentialValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.secureDataServiceMock.VerifyNoOtherCalls();
        }
    }
}