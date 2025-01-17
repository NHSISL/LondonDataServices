// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.DataSetSpecifications
{
    public partial class DataSetSpecificationProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAllDataSetSpecificationsAsync()
        {
            // given
            IQueryable<DataSetSpecification> randomDataSetSpecifications = CreateRandomDataSetSpecifications();
            IQueryable<DataSetSpecification> storageDataSetSpecifications = randomDataSetSpecifications;
            IQueryable<DataSetSpecification> expectedDataSetSpecifications = storageDataSetSpecifications;

            this.dataSetSpecificationServiceMock.Setup(broker =>
                broker.RetrieveAllDataSetSpecificationsAsync())
                    .ReturnsAsync(storageDataSetSpecifications);

            // when
            IQueryable<DataSetSpecification> actualDataSetSpecifications =
                await this.dataSetSpecificationProcessingService.RetrieveAllDataSetSpecificationsAsync();

            // then
            actualDataSetSpecifications.Should().BeEquivalentTo(expectedDataSetSpecifications);

            this.dataSetSpecificationServiceMock.Verify(broker =>
                broker.RetrieveAllDataSetSpecificationsAsync(),
                    Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
