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
        public async Task ShouldModifyDataSetAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DataSet randomDataSet = CreateRandomModifyDataSet(randomDateTimeOffset);
            DataSet inputDataSet = randomDataSet;
            DataSet storageDataSet = inputDataSet.DeepClone();
            storageDataSet.UpdatedDate = randomDataSet.CreatedDate;
            DataSet updatedDataSet = inputDataSet;
            DataSet expectedDataSet = updatedDataSet.DeepClone();
            Guid dataSetId = inputDataSet.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateDataSetAsync(inputDataSet))
                    .ReturnsAsync(updatedDataSet);

            // when
            DataSet actualDataSet =
                await this.dataSetService.ModifyDataSetAsync(inputDataSet);

            // then
            actualDataSet.Should().BeEquivalentTo(expectedDataSet);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateDataSetAsync(inputDataSet),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}