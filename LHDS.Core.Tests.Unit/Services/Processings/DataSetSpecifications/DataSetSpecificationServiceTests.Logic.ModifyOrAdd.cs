// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.DataSetSpecifications
{
    public partial class DataSetSpecificationProcessingServiceTests
    {
        [Fact]
        public async Task ShouldModifyDataSetSpecificationIfOneExistsAndNotAddAsync()
        {
            // Given
            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification();
            DataSetSpecification storageDataSetSpecification = randomDataSetSpecification;
            DataSetSpecification modifiedDataSetSpecification = storageDataSetSpecification.DeepClone();
            modifiedDataSetSpecification.UpdatedDate = DateTimeOffset.UtcNow;
            DataSetSpecification updatedDataSetSpecification = modifiedDataSetSpecification.DeepClone();
            DataSetSpecification expectedDataSetSpecification = updatedDataSetSpecification;

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.RetrieveDataSetSpecificationByIdAsync(modifiedDataSetSpecification.Id))
                    .ReturnsAsync(value: storageDataSetSpecification);

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.ModifyDataSetSpecificationAsync(modifiedDataSetSpecification))
                    .ReturnsAsync(value: updatedDataSetSpecification);

            // When
            DataSetSpecification actualDataSetSpecification = await this.dataSetSpecificationProcessingService
                .ModifyOrAddDataSetSpecificationAsync(modifiedDataSetSpecification);

            // Then
            actualDataSetSpecification.Should().BeEquivalentTo(expectedDataSetSpecification);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.RetrieveDataSetSpecificationByIdAsync(modifiedDataSetSpecification.Id),
                    Times.Once);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.ModifyDataSetSpecificationAsync(modifiedDataSetSpecification),
                    Times.Once);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.AddDataSetSpecificationAsync(modifiedDataSetSpecification),
                    Times.Never);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldAddDataSetSpecificationIfDataSetSpecificationDoesNotExistsAsync()
        {
            // Given
            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification();
            DataSetSpecification inputDataSetSpecification = randomDataSetSpecification;
            DataSetSpecification storageDataSetSpecification = inputDataSetSpecification.DeepClone();
            DataSetSpecification expectedDataSetSpecification = storageDataSetSpecification;

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.RetrieveDataSetSpecificationByIdAsync(inputDataSetSpecification.Id))
                    .ReturnsAsync(value: null);

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.AddDataSetSpecificationAsync(inputDataSetSpecification))
                    .ReturnsAsync(value: storageDataSetSpecification);

            // When
            await this.dataSetSpecificationProcessingService.ModifyOrAddDataSetSpecificationAsync(inputDataSetSpecification);

            // Then
            this.dataSetSpecificationServiceMock.Verify(service =>
                service.RetrieveDataSetSpecificationByIdAsync(inputDataSetSpecification.Id),
                    Times.Once);

            this.dataSetSpecificationServiceMock.Verify(service =>
            service.AddDataSetSpecificationAsync(inputDataSetSpecification),
            Times.Once);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.ModifyDataSetSpecificationAsync(inputDataSetSpecification),
                    Times.Never);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
