// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SecureData;
using LHDS.Core.Models.Foundations.SecureData.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SecureDatas
{
    public partial class SecureDataServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionAddOrModifyIfSecureDataIsNullAsync()
        {
            // given
            SecureData nullSecureData = null;

            var nullSecureDataException =
                new NullSecureDataException(message: "Secure data is null.");

            var expectedSecureDataValidationException =
                new SecureDataValidationException(
                    message: "Secure data validation errors occurred, please try again.",
                    innerException: nullSecureDataException);

            // when
            ValueTask<SecureData> addSecureDataTask = this.secureDataService.AddOrModifySecureData(nullSecureData);

            SecureDataValidationException actualSecureDataValidationException =
                await Assert.ThrowsAsync<SecureDataValidationException>(addSecureDataTask.AsTask);

            // then
            actualSecureDataValidationException.Should().BeEquivalentTo(expectedSecureDataValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSecureDataValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.keyVaultSecretBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionAddOrModifyIfSecureDataIsInvalidAsync(string invalidText)
        {
            // given
            var invalidSecureData = new SecureData
            {
                Name = invalidText,
                Value = invalidText
            };

            var invalidAddressException =
                new InvalidSecureDataException(
                    message: "Invalid secure data errors occured. Please correct the errors and try again.");

            invalidAddressException.AddData(
                key: nameof(SecureData.Name),
                values: "Text is required");

            var expectedSecureDataValidationException =
                new SecureDataValidationException(
                    message: "Secure data validation errors occurred, please try again.",
                    innerException: invalidAddressException);

            // when
            ValueTask<SecureData> addSecureDataTask =
                this.secureDataService.AddOrModifySecureData(invalidSecureData);

            SecureDataValidationException actualSecureDataValidationException =
                await Assert.ThrowsAsync<SecureDataValidationException>(addSecureDataTask.AsTask);

            // then
            actualSecureDataValidationException.Should()
                .BeEquivalentTo(expectedSecureDataValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSecureDataValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.keyVaultSecretBrokerMock.VerifyNoOtherCalls();
        }
    }
}