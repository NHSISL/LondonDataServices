// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.ObjectColumns;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.ObjectColumns
{
    public partial class ObjectColumnProcessingServiceTests
    {
        [Fact]
        public async Task ShouldModifyObjectColumnIfOneExistsAndNotAddAsync()
        {
            // Given
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn();
            ObjectColumn storageObjectColumn = randomObjectColumn;
            ObjectColumn modifiedObjectColumn = storageObjectColumn.DeepClone();
            modifiedObjectColumn.UpdatedDate = DateTimeOffset.UtcNow;
            ObjectColumn updatedObjectColumn = modifiedObjectColumn.DeepClone();
            ObjectColumn expectedObjectColumn = updatedObjectColumn;

            this.objectColumnServiceMock.Setup(service =>
                service.RetrieveObjectColumnByIdAsync(modifiedObjectColumn.Id))
                    .ReturnsAsync(value: storageObjectColumn);

            this.objectColumnServiceMock.Setup(service =>
                service.ModifyObjectColumnAsync(modifiedObjectColumn))
                    .ReturnsAsync(value: updatedObjectColumn);

            // When
            ObjectColumn actualObjectColumn = await this.objectColumnProcessingService
                .ModifyOrAddObjectColumnAsync(modifiedObjectColumn);

            // Then
            actualObjectColumn.Should().BeEquivalentTo(expectedObjectColumn);

            this.objectColumnServiceMock.Verify(service =>
                service.RetrieveObjectColumnByIdAsync(modifiedObjectColumn.Id),
                    Times.Once);

            this.objectColumnServiceMock.Verify(service =>
                service.ModifyObjectColumnAsync(modifiedObjectColumn),
                    Times.Once);

            this.objectColumnServiceMock.Verify(service =>
                service.AddObjectColumnAsync(modifiedObjectColumn),
                    Times.Never);

            this.objectColumnServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
