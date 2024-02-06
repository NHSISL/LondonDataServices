using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.Addresses;
using Xunit;
using Azure.Security.KeyVault.Secrets;
using LHDS.Core.Models.Foundations.SecureData;
using LHDS.Core.Models.Foundations.Addresses.Exceptions;
using LHDS.Core.Models.Foundations.SecureData.Exceptions;

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
                await Assert.ThrowsAsync<SecureDataValidationException>(() =>
                    addSecureDataTask.AsTask());

            // then
            actualSecureDataValidationException.Should().BeEquivalentTo(expectedSecureDataValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSecureDataValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.secureDataBrokerMock.VerifyNoOtherCalls();
        }
    }
}