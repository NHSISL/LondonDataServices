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
        public async Task ShouldAddDataSetAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            DataSet randomDataSet = CreateRandomDataSet(randomDateTimeOffset);
            DataSet inputDataSet = randomDataSet;
            DataSet storageDataSet = inputDataSet;
            DataSet expectedDataSet = storageDataSet.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertDataSetAsync(inputDataSet))
                    .ReturnsAsync(storageDataSet);

            // when
            DataSet actualDataSet = await this.dataSetService
                .AddDataSetAsync(inputDataSet);

            // then
            actualDataSet.Should().BeEquivalentTo(expectedDataSet);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertDataSetAsync(inputDataSet),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}