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
        public async Task ShouldRetrieveDataSetByIdAsync()
        {
            // Given
            DataSet randomDataSet = CreateRandomDataSet();
            DataSet storageDataSet = randomDataSet;
            DataSet expectedDataSet = storageDataSet.DeepClone();

            this.dataSetServiceMock.Setup(service =>
                service.RetrieveDataSetByIdAsync(randomDataSet.Id))
                    .ReturnsAsync(storageDataSet);

            // When
            DataSet actualDataSet =
                await this.dataSetProcessingService
                    .RetrieveDataSetByIdAsync(randomDataSet.Id);

            // Then
            actualDataSet.Should().BeEquivalentTo(expectedDataSet);

            this.dataSetServiceMock.Verify(service =>
                service.RetrieveDataSetByIdAsync(randomDataSet.Id),
                    Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}