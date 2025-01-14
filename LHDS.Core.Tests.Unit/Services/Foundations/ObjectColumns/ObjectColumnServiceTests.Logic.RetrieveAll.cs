// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ObjectColumns;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ObjectColumns
{
    public partial class ObjectColumnServiceTests
    {
        [Fact]
        public async Task ShouldReturnObjectColumns()
        {
            // given
            IQueryable<ObjectColumn> randomObjectColumns = CreateRandomObjectColumns();
            IQueryable<ObjectColumn> storageObjectColumns = randomObjectColumns;
            IQueryable<ObjectColumn> expectedObjectColumns = storageObjectColumns;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllObjectColumnsAsync())
                    .ReturnsAsync(storageObjectColumns);

            // when
            IQueryable<ObjectColumn> actualObjectColumns =
                await this.objectColumnService.RetrieveAllObjectColumnsAsync();

            // then
            actualObjectColumns.Should().BeEquivalentTo(expectedObjectColumns);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllObjectColumnsAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}