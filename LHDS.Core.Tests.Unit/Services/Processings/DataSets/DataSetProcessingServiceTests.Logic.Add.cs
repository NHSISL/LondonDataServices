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
        public async Task ShouldAddDataSetAsync()
        {
            // Given
            DataSet randomDataSet = CreateRandomDataSet();
            DataSet inputDataSet = randomDataSet;
            DataSet storageDataSet = inputDataSet;
            DataSet expectedDataSet = storageDataSet.DeepClone();

            this.dataSetServiceMock.Setup(service =>
                service.AddDataSetAsync(inputDataSet))
                    .ReturnsAsync(storageDataSet);

            // When
            await this.dataSetProcessingService.AddDataSetAsync(inputDataSet);

            // Then
            this.dataSetServiceMock.Verify(service =>
                service.AddDataSetAsync(inputDataSet),
                    Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
        }
    }
}