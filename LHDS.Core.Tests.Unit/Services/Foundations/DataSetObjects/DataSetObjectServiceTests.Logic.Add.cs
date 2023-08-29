using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.DataSetObjects;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetObjects
{
    public partial class DataSetObjectServiceTests
    {
        [Fact]
        public async Task ShouldAddDataSetObjectAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            DataSetObject randomDataSetObject = CreateRandomDataSetObject(randomDateTimeOffset);
            DataSetObject inputDataSetObject = randomDataSetObject;
            DataSetObject storageDataSetObject = inputDataSetObject;
            DataSetObject expectedDataSetObject = storageDataSetObject.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertDataSetObjectAsync(inputDataSetObject))
                    .ReturnsAsync(storageDataSetObject);

            // when
            DataSetObject actualDataSetObject = await this.dataSetObjectService
                .AddDataSetObjectAsync(inputDataSetObject);

            // then
            actualDataSetObject.Should().BeEquivalentTo(expectedDataSetObject);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetObjectAsync(inputDataSetObject),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}