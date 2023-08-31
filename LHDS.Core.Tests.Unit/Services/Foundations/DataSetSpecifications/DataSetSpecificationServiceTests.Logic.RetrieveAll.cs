using System.Linq;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.DataSetSpecifications;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetSpecifications
{
    public partial class DataSetSpecificationServiceTests
    {
        [Fact]
        public void ShouldReturnDataSetSpecifications()
        {
            // given
            IQueryable<DataSetSpecification> randomDataSetSpecifications = CreateRandomDataSetSpecifications();
            IQueryable<DataSetSpecification> storageDataSetSpecifications = randomDataSetSpecifications;
            IQueryable<DataSetSpecification> expectedDataSetSpecifications = storageDataSetSpecifications;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDataSetSpecifications())
                    .Returns(storageDataSetSpecifications);

            // when
            IQueryable<DataSetSpecification> actualDataSetSpecifications =
                this.dataSetSpecificationService.RetrieveAllDataSetSpecifications();

            // then
            actualDataSetSpecifications.Should().BeEquivalentTo(expectedDataSetSpecifications);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDataSetSpecifications(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}