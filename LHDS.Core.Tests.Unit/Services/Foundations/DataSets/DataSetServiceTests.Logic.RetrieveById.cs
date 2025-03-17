// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.DataSets;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSets
{
    public partial class DataSetServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveDataSetByIdAsync()
        {
            // given
            DataSet randomDataSet = CreateRandomDataSet();
            DataSet inputDataSet = randomDataSet;
            DataSet storageDataSet = randomDataSet;
            DataSet expectedDataSet = storageDataSet.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetByIdAsync(inputDataSet.Id))
                    .ReturnsAsync(storageDataSet);

            // when
            DataSet actualDataSet =
                await this.dataSetService.RetrieveDataSetByIdAsync(inputDataSet.Id);

            // then
            actualDataSet.Should().BeEquivalentTo(expectedDataSet);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetByIdAsync(inputDataSet.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
        }
    }
}