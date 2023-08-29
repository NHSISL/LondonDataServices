using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetSpecifications
{
    public partial class DataSetSpecificationServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveDataSetSpecificationByIdAsync()
        {
            // given
            DataSetSpecification randomDataSetSpecification = CreateRandomDataSetSpecification();
            DataSetSpecification inputDataSetSpecification = randomDataSetSpecification;
            DataSetSpecification storageDataSetSpecification = randomDataSetSpecification;
            DataSetSpecification expectedDataSetSpecification = storageDataSetSpecification.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetSpecificationByIdAsync(inputDataSetSpecification.Id))
                    .ReturnsAsync(storageDataSetSpecification);

            // when
            DataSetSpecification actualDataSetSpecification =
                await this.dataSetSpecificationService.RetrieveDataSetSpecificationByIdAsync(inputDataSetSpecification.Id);

            // then
            actualDataSetSpecification.Should().BeEquivalentTo(expectedDataSetSpecification);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetSpecificationByIdAsync(inputDataSetSpecification.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}