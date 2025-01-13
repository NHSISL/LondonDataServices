// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ObjectColumns;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.ObjectColumns
{
    public partial class ObjectColumnProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAllObjectColumns()
        {
            // given
            IQueryable<ObjectColumn> randomObjectColumns = CreateRandomObjectColumns();
            IQueryable<ObjectColumn> storageObjectColumns = randomObjectColumns;
            IQueryable<ObjectColumn> expectedObjectColumns = storageObjectColumns;

            this.objectColumnServiceMock.Setup(broker =>
                broker.RetrieveAllObjectColumnsAsync())
                    .ReturnsAsync(storageObjectColumns);

            // when
            IQueryable<ObjectColumn> actualObjectColumns =
                await this.objectColumnProcessingService.RetrieveAllObjectColumnsAsync();

            // then
            actualObjectColumns.Should().BeEquivalentTo(expectedObjectColumns);

            this.objectColumnServiceMock.Verify(broker =>
                broker.RetrieveAllObjectColumnsAsync(),
                    Times.Once);

            this.objectColumnServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
