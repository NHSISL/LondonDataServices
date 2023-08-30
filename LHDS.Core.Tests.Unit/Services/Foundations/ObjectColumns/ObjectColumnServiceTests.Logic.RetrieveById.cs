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
        public async Task ShouldRetrieveObjectColumnByIdAsync()
        {
            // given
            ObjectColumn randomObjectColumn = CreateRandomObjectColumn();
            ObjectColumn inputObjectColumn = randomObjectColumn;
            ObjectColumn storageObjectColumn = randomObjectColumn;
            ObjectColumn expectedObjectColumn = storageObjectColumn.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectObjectColumnByIdAsync(inputObjectColumn.Id))
                    .ReturnsAsync(storageObjectColumn);

            // when
            ObjectColumn actualObjectColumn =
                await this.objectColumnService.RetrieveObjectColumnByIdAsync(inputObjectColumn.Id);

            // then
            actualObjectColumn.Should().BeEquivalentTo(expectedObjectColumn);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectObjectColumnByIdAsync(inputObjectColumn.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}