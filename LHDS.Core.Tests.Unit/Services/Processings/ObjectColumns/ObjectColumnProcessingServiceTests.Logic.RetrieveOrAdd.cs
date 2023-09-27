// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.ObjectColumns;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.ObjectColumns
{
    public partial class ObjectColumnProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveObjectColumnIfOneExistsAndNotAddAsync()
        {
            // Given
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn();
            ObjectColumn inputObjectColumn = randomObjectColumn;
            ObjectColumn storageObjectColumn = inputObjectColumn;
            ObjectColumn expectedObjectColumn = storageObjectColumn.DeepClone();

            this.objectColumnServiceMock.Setup(service =>
                service.RetrieveObjectColumnByIdAsync(inputObjectColumn.Id))
                    .ReturnsAsync(value: storageObjectColumn);

            // When
            await this.objectColumnProcessingService.RetrieveOrAddObjectColumnAsync(inputObjectColumn);

            // Then
            this.objectColumnServiceMock.Verify(service =>
                service.RetrieveObjectColumnByIdAsync(inputObjectColumn.Id),
                    Times.Once);

            this.objectColumnServiceMock.Verify(service =>
                service.AddObjectColumnAsync(inputObjectColumn),
                    Times.Never);

            this.objectColumnServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
