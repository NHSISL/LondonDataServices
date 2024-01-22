// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ObjectColumns;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.ObjectColumns
{
    public partial class ObjectColumnProcessingServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllObjectColumns()
        {
            // given
            IQueryable<ObjectColumn> randomObjectColumns = CreateRandomObjectColumns();
            IQueryable<ObjectColumn> storageObjectColumns = randomObjectColumns;
            IQueryable<ObjectColumn> expectedObjectColumns = storageObjectColumns;

            this.objectColumnServiceMock.Setup(broker =>
                broker.RetrieveAllObjectColumns())
                    .Returns(storageObjectColumns);

            // when
            IQueryable<ObjectColumn> actualObjectColumns =
                this.objectColumnProcessingService.RetrieveAllObjectColumns();

            // then
            actualObjectColumns.Should().BeEquivalentTo(expectedObjectColumns);

            this.objectColumnServiceMock.Verify(broker =>
                broker.RetrieveAllObjectColumns(),
                    Times.Once);

            this.objectColumnServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
