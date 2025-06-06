// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldRetrieveDataSetSpecificationByIdAsync()
        {
            // Given
            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification();
            DataSetSpecification storageDataSetSpecification = randomDataSetSpecification;
            DataSetSpecification expectedDataSetSpecification = storageDataSetSpecification.DeepClone();

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.RetrieveDataSetSpecificationByIdAsync(randomDataSetSpecification.Id))
                    .ReturnsAsync(storageDataSetSpecification);

            // When
            DataSetSpecification actualDataSetSpecification =
                await this.dataSetSpecificationProcessingService
                    .RetrieveDataSetSpecificationByIdAsync(randomDataSetSpecification.Id);

            // Then
            actualDataSetSpecification.Should().BeEquivalentTo(expectedDataSetSpecification);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.RetrieveDataSetSpecificationByIdAsync(randomDataSetSpecification.Id),
                    Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
