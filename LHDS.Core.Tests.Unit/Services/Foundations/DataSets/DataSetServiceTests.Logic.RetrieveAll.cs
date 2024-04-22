// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSets;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSets
{
    public partial class DataSetServiceTests
    {
        [Fact]
        public void ShouldReturnDataSets()
        {
            // given
            IQueryable<DataSet> randomDataSets = CreateRandomDataSets();
            IQueryable<DataSet> storageDataSets = randomDataSets;
            IQueryable<DataSet> expectedDataSets = storageDataSets;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDataSets())
                    .Returns(storageDataSets);

            // when
            IQueryable<DataSet> actualDataSets =
                this.dataSetService.RetrieveAllDataSets();

            // then
            actualDataSets.Should().BeEquivalentTo(expectedDataSets);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDataSets(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}