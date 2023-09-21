// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.DataSets;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.DataSets
{
    public partial class DataSetProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveDataSetIfOneExistsAndNotAddAsync()
        {
            // Given
            DataSet randomDataSet = CreateRandomDataSet();
            DataSet inputDataSet = randomDataSet;
            DataSet storageDataSet = inputDataSet;
            DataSet expectedDataSet = storageDataSet.DeepClone();

            this.dataSetServiceMock.Setup(service =>
                service.RetrieveDataSetByIdAsync(inputDataSet.Id))
                    .ReturnsAsync(value: storageDataSet);

            // When
            await this.dataSetProcessingService.RetrieveOrAddDataSetAsync(inputDataSet);

            // Then
            this.dataSetServiceMock.Verify(service =>
                service.RetrieveDataSetByIdAsync(inputDataSet.Id),
                    Times.Once);

            this.dataSetServiceMock.Verify(service =>
                service.AddDataSetAsync(inputDataSet),
                    Times.Never);

            this.dataSetServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldAddDataSetIfOneDoesNotExistAsync()
        {
            // Given
            DataSet randomDataSet = CreateRandomDataSet();
            DataSet inputDataSet = randomDataSet;
            DataSet storageDataSet = inputDataSet;
            DataSet expectedDataSet = storageDataSet.DeepClone();

            this.dataSetServiceMock.Setup(service =>
                service.RetrieveDataSetByIdAsync(inputDataSet.Id))
                    .ReturnsAsync(value: null);

            this.dataSetServiceMock.Setup(service =>
                service.AddDataSetAsync(inputDataSet))
                    .ReturnsAsync(storageDataSet);

            // When
            await this.dataSetProcessingService.RetrieveOrAddDataSetAsync(inputDataSet);

            // Then
            this.dataSetServiceMock.Verify(service =>
                service.RetrieveDataSetByIdAsync(inputDataSet.Id),
                    Times.Once);

            this.dataSetServiceMock.Verify(service =>
                service.AddDataSetAsync(inputDataSet),
                    Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}