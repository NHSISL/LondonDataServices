using System;
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
        public async Task ShouldRemoveDataTypeByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputDataTypeId = randomId;
            DataType randomDataType = CreateRandomDataType();
            DataType storageDataType = randomDataType;
            DataType expectedInputDataType = storageDataType;
            DataType deletedDataType = expectedInputDataType;
            DataType expectedDataType = deletedDataType.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataTypeByIdAsync(inputDataTypeId))
                    .ReturnsAsync(storageDataType);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteDataTypeAsync(expectedInputDataType))
                    .ReturnsAsync(deletedDataType);

            // when
            DataType actualDataType = await this.dataTypeService
                .RemoveDataTypeByIdAsync(inputDataTypeId);

            // then
            actualDataType.Should().BeEquivalentTo(expectedDataType);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataTypeByIdAsync(inputDataTypeId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDataTypeAsync(expectedInputDataType),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}