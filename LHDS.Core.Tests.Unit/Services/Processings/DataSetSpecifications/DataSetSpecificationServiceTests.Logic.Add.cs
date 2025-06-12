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
        public async Task ShouldAddDataSetSpecificationAsync()
        {
            // Given
            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification();
            DataSetSpecification inputDataSetSpecification = randomDataSetSpecification;
            DataSetSpecification storageDataSetSpecification = inputDataSetSpecification;
            DataSetSpecification expectedDataSetSpecification = storageDataSetSpecification.DeepClone();

            this.dataSetSpecificationServiceMock.Setup(service =>
                service.AddDataSetSpecificationAsync(inputDataSetSpecification))
                    .ReturnsAsync(storageDataSetSpecification);

            // When
            DataSetSpecification actualDataSetSpecification = await this.dataSetSpecificationProcessingService
                .AddDataSetSpecificationAsync(inputDataSetSpecification);

            // Then
            actualDataSetSpecification.Should().BeEquivalentTo(expectedDataSetSpecification);

            this.dataSetSpecificationServiceMock.Verify(service =>
                service.AddDataSetSpecificationAsync(inputDataSetSpecification),
                    Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
