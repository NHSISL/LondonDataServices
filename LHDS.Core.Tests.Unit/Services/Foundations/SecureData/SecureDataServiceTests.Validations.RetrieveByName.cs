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
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnRetrieveByNameIfNameIsInvalidAsync(string invalidText)
        {
            // given
            string invalidSecretName = invalidText;

            var invalidArgumentSecureDataException =
                new InvalidArgumentSecureDataException(
                    message: "Invalid secure data argument. Please correct the errors and try again.");

            invalidArgumentSecureDataException.AddData(
                key: "secretName",
                values: "Text is required");

            var expectedSecureDataValidationException =
                new SecureDataValidationException(
                    message: "Secure data validation errors occurred, please try again.",
                    innerException: invalidArgumentSecureDataException);

            // when
            ValueTask<SecureData> retrieveSecureDataTask =
                this.secureDataService.RetrieveSecretDataByNameAsync(invalidSecretName);

            SecureDataValidationException actualSecureDataValidationException =
                await Assert.ThrowsAsync<SecureDataValidationException>(retrieveSecureDataTask.AsTask);

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