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
        public async Task ShouldAddObjectColumnAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            ObjectColumn randomObjectColumn = CreateRandomObjectColumn(randomDateTimeOffset);
            ObjectColumn inputObjectColumn = randomObjectColumn;
            ObjectColumn storageObjectColumn = inputObjectColumn;
            ObjectColumn expectedObjectColumn = storageObjectColumn.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertObjectColumnAsync(inputObjectColumn))
                    .ReturnsAsync(storageObjectColumn);

            // when
            ObjectColumn actualObjectColumn = await this.objectColumnService
                .AddObjectColumnAsync(inputObjectColumn);

            // then
            actualObjectColumn.Should().BeEquivalentTo(expectedObjectColumn);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertObjectColumnAsync(inputObjectColumn),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}