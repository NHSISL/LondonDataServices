// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.DataSets;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.DataSets
{
    public partial class DataSetProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRemoveDataSetByIdAsync()
        {
            // Given
            DataSet randomDataSet = CreateRandomDataSet();
            DataSet storageDataSet = randomDataSet;
            DataSet expectedDataSet = storageDataSet.DeepClone();

            this.dataSetServiceMock.Setup(service =>
                service.RemoveDataSetByIdAsync(randomDataSet.Id))
                    .ReturnsAsync(storageDataSet);

            // When
            DataSet actualDataSet =
                await this.dataSetProcessingService
                    .RemoveDataSetByIdAsync(randomDataSet.Id);

            // Then
            actualDataSet.Should().BeEquivalentTo(expectedDataSet);

            this.dataSetServiceMock.Verify(service =>
                service.RemoveDataSetByIdAsync(randomDataSet.Id),
                    Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}