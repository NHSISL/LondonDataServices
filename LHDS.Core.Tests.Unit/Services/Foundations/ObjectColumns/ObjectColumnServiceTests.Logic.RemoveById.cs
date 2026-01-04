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
            string randomUserId = GetRandomStringWithLengthOf(50);
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn(randomDateTimeOffset, randomUserId);
            Guid inputObjectColumnId = randomObjectColumn.Id;
            ObjectColumn storageObjectColumn = randomObjectColumn;
            ObjectColumn ObjectColumWithDeleteAuditApplied = storageObjectColumn.DeepClone();
            ObjectColumWithDeleteAuditApplied.UpdatedBy = randomUserId.ToString();
            ObjectColumWithDeleteAuditApplied.UpdatedDate = randomDateTimeOffset;
            ObjectColumn updatedObjectColumn = storageObjectColumn;
            ObjectColumn deletedObjectColumn = updatedObjectColumn;
            ObjectColumn expectedObjectColumn = deletedObjectColumn.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectObjectColumnByIdAsync(inputObjectColumnId))
                    .ReturnsAsync(storageObjectColumn);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.ApplyRemoveAuditValuesAsync(storageObjectColumn))
                    .ReturnsAsync(ObjectColumWithDeleteAuditApplied);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.securityAuditBrokerMock.Setup(broker =>
                broker.GetUserIdAsync())
                    .ReturnsAsync(randomUserId);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateObjectColumnAsync(ObjectColumWithDeleteAuditApplied))
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

            this.securityAuditBrokerMock.Verify(broker =>
                broker.ApplyRemoveAuditValuesAsync(storageObjectColumn),
                    Times.Once);

            this.securityAuditBrokerMock.Verify(broker =>
                broker.GetUserIdAsync(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateObjectColumnAsync(ObjectColumWithDeleteAuditApplied),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteObjectColumnAsync(updatedObjectColumn),
                    Times.Once);

            this.securityAuditBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}