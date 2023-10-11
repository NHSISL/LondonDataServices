// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSets;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.DataSetSpecifications
{
    public partial class DataSetSpecificationProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveActiveDataSetSpecificationsAsync()
        {
            // given
            Guid randomSupplierId = landingConfiguration.LandingSupplierId;
            DataSet randomDataSet = CreateRandomDataSet(randomSupplierId);

            IQueryable<DataSetSpecification> randomDataSetSpecifications =
                CreateRandomDataSetSpecifications(dataSetId: randomDataSet.Id, count: 1);

            IQueryable<DataSetSpecification> storageDataSetSpecifications = randomDataSetSpecifications;
            IQueryable<DataSetSpecification> expectedDataSetSpecifications = storageDataSetSpecifications;

            this.dataSetSpecificationServiceMock.Setup(broker =>
                broker.RetrieveAllDataSetSpecifications())
                    .Returns(storageDataSetSpecifications);

            // when
            DataSetSpecification actualDataSetSpecifications =
                 await this.dataSetSpecificationProcessingService.GetActiveDataSetSpecificationAsync(randomSupplierId);

            // then
            actualDataSetSpecifications.Should().BeEquivalentTo(expectedDataSetSpecifications);

            this.dataSetSpecificationServiceMock.Verify(broker =>
                broker.RetrieveAllDataSetSpecifications(),
                    Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
