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
        public async Task ShouldModifyObjectColumnAsync()
        {
            // Given
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn();
            ObjectColumn inputObjectColumn = randomObjectColumn;
            ObjectColumn storageObjectColumn = inputObjectColumn;
            ObjectColumn expectedObjectColumn = storageObjectColumn.DeepClone();

            this.objectColumnServiceMock.Setup(service =>
                service.ModifyObjectColumnAsync(inputObjectColumn))
                    .ReturnsAsync(storageObjectColumn);

            // When
            await this.objectColumnProcessingService.ModifyObjectColumnAsync(inputObjectColumn);

            // Then
            this.objectColumnServiceMock.Verify(service =>
                service.ModifyObjectColumnAsync(inputObjectColumn),
                    Times.Once);

            this.objectColumnServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
