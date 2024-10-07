// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.ConfigImportExportTool.Models.Foundations.ObjectColumns;
using Moq;
using Xunit;

namespace LHDS.ConfigImportExportTool.Tests.Unit.Services.Foundations.ObjectColumns
{
    public partial class ObjectColumnServiceTests
    {
        [Fact]
        public async Task ShouldRemoveObjectColumnByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputObjectColumnId = randomId;
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn();
            ObjectColumn storageObjectColumn = randomObjectColumn;
            ObjectColumn expectedInputObjectColumn = storageObjectColumn;
            ObjectColumn deletedObjectColumn = expectedInputObjectColumn;
            ObjectColumn expectedObjectColumn = deletedObjectColumn.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectObjectColumnByIdAsync(inputObjectColumnId))
                    .ReturnsAsync(storageObjectColumn);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteObjectColumnAsync(expectedInputObjectColumn))
                    .ReturnsAsync(deletedObjectColumn);

            // when
            ObjectColumn actualObjectColumn = await this.objectColumnService
                .RemoveObjectColumnByIdAsync(inputObjectColumnId);

            // then
            actualObjectColumn.Should().BeEquivalentTo(expectedObjectColumn);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(inputObjectColumnId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteObjectColumnAsync(expectedInputObjectColumn),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}