// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.DataSetSpecifications
{
    public partial class DataSetSpecificationProcessingServiceTests
    {
        [Fact]
        public async Task ShouldModifyDataSetSpecificationAsync()
        {
            // Given
            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification();
            DataSetSpecification inputDataSetSpecification = randomDataSetSpecification;
            DataSetSpecification storageDataSetSpecification = inputDataSetSpecification;
            DataSetSpecification expectedDataSetSpecification = storageDataSetSpecification.DeepClone();

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.ModifyDataSetSpecificationAsync(inputDataSetSpecification))
                    .ReturnsAsync(storageDataSetSpecification);

            // When
            await this.dataSetSpecificationProcessingService.ModifyDataSetSpecificationAsync(inputDataSetSpecification);

            // Then
            this.dataSetSpecificationServiceMock.Verify(service =>
                service.ModifyDataSetSpecificationAsync(inputDataSetSpecification),
                    Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
