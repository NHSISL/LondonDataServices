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
        public async Task ShouldRemoveDataSetObjectByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputDataSetObjectId = randomId;
            DataSetObject randomDataSetObject = CreateRandomDataSetObject();
            DataSetObject storageDataSetObject = randomDataSetObject;
            DataSetObject expectedInputDataSetObject = storageDataSetObject;
            DataSetObject deletedDataSetObject = expectedInputDataSetObject;
            DataSetObject expectedDataSetObject = deletedDataSetObject.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetObjectByIdAsync(inputDataSetObjectId))
                    .ReturnsAsync(storageDataSetObject);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteDataSetObjectAsync(expectedInputDataSetObject))
                    .ReturnsAsync(deletedDataSetObject);

            // when
            DataSetObject actualDataSetObject = await this.dataSetObjectService
                .RemoveDataSetObjectByIdAsync(inputDataSetObjectId);

            // then
            actualDataSetObject.Should().BeEquivalentTo(expectedDataSetObject);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetObjectByIdAsync(inputDataSetObjectId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDataSetObjectAsync(expectedInputDataSetObject),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}