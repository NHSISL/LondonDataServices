// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldRemoveObjectColumnByIdAsync()
        {
            // Given
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn();
            ObjectColumn storageObjectColumn = randomObjectColumn;
            ObjectColumn expectedObjectColumn = storageObjectColumn.DeepClone();

            this.objectColumnServiceMock.Setup(service =>
                service.RemoveObjectColumnByIdAsync(randomObjectColumn.Id))
                    .ReturnsAsync(storageObjectColumn);

            // When
            ObjectColumn actualObjectColumn =
                await this.objectColumnProcessingService
                    .RemoveObjectColumnByIdAsync(randomObjectColumn.Id);

            // Then
            actualObjectColumn.Should().BeEquivalentTo(expectedObjectColumn);

            this.objectColumnServiceMock.Verify(service =>
                service.RemoveObjectColumnByIdAsync(randomObjectColumn.Id),
                    Times.Once);

            this.objectColumnServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
