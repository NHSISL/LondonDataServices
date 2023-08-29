using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.ObjectColumns;
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

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateObjectColumnAsync(inputObjectColumn))
                    .ReturnsAsync(updatedObjectColumn);

            // when
            ObjectColumn actualObjectColumn =
                await this.objectColumnService.ModifyObjectColumnAsync(inputObjectColumn);

            // then
            actualObjectColumn.Should().BeEquivalentTo(expectedObjectColumn);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateObjectColumnAsync(inputObjectColumn),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}