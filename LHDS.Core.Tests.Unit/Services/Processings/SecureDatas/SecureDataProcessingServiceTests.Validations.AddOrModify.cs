// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Models.Processings.SubscriberCredentials.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.SecureDatas
{
    public partial class SecureDataProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionAddOrModifyIfSubscriberCredentialIsNullAsync()
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
            ValueTask<SubscriberCredential> addSubscriberCredentialTask =
                this.secureDataProcessingService.AddOrModifySecureDataAsync(nullSubscriberCredential);

            SubscriberCredentialValidationException actualSubscriberCredentialValidationException =
                await Assert.ThrowsAsync<SubscriberCredentialValidationException>(() =>
                    addSubscriberCredentialTask.AsTask());

            // then
            actualSubscriberCredentialValidationException.Should().BeEquivalentTo(
                expectedSubscriberCredentialValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSubscriberCredentialValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.secureDataServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionAddOrModifyIfSubscriberCredentialIsInvalidAsync(
            string invalidText)
        {
            // given
            var invalidSubscriberCredential = new SubscriberCredential
            {
                Id = Guid.Empty,
                SupplierSharingAgreementShortName = invalidText,
                FtpUserName = invalidText,
                FtpPassPhrase = invalidText,
                FtpPrivateKey = invalidText,
                FtpPublicKey = invalidText,
                GpgPassPhrase = invalidText,
                GpgPrivateKey = invalidText,
                GpgPublicKey = invalidText,
            };

            var invalidSubscriberCredentialException =
                new InvalidSubscriberCredentialException(
                    message: "Invalid subscriber credential errors occured. Please correct the errors and try again.");

            invalidSubscriberCredentialException.AddData(
                key: nameof(SubscriberCredential.Id),
                values: "Id is required");

            invalidSubscriberCredentialException.AddData(
                key: nameof(SubscriberCredential.SupplierSharingAgreementShortName),
                values: "Text is required");

            invalidSubscriberCredentialException.AddData(
                key: nameof(SubscriberCredential.FtpUserName),
                values: "Text is required");

            invalidSubscriberCredentialException.AddData(
                key: nameof(SubscriberCredential.FtpPassPhrase),
                values: "Text is required");

            invalidSubscriberCredentialException.AddData(
                key: nameof(SubscriberCredential.FtpPrivateKey),
                values: "Text is required");

            invalidSubscriberCredentialException.AddData(
                key: nameof(SubscriberCredential.FtpPublicKey),
                values: "Text is required");

            invalidSubscriberCredentialException.AddData(
                key: nameof(SubscriberCredential.GpgPassPhrase),
                values: "Text is required");

            invalidSubscriberCredentialException.AddData(
                key: nameof(SubscriberCredential.GpgPrivateKey),
                values: "Text is required");


            invalidSubscriberCredentialException.AddData(
                key: nameof(SubscriberCredential.GpgPublicKey),
                values: "Text is required");

            var expectedSubscriberCredentialValidationException =
               new SubscriberCredentialValidationException(
                   message: "Subscriber credential validation errors occurred, please try again.",
                   innerException: invalidSubscriberCredentialException);

            // when
            ValueTask<SubscriberCredential> addSubscriberCredentialTask =
                this.secureDataProcessingService.AddOrModifySecureDataAsync(invalidSubscriberCredential);

            SubscriberCredentialValidationException actualSubscriberCredentialValidationException =
                await Assert.ThrowsAsync<SubscriberCredentialValidationException>(() =>
                    addSubscriberCredentialTask.AsTask());

            // then
            actualSubscriberCredentialValidationException.Should()
                .BeEquivalentTo(expectedSubscriberCredentialValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSubscriberCredentialValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.secureDataServiceMock.VerifyNoOtherCalls();
        }
    }
}