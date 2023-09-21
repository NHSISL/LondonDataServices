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
        public async Task ShouldModifyDataSetIfOneExistsAndNotAddAsync()
        {
            // Given
            DataSet randomDataSet = CreateRandomDataSet();
            DataSet storageDataSet = randomDataSet;
            DataSet modifiedDataSet = storageDataSet.DeepClone();
            modifiedDataSet.DataSetSupplier = modifiedDataSet.DataSetSupplier + "Modified";
            DataSet updatedDataSet = modifiedDataSet.DeepClone();
            DataSet expectedDataSet = updatedDataSet;

            this.dataSetServiceMock.Setup(service =>
                service.RetrieveDataSetByIdAsync(modifiedDataSet.Id))
                    .ReturnsAsync(value: storageDataSet);

            this.dataSetServiceMock.Setup(service =>
                service.ModifyDataSetAsync(modifiedDataSet))
                    .ReturnsAsync(value: updatedDataSet);

            // When
            await this.dataSetProcessingService.ModifyOrAddDataSetAsync(modifiedDataSet);

            // Then
            this.dataSetServiceMock.Verify(service =>
                service.RetrieveDataSetByIdAsync(modifiedDataSet.Id),
                    Times.Once);

            this.dataSetServiceMock.Verify(service =>
                service.ModifyDataSetAsync(modifiedDataSet),
                    Times.Once);

            this.dataSetServiceMock.Verify(service =>
                service.AddDataSetAsync(modifiedDataSet),
                    Times.Never);

            this.dataSetServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldAddDataSetIfDataSetDoesNotExistsAsync()
        {
            // Given
            DataSet randomDataSet = CreateRandomDataSet();
            DataSet inputDataSet = randomDataSet;
            DataSet storageDataSet = inputDataSet.DeepClone();
            DataSet expectedDataSet = storageDataSet;

            this.dataSetServiceMock.Setup(service =>
                service.RetrieveDataSetByIdAsync(inputDataSet.Id))
                    .ReturnsAsync(value: null);

            this.dataSetServiceMock.Setup(service =>
                service.AddDataSetAsync(inputDataSet))
                    .ReturnsAsync(value: storageDataSet);

            // When
            await this.dataSetProcessingService.ModifyOrAddDataSetAsync(inputDataSet);

            // Then
            this.dataSetServiceMock.Verify(service =>
                service.RetrieveDataSetByIdAsync(inputDataSet.Id),
                    Times.Once);

            this.dataSetServiceMock.Verify(service =>
            service.AddDataSetAsync(inputDataSet),
            Times.Once);

            this.dataSetServiceMock.Verify(service =>
                service.ModifyDataSetAsync(inputDataSet),
                    Times.Never);

            this.dataSetServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}