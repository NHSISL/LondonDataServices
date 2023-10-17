using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.Addresses.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Addresses
{
    public partial class AddressServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Address someAddress = CreateRandomAddress();
            SqlException sqlException = GetSqlException();

            var failedAddressStorageException =
                new FailedAddressStorageException(
                    message: "Failed address storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedAddressDependencyException =
                new AddressDependencyException(
                    message: "Address dependency error occurred, contact support.",
                    innerException: failedAddressStorageException);             

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Address> addAddressTask =
                this.addressService.AddAddressAsync(someAddress);

            AddressDependencyException actualAddressDependencyException =
                await Assert.ThrowsAsync<AddressDependencyException>(
                    addAddressTask.AsTask);

            // then
            actualAddressDependencyException.Should()
                .BeEquivalentTo(expectedAddressDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressAsync(It.IsAny<Address>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAddressDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfAddressAlreadyExsitsAndLogItAsync()
        {
            // given
            Address randomAddress = CreateRandomAddress();
            Address alreadyExistsAddress = randomAddress;
            string randomMessage = GetRandomString();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsAddressException =
                new AlreadyExistsAddressException(
                    message: "Address with the same Id already exists.",
                    innerException: duplicateKeyException);

            var expectedAddressDependencyValidationException =
                new AddressDependencyValidationException(
                    message: "Address dependency validation occurred, please try again.",
                    innerException: alreadyExistsAddressException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<Address> addAddressTask =
                this.addressService.AddAddressAsync(alreadyExistsAddress);

            // then
            AddressDependencyValidationException actualAddressDependencyValidationException =
                await Assert.ThrowsAsync<AddressDependencyValidationException>(
                    addAddressTask.AsTask);

            actualAddressDependencyValidationException.Should()
                .BeEquivalentTo(expectedAddressDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressAsync(It.IsAny<Address>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddressDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}