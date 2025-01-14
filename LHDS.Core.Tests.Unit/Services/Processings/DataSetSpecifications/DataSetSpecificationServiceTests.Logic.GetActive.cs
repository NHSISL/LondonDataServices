// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
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
            Guid randomSupplierId = Guid.NewGuid();
            DataSet randomDataSet = CreateRandomDataSet(randomSupplierId);

            IQueryable<DataSetSpecification> randomDataSetSpecifications =
                CreateRandomDataSetSpecifications(dataSet: randomDataSet, dataSetId: randomDataSet.Id, count: 1);

            IQueryable<DataSetSpecification> storageDataSetSpecifications = randomDataSetSpecifications;
            IQueryable<DataSetSpecification> expectedDataSetSpecifications = storageDataSetSpecifications.DeepClone();
            DataSetSpecification expectedDataSetSpecification = expectedDataSetSpecifications.First();

            this.dataSetSpecificationServiceMock.Setup(broker =>
                broker.RetrieveAllDataSetSpecificationsAsync())
                    .ReturnsAsync(storageDataSetSpecifications);

            // when
            DataSetSpecification actualDataSetSpecification =
                 await this.dataSetSpecificationProcessingService.GetActiveDataSetSpecification(randomSupplierId);

            // then
            actualDataSetSpecification.Should().BeEquivalentTo(expectedDataSetSpecification);

            this.dataSetSpecificationServiceMock.Verify(broker =>
                broker.RetrieveAllDataSetSpecificationsAsync(),
                    Times.Once);

            this.dataSetSpecificationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
