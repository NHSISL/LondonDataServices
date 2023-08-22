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
        public async Task ShouldAddDataTypeAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            DataType randomDataType = CreateRandomDataType(randomDateTimeOffset);
            DataType inputDataType = randomDataType;
            DataType storageDataType = inputDataType;
            DataType expectedDataType = storageDataType.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertDataTypeAsync(inputDataType))
                    .ReturnsAsync(storageDataType);

            // when
            DataType actualDataType = await this.dataTypeService
                .AddDataTypeAsync(inputDataType);

            // then
            actualDataType.Should().BeEquivalentTo(expectedDataType);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataTypeAsync(inputDataType),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}