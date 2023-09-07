using System.Linq;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetObjects
{
    public partial class DataSetObjectServiceTests
    {
        [Fact]
        public void ShouldReturnDataSetObjects()
        {
            // given
            IQueryable<SpecificationObject> randomDataSetObjects = CreateRandomDataSetObjects();
            IQueryable<SpecificationObject> storageDataSetObjects = randomDataSetObjects;
            IQueryable<SpecificationObject> expectedDataSetObjects = storageDataSetObjects;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDataSetObjects())
                    .Returns(storageDataSetObjects);

            // when
            IQueryable<SpecificationObject> actualDataSetObjects =
                this.dataSetObjectService.RetrieveAllDataSetObjects();

            // then
            actualDataSetObjects.Should().BeEquivalentTo(expectedDataSetObjects);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllDataSetObjects(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}