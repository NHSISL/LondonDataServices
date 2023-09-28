// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
        public async Task ShouldAddObjectColumnAsync()
        {
            // Given
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn();
            ObjectColumn inputObjectColumn = randomObjectColumn;
            ObjectColumn storageObjectColumn = inputObjectColumn;
            ObjectColumn expectedObjectColumn = storageObjectColumn.DeepClone();

            this.objectColumnServiceMock.Setup(service =>
                service.AddObjectColumnAsync(inputObjectColumn))
                    .ReturnsAsync(storageObjectColumn);

            // When
            ObjectColumn actualObjectColumn = await this.objectColumnProcessingService
                .AddObjectColumnAsync(inputObjectColumn);

            // Then
            actualObjectColumn.Should().BeEquivalentTo(expectedObjectColumn);

            this.objectColumnServiceMock.Verify(service =>
                service.AddObjectColumnAsync(inputObjectColumn),
                    Times.Once);

            this.objectColumnServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
