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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertObjectColumnAsync(inputObjectColumn))
                    .ReturnsAsync(storageObjectColumn);

            // when
            ObjectColumn actualObjectColumn = await this.objectColumnService
                .AddObjectColumnAsync(inputObjectColumn);

            // then
            actualObjectColumn.Should().BeEquivalentTo(expectedObjectColumn);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertObjectColumnAsync(inputObjectColumn),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}