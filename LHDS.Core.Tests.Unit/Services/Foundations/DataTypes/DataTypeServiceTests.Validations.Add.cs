using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.DataTypes;
using LHDS.Core.Models.Foundations.DataTypes.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataTypes
{
    public partial class DataTypeServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfDataTypeIsNullAndLogItAsync()
        {
            // given
            DataType nullDataType = null;

            var nullDataTypeException =
                new NullDataTypeException();

            var expectedDataTypeValidationException =
                new DataTypeValidationException(nullDataTypeException);

            // when
            ValueTask<DataType> addDataTypeTask =
                this.dataTypeService.AddDataTypeAsync(nullDataType);

            DataTypeValidationException actualDataTypeValidationException =
                await Assert.ThrowsAsync<DataTypeValidationException>(
                    addDataTypeTask.AsTask);

            // then
            actualDataTypeValidationException.Should().BeEquivalentTo(expectedDataTypeValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDataTypeValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}