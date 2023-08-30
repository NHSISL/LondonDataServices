using System.Linq;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.DataSetObjects;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetObjects
{
    public partial class DataSetObjectServiceTests
    {
        [Fact]
        public void ShouldReturnDataSetObjects()
        {
            // given
            IQueryable<DataSetObject> randomDataSetObjects = CreateRandomDataSetObjects();
            IQueryable<DataSetObject> storageDataSetObjects = randomDataSetObjects;
            IQueryable<DataSetObject> expectedDataSetObjects = storageDataSetObjects;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllDataSetObjects())
                    .Returns(storageDataSetObjects);

            // when
            IQueryable<DataSetObject> actualDataSetObjects =
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