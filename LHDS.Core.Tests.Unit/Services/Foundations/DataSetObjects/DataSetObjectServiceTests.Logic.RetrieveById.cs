using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.SpecificationObjects;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.DataSetObjects
{
    public partial class DataSetObjectServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveDataSetObjectByIdAsync()
        {
            // given
            SpecificationObject randomDataSetObject = CreateRandomDataSetObject();
            SpecificationObject inputDataSetObject = randomDataSetObject;
            SpecificationObject storageDataSetObject = randomDataSetObject;
            SpecificationObject expectedDataSetObject = storageDataSetObject.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectDataSetObjectByIdAsync(inputDataSetObject.Id))
                    .ReturnsAsync(storageDataSetObject);

            // when
            SpecificationObject actualDataSetObject =
                await this.dataSetObjectService.RetrieveDataSetObjectByIdAsync(inputDataSetObject.Id);

            // then
            actualDataSetObject.Should().BeEquivalentTo(expectedDataSetObject);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectDataSetObjectByIdAsync(inputDataSetObject.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}