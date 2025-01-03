// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSets;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.DataSets
{
    public partial class DataSetProcessingServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllDataSets()
        {
            // given
            IQueryable<DataSet> randomDataSets = CreateRandomDataSets();
            IQueryable<DataSet> storageDataSets = randomDataSets;
            IQueryable<DataSet> expectedDataSets = storageDataSets;

            this.dataSetServiceMock.Setup(broker =>
                broker.RetrieveAllDataSetsAsync())
                    .ReturnsAsync(storageDataSets);

            // when
            IQueryable<DataSet> actualDataSets =
                this.dataSetProcessingService.RetrieveAllDataSets();

            // then
            actualDataSets.Should().BeEquivalentTo(expectedDataSets);

            this.dataSetServiceMock.Verify(broker =>
                broker.RetrieveAllDataSetsAsync(),
                    Times.Once);

            this.dataSetServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}