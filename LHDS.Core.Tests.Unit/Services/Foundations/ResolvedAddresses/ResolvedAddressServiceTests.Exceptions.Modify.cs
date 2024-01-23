using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress();
            SqlException sqlException = GetSqlException();

            var failedResolvedAddressStorageException =
                new FailedResolvedAddressStorageException(
                    message: "Failed resolvedAddress storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedResolvedAddressDependencyException =
                new ResolvedAddressDependencyException(
                    message: "ResolvedAddress dependency error occurred, contact support.",
                    innerException: failedResolvedAddressStorageException); 

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<ResolvedAddress> modifyResolvedAddressTask =
                this.resolvedAddressService.ModifyResolvedAddressAsync(randomResolvedAddress);

            ResolvedAddressDependencyException actualResolvedAddressDependencyException =
                await Assert.ThrowsAsync<ResolvedAddressDependencyException>(
                    modifyResolvedAddressTask.AsTask);

            // then
            actualResolvedAddressDependencyException.Should()
                .BeEquivalentTo(expectedResolvedAddressDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressByIdAsync(randomResolvedAddress.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedResolvedAddressDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateResolvedAddressAsync(randomResolvedAddress),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}