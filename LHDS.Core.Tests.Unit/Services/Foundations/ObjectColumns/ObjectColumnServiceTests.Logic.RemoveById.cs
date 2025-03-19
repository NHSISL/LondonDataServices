// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.ObjectColumns;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ObjectColumns
{
    public partial class ObjectColumnServiceTests
    {
        [Fact]
        public async Task ShouldRemoveObjectColumnByIdAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            EntraUser randomEntraUser = CreateRandomEntraUser();
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn(randomDateTimeOffset, randomEntraUser.EntraUserId);
            Guid inputObjectColumnId = randomObjectColumn.Id;
            ObjectColumn storageObjectColumn = randomObjectColumn;
            ObjectColumn ingestionTrackingWithDeleteAuditApplied = storageObjectColumn.DeepClone();
            ingestionTrackingWithDeleteAuditApplied.UpdatedBy = randomEntraUser.EntraUserId.ToString();
            ingestionTrackingWithDeleteAuditApplied.UpdatedDate = randomDateTimeOffset;
            ObjectColumn updatedObjectColumn = storageObjectColumn;
            ObjectColumn deletedObjectColumn = updatedObjectColumn;
            ObjectColumn expectedObjectColumn = deletedObjectColumn.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityBrokerMock.Setup(broker =>
                broker.GetCurrentUserAsync())
                    .ReturnsAsync(randomEntraUser);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectObjectColumnByIdAsync(inputObjectColumnId))
                    .ReturnsAsync(storageObjectColumn);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateObjectColumnAsync(randomObjectColumn))
                    .ReturnsAsync(updatedObjectColumn);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteObjectColumnAsync(updatedObjectColumn))
                    .ReturnsAsync(deletedObjectColumn);

            // when
            ObjectColumn actualObjectColumn = await this.objectColumnService
                .RemoveObjectColumnByIdAsync(inputObjectColumnId);

            // then
            actualObjectColumn.Should().BeEquivalentTo(expectedObjectColumn);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(inputObjectColumnId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.securityBrokerMock.Verify(broker =>
                broker.GetCurrentUserAsync(),
                    Times.Exactly(2));

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateObjectColumnAsync(randomObjectColumn),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteObjectColumnAsync(updatedObjectColumn),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.securityBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}