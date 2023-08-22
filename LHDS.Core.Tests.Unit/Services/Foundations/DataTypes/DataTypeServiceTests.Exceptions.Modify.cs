using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.DataTypes;
using LHDS.Core.Models.Foundations.DataTypes.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataTypes
{
    public partial class DataTypeServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DataType randomDataType = CreateRandomDataType();
            SqlException sqlException = GetSqlException();

            var failedDataTypeStorageException =
                new FailedDataTypeStorageException(sqlException);

            var expectedDataTypeDependencyException =
                new DataTypeDependencyException(failedDataTypeStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<DataType> modifyDataTypeTask =
                this.dataTypeService.ModifyDataTypeAsync(randomDataType);

            DataTypeDependencyException actualDataTypeDependencyException =
                await Assert.ThrowsAsync<DataTypeDependencyException>(
                    modifyDataTypeTask.AsTask);

            // then
            actualDataTypeDependencyException.Should()
                .BeEquivalentTo(expectedDataTypeDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataTypeByIdAsync(randomDataType.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedDataTypeDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataTypeAsync(randomDataType),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}