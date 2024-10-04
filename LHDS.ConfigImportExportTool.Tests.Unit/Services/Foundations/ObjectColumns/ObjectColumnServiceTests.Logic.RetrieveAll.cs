// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ObjectColumns;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Foundations.ObjectColumns
{
    public partial class ObjectColumnServiceTests
    {
        [Fact]
        public void ShouldReturnObjectColumns()
        {
            // given
            IQueryable<ObjectColumn> randomObjectColumns = CreateRandomObjectColumns();
            IQueryable<ObjectColumn> storageObjectColumns = randomObjectColumns;
            IQueryable<ObjectColumn> expectedObjectColumns = storageObjectColumns;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllObjectColumns())
                    .Returns(storageObjectColumns);

            // when
            IQueryable<ObjectColumn> actualObjectColumns =
                this.objectColumnService.RetrieveAllObjectColumns();

            // then
            actualObjectColumns.Should().BeEquivalentTo(expectedObjectColumns);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllObjectColumns(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}