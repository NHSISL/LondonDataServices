// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using FluentAssertions;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Foundations.ObjectColumns
{
    public partial class ObjectColumnServiceTests
    {
        [Fact]
        public async void ShouldReturnObjectColumns()
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