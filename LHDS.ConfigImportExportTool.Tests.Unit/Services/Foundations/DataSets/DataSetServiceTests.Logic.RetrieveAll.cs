// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.Datasets;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSets
{
    public partial class DataSetServiceTests
    {
        [Fact]
        public async Task ShouldReturnDataSets()
        {
            // given
            IQueryable<DataSet> randomDataSets = CreateRandomDataSets();
            IQueryable<DataSet> storageDataSets = randomDataSets;
            IQueryable<DataSet> expectedDataSets = storageDataSets;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDataSetsAsync())
                    .ReturnsAsync(storageDataSets);

            // when
            IQueryable<DataSet> actualDataSets =
                await this.dataSetService.RetrieveAllDataSetsAsync();

            // then
            actualDataSets.Should().BeEquivalentTo(expectedDataSets);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDataSetsAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}