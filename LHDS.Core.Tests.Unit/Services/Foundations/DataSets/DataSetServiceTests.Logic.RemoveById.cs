using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.DataSets;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSets
{
    public partial class DataSetServiceTests
    {
        [Fact]
        public async Task ShouldRemoveDataSetByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputDataSetId = randomId;
            DataSet randomDataSet = CreateRandomDataSet();
            DataSet storageDataSet = randomDataSet;
            DataSet expectedInputDataSet = storageDataSet;
            DataSet deletedDataSet = expectedInputDataSet;
            DataSet expectedDataSet = deletedDataSet.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetByIdAsync(inputDataSetId))
                    .ReturnsAsync(storageDataSet);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteDataSetAsync(expectedInputDataSet))
                    .ReturnsAsync(deletedDataSet);

            // when
            DataSet actualDataSet = await this.dataSetService
                .RemoveDataSetByIdAsync(inputDataSetId);

            // then
            actualDataSet.Should().BeEquivalentTo(expectedDataSet);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetByIdAsync(inputDataSetId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteDataSetAsync(expectedInputDataSet),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}