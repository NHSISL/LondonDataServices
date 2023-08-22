using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.DataTypes;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataTypes
{
    public partial class DataTypeServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveDataTypeByIdAsync()
        {
            // given
            DataType randomDataType = CreateRandomDataType();
            DataType inputDataType = randomDataType;
            DataType storageDataType = randomDataType;
            DataType expectedDataType = storageDataType.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataTypeByIdAsync(inputDataType.Id))
                    .ReturnsAsync(storageDataType);

            // when
            DataType actualDataType =
                await this.dataTypeService.RetrieveDataTypeByIdAsync(inputDataType.Id);

            // then
            actualDataType.Should().BeEquivalentTo(expectedDataType);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataTypeByIdAsync(inputDataType.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}