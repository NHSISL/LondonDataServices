// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetSpecifications
{
    public partial class DataSetSpecificationServiceTests
    {
        [Fact]
        public async Task ShouldReturnDataSetSpecifications()
        {
            // given
            IQueryable<DataSetSpecification> randomDataSetSpecifications = CreateRandomDataSetSpecifications();
            IQueryable<DataSetSpecification> storageDataSetSpecifications = randomDataSetSpecifications;
            IQueryable<DataSetSpecification> expectedDataSetSpecifications = storageDataSetSpecifications;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDataSetSpecificationsAsync())
                    .ReturnsAsync(storageDataSetSpecifications);

            // when
            IQueryable<DataSetSpecification> actualDataSetSpecifications =
                await this.dataSetSpecificationService.RetrieveAllDataSetSpecificationsAsync();

            // then
            actualDataSetSpecifications.Should().BeEquivalentTo(expectedDataSetSpecifications);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDataSetSpecificationsAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}