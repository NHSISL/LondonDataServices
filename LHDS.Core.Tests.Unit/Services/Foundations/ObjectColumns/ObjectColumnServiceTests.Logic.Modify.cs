// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.ObjectColumns;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ObjectColumns
{
    public partial class ObjectColumnServiceTests
    {
        [Fact]
        public async Task ShouldModifyObjectColumnAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ObjectColumn randomObjectColumn = CreateRandomModifyObjectColumn(randomDateTimeOffset);
            ObjectColumn inputObjectColumn = randomObjectColumn;
            ObjectColumn storageObjectColumn = inputObjectColumn.DeepClone();
            storageObjectColumn.UpdatedDate = randomObjectColumn.CreatedDate;
            ObjectColumn updatedObjectColumn = inputObjectColumn;
            ObjectColumn expectedObjectColumn = updatedObjectColumn.DeepClone();
            Guid objectColumnId = inputObjectColumn.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectObjectColumnByIdAsync(objectColumnId))
                    .ReturnsAsync(storageObjectColumn);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateObjectColumnAsync(inputObjectColumn))
                    .ReturnsAsync(updatedObjectColumn);

            // when
            ObjectColumn actualObjectColumn =
                await this.objectColumnService.ModifyObjectColumnAsync(inputObjectColumn);

            // then
            actualObjectColumn.Should().BeEquivalentTo(expectedObjectColumn);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(inputObjectColumn.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateObjectColumnAsync(inputObjectColumn),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}